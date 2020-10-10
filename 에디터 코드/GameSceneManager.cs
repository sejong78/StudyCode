using Champion.UI;
using Champion.Util;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Champion.SceneManagement
{
    public delegate void SceneChangeEvent(GameSceneData scene);

    /// <summary>
    /// 화면 전환 기능을 관리합니다.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        private readonly string[] TOP_MENU_NAMES = { "CharacterMode", "GearEnterMode", "SkillEnterMode", "SocialMode", "InAppMode", "AchievementsMode" , "ClanHub" , "ClanCreateMode" };

        private class BackupData : IBackup
        {
            /// <summary>
            /// 씬 이름
            /// </summary>
            public string sceneName;

            /// <summary>
            /// UI 백업 정보
            /// </summary>
            public IBackup ui;

            /// <summary>
            /// 게임 모드 백업 정보
            /// </summary>
            public IBackup gameMode;
        }

        /// <summary>
        /// 처리해야 하는일을 플래그로 관리합니다.
        /// </summary>
        [System.Flags]
        private enum DoFlag
        {
            None = 0,
            Tween = 1 << 0,
            Fade = 1 << 1,
            All = Tween | Fade,
        }

        #region Members

        /// <summary>
        /// 화면 전환 시작시 이벤트
        /// </summary>
        public event SceneChangeEvent onChangeBegin;

        /// <summary>
        /// 화면 전환 이벤트
        /// </summary>
        public event SceneChangeEvent onChanged;

        /// <summary>
        /// 화면 전환 완료시 이벤트 처리
        /// 페이드 연출 및 진입 연출까지 완료된 시점을 화면 전환 완료로 봅니다.
        /// </summary>
        public event SceneChangeEvent onChangeEnd;

        private GameSceneData _SceneData = new GameSceneData();

        /// <summary>
        /// 화면 정보 테이블
        /// </summary>
        private GameSceneTable sceneTable;

        /// <summary>
        /// 화면 이동 기록
        /// </summary>
        private Stack<BackupData> sceneHistory = new Stack<BackupData>();

        private BackgroundManager backgroundManager;
        private TargetObjectContainer targetObjects;

        /// <summary>
        /// 뒤로 가기시 처리할 화면 복원 정보
        /// </summary>
        private BackupData restoreScene;

        /// <summary>
        /// 화면 전환 처리 대상들
        /// </summary>
        private List<IGameSceneHandler> sceneHandlers = new List<IGameSceneHandler>();

        /// <summary>
        /// 화면 전환시 페이드 인/아웃 연출을 사용합니다.
        /// </summary>
        private bool useFade;
        private Intent SceneChangeContext = null;
        private DoFlag todos;

        #endregion Members

        #region Properties

        /// <summary>
        /// 현재 사용중인 화면 정보
        /// </summary>
        public GameSceneData sceneData
        {
            get
            {
                return _SceneData;
            }
            private set
            {
                /// 레퍼런스를 복사할 경우 원본 정보가 훼손될 수 있으므로 값을 복사합니다.
                _SceneData.Set(value);
                cameraMode = sceneTable.GetCameraMode(value.cameraMode);
#if UNITY_EDITOR
                if (cameraMode == null && !string.IsNullOrEmpty(value.cameraMode))
                {
                    Debug.LogError(string.Format("[SCENE] 카메라 모드 정보가 없습니다. ({0}.{1})", _SceneData.name, _SceneData.cameraMode));
                }
#endif
            }
        }

        /// <summary>
        /// 현재 화면 이름
        /// </summary>
        public string SceneName
        {
            get
            {
                return sceneData != null ? sceneData.name : string.Empty;
            }
        }

        public CameraMode cameraMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 뒤로 가기가 가능한지 확인합니다.
        /// </summary>
        public bool canBack
        {
            get { return sceneHistory.Count > 0; }
        }

        /// <summary>
        /// 화면 전환 처리가 완료 됐는지 확인합니다.
        /// </summary>
        public bool isDone
        {
            get
            {
                return _IsDone;
            }
            private set
            {
                _IsDone = value;
            }
        }

        private bool _IsDone = true;

        #endregion Properties

        #region ReservePopup기능 (특정씬에 진입시 예약된팝업을 띄울 수 있는 기능)

        private Dictionary<GameSceneType, PopupBunch> _reservedPopup;

        private Dictionary<GameSceneType, PopupBunch> reservedPopup
        {
            get
            {
                if (_reservedPopup == null)
                {
                    _reservedPopup = new Dictionary<GameSceneType, PopupBunch>();
                }
                return _reservedPopup;
            }

            set
            {
                Debug.LogError("readonly pls...");
            }
        }

        public struct PopupBunch
        {
            public string popupName;
            public Intent it;
            public bool showBackground;
        }

        public void ReservePopup(GameSceneType type, string popupName, Intent it, bool showBackground)
        {
            PopupBunch bunch = new PopupBunch();
            bunch.popupName = popupName;
            bunch.showBackground = showBackground;
            bunch.it = it;

            reservedPopup[type] = bunch;
        }

        #endregion ReservePopup기능 (특정씬에 진입시 예약된팝업을 띄울 수 있는 기능)

        public Intent GetContext()
        {
            return SceneChangeContext;
        }

        public void Preload(string sceneName)
        {
            var data = sceneTable.GetScene(sceneName);
            if (data != null)
            {
                UIPanelManager.instance.Preload(data.panelName);
            }
        }

        #region 화면 이동

        public void Goto(string sceneName, Intent it)
        {
            if (!isDone)
            {
#if DEBUG
                Debug.LogError("화면 전환 중 요청이 들어와 무시합니다.");
#endif
                return;
            }

            var scene = sceneTable.GetScene(sceneName);
            if (scene == null)
            {
#if UNITY_EDITOR
                Debug.Log(string.Format("[SCENE] No Exist: {0}", sceneName));
#endif
                return;
            }

            /// 현재 선택된 화면일 경우 화면 전환 무시
            string currentScene = sceneData.name;
            if (string.IsNullOrEmpty(currentScene) == false)
            {
                bool found = currentScene.Equals(sceneName);
                if (found)
                {
#if UNITY_EDITOR
                Debug.Log(string.Format("[SCENE] Screen Duplication: {0}", scene));
#endif
                    return;
                }
            }

            /// 현재 씬으로 돌아올 수 있다면 백업합니다.
            if (sceneData.canBack)
            {
                Backup();
            }
            /// 이동할 씬이 메인 화면이라면 뒤로 가기 정보를 초기화합니다.
            if (scene.resetBackup)
            {
                ResetBackup();
                if (TOP_MENU_NAMES.Any(t => t.Equals(scene.name, StringComparison.OrdinalIgnoreCase)))
                {
                    ResetBackup();
                    BackupData backup = new BackupData();

                    backup.sceneName = "Menu";
                    backup.ui = UIManager.instance.Backup();
                    backup.gameMode = GameModeManager.instance.Backup();
                    sceneHistory.Push(backup);
                }
            }

            SceneChangeContext = it;

#if UNITY_EDITOR
            Debug.Log(string.Format("[SCENE] GOTO: {0}", sceneName));
#endif
            Change(scene);
        }

		/// <summary>
		/// 현재 씬을 백업하지 않고, 씬을 이동합니다.
		/// </summary>
		/// <param name="sceneName"></param>
		/// <param name="it"></param
		public void Penetration( string sceneName, Intent it )
		{
			if( isDone == false )
			{
#if DEBUG
				Debug.LogError( "화면 전환 중 요청이 들어와 무시합니다." );
#endif
				return;
			}

			var scene = sceneTable.GetScene(sceneName);
			if( scene == null )
			{
#if UNITY_EDITOR
                Debug.Log(string.Format("[SCENE] No Exist: {0}", sceneName));
#endif
				return;
			}

			/// 현재 선택된 화면일 경우 화면 전환 무시
			string currentScene = sceneData.name;
			if( string.IsNullOrEmpty( currentScene ) == false )
			{
				bool found = currentScene.Equals(sceneName);
				if( found )
				{
#if UNITY_EDITOR
                Debug.Log(string.Format("[SCENE] Screen Duplication: {0}", scene));
#endif
					return;
				}
			}

			/// 이동할 씬이 메인 화면이라면 뒤로 가기 정보를 초기화합니다.
			if( scene.resetBackup == true )
			{
				ResetBackup();
				if( TOP_MENU_NAMES.Any( t => t.Equals( scene.name, StringComparison.OrdinalIgnoreCase ) ) )
				{
					ResetBackup();
					BackupData backup = new BackupData();

					backup.sceneName = "Menu";
					backup.ui = UIManager.instance.Backup();
					backup.gameMode = GameModeManager.instance.Backup();
					sceneHistory.Push( backup );
				}
			}

			SceneChangeContext = it;

#if UNITY_EDITOR
            Debug.Log(string.Format("[SCENE] Penetration: {0}", sceneName));
#endif
			Change( scene );
		}


		/// <summary>
		/// 특정 화면으로 뒤로 가기 처리를 합니다.
		/// 뒤로 가기 목록에 지정한 화면이 없을 경우 아무런 처리를 하지 않습니다.
		/// </summary>
		/// <param name="sceneName"></param>
		/// <returns></returns>
		public bool Back(string sceneName, Intent context = null)
        {
            bool exist = sceneHistory.Any<BackupData>(backup => sceneName.Equals(backup.sceneName));
            if (!exist)
            {
                return false;
            }

            bool found = false;
            do
            {
                var backup = sceneHistory.Peek();
                found = sceneName.Equals(backup.sceneName);
                if (!found)
                {
                    sceneHistory.Pop();
                }
            } while (!found);

            return Back(context);
        }

        /// <summary>
        /// 뒤로 가기 처리를 합니다.
        /// </summary>
        public bool Back(Intent context = null)
        {
            /// 화면 전환중일 경우에는 무시합니다.
            if (!isDone)
            {
                return false;
            }

            /// 게임 모드에 따라 다르게 처리해야 합니다.
            /// TODO: 인게임 중일 경우 일시 정지 처리를 해야 합니다.
            if ((GameManager.playing && !GameManager.instance.isNewReplayPlaying) || GameManager.paused)
            {
                return false;
            }

            if (sceneHistory.Count == 0)
            {
                return false;
            }

            restoreScene = sceneHistory.Pop();
            if (restoreScene == null)
            {
                return false;
            }

#if UNITY_EDITOR
            Debug.Log(string.Format("[SCENE] BACK: {0}", restoreScene.sceneName));
#endif
            var scene = sceneTable.GetScene(restoreScene.sceneName);
            SceneChangeContext = context;
            Change(scene);
            return true;
        }

		public int FindSceneIndexSceneHistory( string targetSceneName )
		{
			int rtnIndex = sceneHistory.Count;

			foreach( var sh in sceneHistory )
			{
				if( string.Compare( targetSceneName, sh.sceneName ) == 0 )
					return rtnIndex;

				--rtnIndex;
			}

			return rtnIndex;
		}

		/// <summary>
		/// 씬 이동 정보중 n개 만 남기고 다 날린다.
		/// </summary>
		public void RemoveLastSceneHistory( int remainSceneHistoryCount = 1 )
		{
			// 로직상 최소 하나의 씬( resetBack 이 true 인 )은 있어야 한다. 
			remainSceneHistoryCount = Mathf.Max( 1, remainSceneHistoryCount );

			while( remainSceneHistoryCount < sceneHistory.Count )
			{
				sceneHistory.Pop();
			}

		}

		#endregion 화면 이동

		#region Backup

		private void Backup()
        {
            BackupData backup = new BackupData();

            backup.sceneName = sceneData.name;
            backup.ui = UIManager.instance.Backup();
            backup.gameMode = GameModeManager.instance.Backup();

            sceneHistory.Push(backup);
#if UNITY_EDITOR
            Debug.Log("[SCENE] Scene Backup");
#endif
        }

        private void ResetBackup()
        {
#if UNITY_EDITOR
            Debug.Log("[SCENE] Reset Scene Backup");
#endif
            sceneHistory.Clear();
        }

        private void Restore()
        {
            if (restoreScene == null)
            {
                return;
            }

            UIManager.instance.Restore(restoreScene.ui);
            GameModeManager.instance.Restore(restoreScene.gameMode);
            restoreScene = null;
#if UNITY_EDITOR
            Debug.Log("[SCENE] Scene Restore");
#endif
        }

        #endregion Backup

        #region 이벤트

        private void Awake()
        {
            /// 게임씬에 관련된 정보들을 초기화합니다.
            sceneTable = GameSceneTable.Load();
#if DEBUG
            if (sceneTable == null)
            {
                Debug.LogError("GameSceneTable 로드에 실패하였습니다.");
            }
#endif

            backgroundManager = GetComponent<BackgroundManager>();
            if (backgroundManager == null)
            {
                backgroundManager = gameObject.AddComponent<BackgroundManager>();
            }

            targetObjects = new TargetObjectContainer();
            backgroundManager.onLoaded += targetObjects.OnSceneLoaded;
            backgroundManager.onUnloaded += targetObjects.OnSceneUnloaded;

            isDone = true;
        }

        private void OnDestroy()
        {
            if (backgroundManager != null && targetObjects != null)
            {
                backgroundManager.onLoaded -= targetObjects.OnSceneLoaded;
                backgroundManager.onUnloaded -= targetObjects.OnSceneUnloaded;
            }
        }

        private void Start()
        {
            sceneHandlers.Add(GameModeManager.instance);
            sceneHandlers.Add(UIManager.instance);
            sceneHandlers.Add(backgroundManager);

            if (targetObjects != null)
            {
                targetObjects.Init();
            }
        }

        private void OnFadeIn()
        {
            OnPostChange();
        }

        /// <summary>
        /// 화면 전환 완료시 이벤트 처리를 합니다.
        /// </summary>
        private void OnChangeScene()
        {
#if CRASH_LOG
            CrashTest.Log("OnChangeScene");
#endif

            SceneChangeContext = null;
            isDone = true;

            SoundManager.OnSceneEnter(sceneData);

            /// onChangeEnd 로 GameMode 나 UIManager 에서 처리하는게 좋지 않을까..
            GameSceneType type;
            if (EnumUtil<GameSceneType>.Instance.TryParse(sceneData.name, out type))
            {
                PopupBunch bunch;
                if (reservedPopup.TryGetValue(type, out bunch))
                {
                    UIPopupUtil.ShowPopup(bunch.popupName, bunch.it, bunch.showBackground);
                    reservedPopup.Remove(type);
                }
            }

            if (onChangeEnd != null)
            {
                onChangeEnd(sceneData);
            }
        }

        private void OnFadeOut()
        {
            OnPreChange();
        }

        #endregion 이벤트

        #region 화면 전환 (내부 처리)

        private void Change(GameSceneData data)
        {
            Assert.IsNotNull<GameSceneData>(data);
            if (data == null)
            {
#if UNITY_EDITOR
                Debug.LogError("[SCENE] 화면 정보가 없습니다.");
#endif
                return;
            }

            if (!isDone)
            {
#if UNITY_EDITOR
                Debug.Log("[SCENE] 화면 전환 중이므로 요청을 무시합니다.");
#endif
                return;
            }

            isDone = false;

            sceneData = data;
            sceneData.backgroundName = GameModeManager.instance.GetBackgroundName(data);

            var background = sceneTable.GetBackground(sceneData.backgroundName);
            // 환경음 초기화. 보통은 없다.
            sceneData.ambienceSound = string.Empty;
            if (background != null)
            {
                if (string.IsNullOrEmpty(sceneData.bgm))
                {
                    sceneData.bgm = background.bgm;
                }

                sceneData.ambienceSound = background.ambienceSound;

                if (FlashEffectManager.available)
                {
                    FlashEffectManager.instance.use = background.useFlash;
                }
            }

            /// 배경씬 변경이 필요한지 확인합니다.
            bool changeBackground = backgroundManager.ShouldChange(sceneData.backgroundName);

            /// 페이드 아웃이 필요한 경우 처리합니다.
            useFade = data.useFade || changeBackground;

            SoundManager.OnSceneLeave(sceneData);
            ResetTodo();
            OnPreChange();
        }

        public void AddBackground(string backgroundName, Action<Scene> callback)
        {
            UnityEngine.Events.UnityAction<Scene, LoadSceneMode> onLoaded = null;

            onLoaded = (scene, mode) =>
            {
                backgroundManager.onLoaded -= onLoaded;

                if (callback != null)
                {
                    callback(scene);
                }
            };

            backgroundManager.onLoaded += onLoaded;

            backgroundManager.Add(backgroundName);
        }

        /// <summary>
        /// 화면 전환전 처리해야 항목들을 제어합니다.
        /// 퇴장 연출 -> 페이드 아웃
        /// </summary>
        private void OnPreChange()
        {
            if (DoTween(true))
            {
                return;
            }

            if (DoFade(true))
            {
                return;
            }

            ChangeScene();
        }

        /// <summary>
        /// 작업을 처리했는지 확인합니다.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        private bool GetDone(DoFlag flag)
        {
            return (todos & flag) == 0;
        }

        /// <summary>
        /// 작업 처리를 설정합니다.
        /// </summary>
        /// <param name="flag"></param>
        private void SetDone(DoFlag flag)
        {
            todos &= ~flag;
        }

        private void ResetTodo()
        {
            todos = DoFlag.All;
        }

        private bool DoTween(bool leave)
        {
            if (GetDone(DoFlag.Tween))
            {
                return false;
            }

            SetDone(DoFlag.Tween);
            if (cameraMode != null)
            {
                string id = leave ? cameraMode.tweenIdLeave : cameraMode.tweenIdEnter;
                if (!string.IsNullOrEmpty(id))
                {
                    int count = DOTween.Restart(id);
                    if (count > 0)
                    {
                        var playings = DOTween.PlayingTweens();
                        float duration = GetMaxDuration(playings, id);

                        if (duration == 0f)
                        {
                            return false;
                        }

                        StartCoroutine(WaitTweenComplete(duration, leave));
                        return true;
                    }
                }
            }
            return false;
        }

        private float GetMaxDuration(List<Tween> tweens, string id = null)
        {
            float duration = 0f;
            bool checkId = !string.IsNullOrEmpty(id);

            if (tweens != null && tweens.Count > 0)
            {
                foreach (var tween in tweens)
                {
                    if (checkId && !id.Equals(tween.id))
                    {
                        continue;
                    }

                    // by yg.
                    // 꺼져 있는 tween 의 시간은 추가 하지 않는다.
                    GameObject obj = tween.target as GameObject;
                    if (obj != null)
                    {
                        if (!obj.activeInHierarchy)
                        {
                            continue;
                        }
                    }

                    duration = Mathf.Max(duration, tween.Duration(false) + tween.Delay());
                }
            }

            return duration;
        }

        private bool DoFade(bool fadeOut)
        {
            if (GetDone(DoFlag.Fade))
            {
                return false;
            }

            SetDone(DoFlag.Fade);
            if (useFade)
            {
                if (fadeOut)
                {
                    UIManager.instance.Fade(true, OnFadeOut);
                }
                else
                {
                    UIManager.instance.Fade(false, OnFadeIn);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 화면 퇴장 연출이 종료됐는지 확인합니다.
        /// </summary>
        /// <returns></returns>
        private System.Collections.IEnumerator WaitTweenComplete(float duration, bool leave)
        {
            if (duration > 0f)
            {
                yield return YieldInstructionCache.WaitForSeconds(duration);
            }

            if (leave)
            {
                OnPreChange();
            }
            else
            {
                OnPostChange();
            }
            yield break;
        }

        /// <summary>
        /// 화면 퇴장 연출 종료시 처리를 합니다.
        /// </summary>
        private void OnSceneLeave()
        {
            OnPreChange();
        }

        private void OnPostChange()
        {
            if (DoFade(false))
            {
                return;
            }

            if (DoTween(false))
            {
                return;
            }

            OnChangeScene();
        }

        /// <summary>
        /// 카메라를 변경합니다.
        /// </summary>
        public void ChangeCamera()
        {
            if (cameraMode == null)
            {
                return;
            }

            Camera camera = null;
            switch (cameraMode.type)
            {
                case CameraSelectType.Auto:
                    {
                        camera = FindMainCameraFromActiveScene();
                    }
                    break;

                case CameraSelectType.Manual:
                    {
                        var go = targetObjects.FindTarget(TargetTag.MainCamera, cameraMode.cameraName);
                        if (go != null)
                        {
                            camera = go.GetComponent<Camera>();
                        }
                    }
                    break;

                case CameraSelectType.InGame:
                    {
                        camera = GameCamera.instance.targetCamera;
                    }
                    break;

                default:
                    {
                        throw new System.NotSupportedException(string.Format("지원하지 않는 타입입니다. ({0})", cameraMode.type));
                    }
            }

            /// 카메라가 하나도 없다고?!
            /// 인게임 카메라라도 켜자
            if (camera == null && CameraUtil.GetMainCameraCount() == 0)
            {
                camera = GameCamera.instance.targetCamera;
            }

            if (camera != null)
            {
                CameraUtil.DeactiveMainCameras(camera);
                camera.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 활성화/비활성화 설정을 변경합니다.
        /// </summary>
        private void ChangeShows()
        {
            if (cameraMode != null)
            {
                Show(cameraMode.showTags, true);
                Show(cameraMode.hideTags, false);
            }
        }

        /// <summary>
        /// 태그를 기준을 대상 오브젝트들을 활성화/비활성화 처리합니다.
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="show"></param>
        private void Show(string[] tags, bool show)
        {
            if (tags == null || tags.Length == 0)
            {
                return;
            }

            var enums = EnumUtil<TargetTag>.Instance;
            for (int i = 0; i < tags.Length; i++)
            {
                TargetTag tag = TargetTag.Invalid;
                if (!enums.TryParse(tags[i], out tag))
                {
                    continue;
                }

                var targets = targetObjects.FindTargets(tag);
                if (targets == null || targets.Count == 0)
                {
                    continue;
                }

                for (int t = 0; t < targets.Count; t++)
                {
                    var go = targets[t];
                    if (go != null)
                    {
                        go.SetActive(show);
                    }
                }
            }
        }

        private void ChangeParents()
        {
            if (cameraMode == null)
            {
                return;
            }

            targetObjects.ChangeParent(TargetTag.Player, cameraMode.parentPlayer);
            targetObjects.ChangeParent(TargetTag.Enemy, cameraMode.parentEnemy);
            targetObjects.ChangeParent(TargetTag.NPC, cameraMode.parentNpc);

            if (cameraMode.type == CameraSelectType.InGame)
            {
                GameManager.instance.ResetPosition();
            }

#if UNITY_EDITOR
            Debug.Log("[SCENE] Change Parents: " + cameraMode.name);
#endif
        }

        /// <summary>
        /// tag 로 씬에 있는 targetobject 를 가져 온다.
        /// </summary>
        /// by yg.
        public List<GameObject> GetTargetObjects(TargetTag tag)
        {
            return targetObjects.FindTargets(tag);
        }

        public BackgroundData GetBackgroundData()
        {
            BackgroundData background = null;
            if (sceneData != null && !string.IsNullOrEmpty(sceneData.backgroundName))
            {
                background = sceneTable.GetBackground(sceneData.backgroundName);
            }
            return background;
        }

        public string GetBackgroundBGM()
        {
            sceneData.backgroundName = GameModeManager.instance.GetBackgroundName(sceneData);
            var background = sceneTable.GetBackground(sceneData.backgroundName);
            if (background != null)
            {
                return background.bgm;
            }
            return string.Empty;
        }

        /// <summary>
        /// 동기, 비동기 로딩을 섞어쓰면 게임이 멈추는 현상이 발생하는것 같아 하나씩 로딩을 처리합니다.
        /// </summary>
        /// <returns></returns>
        private System.Collections.IEnumerator WaitChangeScene()
        {
#if PROFILING
            Champion.Profiler.instance.Begin("WaitChangeScene");
#endif
            for (int i = 0; i < sceneHandlers.Count; i++)
            {
                var handler = sceneHandlers[i];
#if PROFILING
                Champion.Profiler.instance.Begin(string.Format("Changing {0}", handler.ToString()));
#endif
                handler.Change(sceneData);
#if PROFILING
                Champion.Profiler.instance.End(string.Format("Changing {0}", handler.ToString()));
#endif

                var loading = handler as ILoadable;
                if (loading != null)
                {
#if PROFILING
                    Champion.Profiler.instance.Begin(string.Format("Loading {0}", handler.ToString()));
#endif
                    while (!loading.isLoaded)
                    {
                        yield return null;
                    }
#if PROFILING
                    Champion.Profiler.instance.End(string.Format("Loading {0}", handler.ToString()));
#endif
                }
            }

            FinishChangeScene();
#if PROFILING
            Champion.Profiler.instance.End("WaitChangeScene");
#endif
        }

        /// <summary>
        /// 씬을 전환합니다.
        /// </summary>
        private void ChangeScene()
        {
#if PROFILING
            Profiler.instance.Begin(string.Format("Scene Change ({0})", sceneData.name));
#endif

            if (onChangeBegin != null)
            {
                onChangeBegin(sceneData);
            }

            StartCoroutine(WaitChangeScene());
        }

        private void FinishChangeScene()
        {
            ChangeCamera();
            ChangeShows();
            ChangeParents();
            ResetTodo();
            Restore();
            OnPostChange();

            if (onChanged != null)
            {
                onChanged(sceneData);
            }

#if PROFILING
            Profiler.instance.End(string.Format("Scene Change ({0})", sceneData.name));
#endif

			
			ScreenTouchEffectManager.DoSceneChanged();
        }

        #endregion 화면 전환 (내부 처리)

        /// <summary>
        /// 현재 활성화된 배경에 배치된 카메라를 찾습니다.
        /// </summary>
        /// <returns></returns>
        private Camera FindMainCameraFromActiveScene()
        {
            string tagName = EnumUtil<TargetTag>.Instance.GetEnumString(TargetTag.MainCamera);
            Scene activeScene;

#if KEEP_ACTIVE_SCENE
            activeScene = backgroundManager.current;
#else
            activeScene = SceneManager.GetActiveScene();
#endif
            if (activeScene.IsValid())
            {
                var roots = activeScene.GetRootGameObjects();
                for (int idxRoot = 0; idxRoot < roots.Length; idxRoot++)
                {
                    GameObject go = roots[idxRoot];
                    var cameras = go.GetComponentsInChildren<Camera>(true);
                    for (int cameraIdx = 0; cameraIdx < cameras.Length; cameraIdx++)
                    {
                        var camera = cameras[cameraIdx];
                        if (camera.gameObject.CompareTag(tagName))
                        {
                            return camera;
                        }
                    }
                }
            }

            return null;
        }
    }
}