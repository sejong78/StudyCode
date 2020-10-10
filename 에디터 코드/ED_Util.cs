using UnityEngine;
using UnityEditor;
using System;
using Microsoft.Win32;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class ED_Util : EditorWindow 
{
    public const int CONTROLS_DEFAULT_LABEL_WIDTH = 140;
    public const string FOLD_OUT_TOOL_TIP = "Click to Expand/Collapse";

    /// <summary>
    /// Used by AddFoldOutListItemButtons to return which button was pressed, and by 
    /// UpdateFoldOutListOnButtonPressed to process the pressed button for regular lists
    /// </summary>
    public enum EListButtons { None, Up, Down, Add, Remove }

    public static ScriptableObject CreateScriptableObejctFile( System.Type _type, string _fileName, string _path, HideFlags _hideFlags)
    {
        //SciprtableObject를 생성
        ScriptableObject sObject = ScriptableObject.CreateInstance(_type);
        AssetDatabase.CreateAsset(sObject, _path + "/" + _fileName + ".asset");
        sObject.hideFlags = (HideFlags)_hideFlags;

        return sObject;
    }
	
    public static void SaveScriptableObejctFile(string _fileName, string _path)
    {
        ScriptableObject obj = AssetDatabase.LoadAssetAtPath(_path + "/" + _fileName + ".asset", typeof(ScriptableObject)) as ScriptableObject;

        SaveScriptableObejctFile(obj);
    }

    public static void SaveScriptableObejctFile(UnityEngine.Object obj)
    {
        if (obj == null)
            return; 

        //디스크에 쓰기
        EditorUtility.SetDirty(obj);

        //EditorUtility.FocusProjectWindow();
        //Selection.activeObject = obj;
    }

    public static ScriptableObject LoadScriptableObject(string _pathName, string _prefabName)
    {
        return Resources.Load(_pathName + "/" + _prefabName) as ScriptableObject;
    }

    public static ScriptableObject LoadScriptableObjectOfProject(string _pathName, string _prefabName)
    {
        return AssetDatabase.LoadAssetAtPath(_pathName + "/" + _prefabName+".asset", typeof(ScriptableObject)) as ScriptableObject;
    }

    public static bool CreateTextFile(string _fileName, string _path, out System.IO.FileStream _result)
    {
        bool returnValue = false;

        FileInfo file = new FileInfo(_path + "/" + _fileName + ".txt");

        if (!file.Exists)
            returnValue = true;

        _result = file.Open(FileMode.OpenOrCreate);

        return returnValue;
    }

    //public static void PrefabSave(GameObject _src)
    //{
    //    // src데이터를 _dest 프리팹에 카피하고 저장해버린다.
    //    //GameObject test = PrefabUtility.GetPrefabParent(_src) as GameObject;
    //    //test = PrefabUtility.GetPrefabObject(_src) as GameObject;

    //    GameObject test = PrefabUtility.GetPrefabParent(Selection.objects[0]) as GameObject;
    //    test = PrefabUtility.GetPrefabObject(_src) as GameObject;

    //    PrefabUtility.ReplacePrefab(_src, PrefabUtility.GetPrefabParent(_src), ReplacePrefabOptions.ConnectToPrefab);
    //}

    public static void DrawText2Field(string _name, string _a, string _b, int nameSize = -1, int aSize = -1, int bSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            EditorGUILayout.TextField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            EditorGUILayout.TextField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_b);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void DrawText3Field(string _name, string _a, string _b, string _c,
        int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1, int dSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            EditorGUILayout.TextField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            EditorGUILayout.TextField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_b);

        GUI.contentColor = Color.green;
        if (cSize != -1)
            EditorGUILayout.TextField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_c);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void DrawFloat3Field(string _name, float _a, float _b, float _c,
        int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1, int dSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            EditorGUILayout.FloatField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.FloatField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            EditorGUILayout.FloatField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.FloatField(_b);

        GUI.contentColor = Color.green;
        if (cSize != -1)
            EditorGUILayout.FloatField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.FloatField(_c);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void DrawInt3Field(string _name, int _a, int _b, int _c, 
        int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1, int dSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            EditorGUILayout.IntField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            EditorGUILayout.IntField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_b);

        GUI.contentColor = Color.green;
        if (cSize != -1)
            EditorGUILayout.IntField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_c);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void InputFloat3Field(string _name, ref float _srcA, ref float _srcB, ref float _srcC, 
        float _a, float _b, float _c, int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            _srcA = EditorGUILayout.FloatField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            _srcA = EditorGUILayout.FloatField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            _srcB = EditorGUILayout.FloatField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            _srcB = EditorGUILayout.FloatField(_b);

        GUI.contentColor = Color.green;
        if (cSize != -1)
            _srcC = EditorGUILayout.FloatField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            _srcC = EditorGUILayout.FloatField(_c);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void InputInt3Field(string _name, ref int _srcA, ref int _srcB, ref int _srcC, 
        int _a, int _b, int _c, int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            _srcA = EditorGUILayout.IntField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            _srcA = EditorGUILayout.IntField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            _srcB = EditorGUILayout.IntField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            _srcB = EditorGUILayout.IntField(_b);

        GUI.contentColor = Color.green;
        if (cSize != -1)
            _srcC = EditorGUILayout.IntField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            _srcC = EditorGUILayout.IntField(_c);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void DrawInt4Field(string _name, int _a, int _b, int _c, int _d,
        int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1, int dSize = -1)
    {
        GUILayout.BeginHorizontal();
        if (nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if (aSize != -1)
            EditorGUILayout.IntField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_a);

        GUI.contentColor = Color.cyan;
        if (bSize != -1)
            EditorGUILayout.IntField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_b);

        GUI.contentColor = Color.green;
        if (cSize != -1)
            EditorGUILayout.IntField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_c);

        GUI.contentColor = Color.yellow;
        if (dSize != -1)
            EditorGUILayout.IntField(_d, GUILayout.Width(dSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.IntField(_d);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static void DrawText4Field(string _name, string _a, string _b, string _c, string _d,
        int nameSize = -1, int aSize = -1, int bSize = -1, int cSize = -1, int dSize = -1)
    {
        GUILayout.BeginHorizontal();
        if( nameSize != -1)
            GUILayout.Label(_name, GUILayout.Width(nameSize), GUILayout.ExpandWidth(true));
        else
            GUILayout.Label(_name);

        GUI.contentColor = Color.white;
        if( aSize != -1)
            EditorGUILayout.TextField(_a, GUILayout.Width(aSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_a);

        GUI.contentColor = Color.cyan;
        if( bSize != -1)
            EditorGUILayout.TextField(_b, GUILayout.Width(bSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_b);

        GUI.contentColor = Color.green;
        if( cSize != -1)
            EditorGUILayout.TextField(_c, GUILayout.Width(cSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_c);

        GUI.contentColor = Color.yellow;
        if( dSize != -1)
            EditorGUILayout.TextField(_d, GUILayout.Width(dSize), GUILayout.ExpandWidth(true));
        else
            EditorGUILayout.TextField(_d);

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }

    public static T[] GetAssetsOfType<T>(string fileExtension) where T : UnityEngine.Object
    {
        List<T> tempObjects = new List<T>();
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

        int i = 0; 
        int goFileInfoLength = goFileInfo.Length;
        FileInfo tempGoFileInfo; 
        string tempFilePath;
        T tempGO;
        for (; i < goFileInfoLength; i++)
        {
            tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
                continue;

            tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(T)) as T;
            if (tempGO == null)
            {
                continue;
            }
            else if (!(tempGO is T))
            {
                continue;
            }

            tempObjects.Add(tempGO);
        }

        return tempObjects.ToArray();
    }

    public static T[] GetAssetsOfType<T>(string fileExtension, string path) where T : UnityEngine.Object
    {
        List<T> tempObjects = new List<T>();

        DirectoryInfo directory = new DirectoryInfo(Application.dataPath + path);
        FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

        int i = 0; int goFileInfoLength = goFileInfo.Length;
        FileInfo tempGoFileInfo; string tempFilePath;
        T tempGO;
        for (; i < goFileInfoLength; i++)
        {
            tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
                continue;

            EditorUtility.DisplayProgressBar("로딩 중", tempGoFileInfo.Name + " (" + i + "/" + goFileInfoLength + ")", i / goFileInfoLength);

            tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(T)) as T;
            if (tempGO == null)
            {
                continue;
            }
            else if (!(tempGO is T))
            {
                continue;
            }

            tempObjects.Add(tempGO);
        }

        EditorUtility.ClearProgressBar();

        return tempObjects.ToArray();
    }

    private void DeletePlayerPrefContainKeys(string _key)
    {
        List<PlayerPrefStore> playerPrefs = new List<PlayerPrefStore>();

        if (Application.platform == RuntimePlatform.WindowsEditor)
            GetPrefKeysWindows(playerPrefs);
        else
            GetPrefKeysMac(playerPrefs);

        for( int i = 0; i < playerPrefs.Count; i++)
        {
            if (playerPrefs[i].name.Contains(_key) == false)
                continue;

            PlayerPrefs.DeleteKey(playerPrefs[i].name);
        }
    }

    private void GetPrefKeysWindows(List<PlayerPrefStore> playerPrefs)
    {
        // Unity stores prefs in the registry on Windows. 
        string regKey = @"Software\" + "Unity\\UnityEditor" + @"\" + PlayerSettings.companyName + @"\" + PlayerSettings.productName;
        RegistryKey key = Registry.CurrentUser.OpenSubKey(regKey);
        foreach (string subkeyName in key.GetValueNames())
        {
            string keyName = subkeyName.Substring(0, subkeyName.LastIndexOf("_"));
            string val = key.GetValue(subkeyName).ToString();
            // getting the type of the key is not supported in Mono with registry yet :(
            // Have to infer type and guess...
            int testInt = -1;
            string newType = "";
            bool couldBeInt = int.TryParse(val, out testInt);
            if (!float.IsNaN(PlayerPrefs.GetFloat(keyName, float.NaN)))
            {
                val = PlayerPrefs.GetFloat(keyName).ToString();
                newType = "real";
            }
            else if (couldBeInt && (PlayerPrefs.GetInt(keyName, testInt - 10) == testInt))
            {
                newType = "integer";
            }
            else {
                newType = "string";
            }
            PlayerPrefStore pref = new PlayerPrefStore(keyName, newType, val);
            playerPrefs.Add(pref);
        }
    }

    private void GetPrefKeysMac(List<PlayerPrefStore> playerPrefs)
    {
        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string pListPath = homePath + "/Library/Preferences/unity." + PlayerSettings.companyName + "." +
                           PlayerSettings.productName + ".plist";
        // Convert from binary plist to xml.
        Process p = new Process();
        ProcessStartInfo psi = new ProcessStartInfo("plutil", "-convert xml1 \"" + pListPath + "\"");
        p.StartInfo = psi;
        p.Start();
        p.WaitForExit();

        StreamReader sr = new StreamReader(pListPath);
        string pListData = sr.ReadToEnd();

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(pListData);

        XmlElement plist = xml["plist"];
        if (plist == null) return;
        XmlNode node = plist["dict"].FirstChild;
        while (node != null)
        {
            string name = node.InnerText;
            node = node.NextSibling;
            PlayerPrefStore pref = new PlayerPrefStore(name, node.Name, node.InnerText);
            node = node.NextSibling;
            playerPrefs.Add(pref);
        }

        //		// Convert plist back to binary
        Process.Start("plutil", " -convert binary1 \"" + pListPath + "\"");
    }

    public static void ChangeAniCurveKey(AnimationCurve _curve, int _keyIndex, float _value,
        AnimationUtility.TangentMode _tangentMode)
    {
        if (_curve.length == 0)
        {
            UnityEngine.Debug.LogError("_curve.length == 0");
            return;
        }

        Keyframe addKeyFrame = new Keyframe(_curve.keys[_keyIndex].time, _value);
        _curve.MoveKey(_keyIndex, addKeyFrame);

        AnimationUtility.SetKeyBroken(_curve, _keyIndex, true);
        AnimationUtility.SetKeyLeftTangentMode(_curve, _keyIndex, _tangentMode);
        AnimationUtility.SetKeyRightTangentMode(_curve, _keyIndex, _tangentMode);
    }

    #region PGEditorUtils
    /// <summary>
    /// Adds a fold-out list GUI from a generic list of any serialized object type
    /// </summary>
    /// <param name="list">A generic List</param>
    /// <param name="expanded">A bool to determine the state of the primary fold-out</param>
    public static bool FoldOutTextList(string label, List<string> list, bool expanded)
    {
        // Store the previous indent and return the flow to it at the end
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // A copy of toolbarButton with left alignment for foldouts
        var foldoutStyle = new GUIStyle(EditorStyles.toolbarButton);
        foldoutStyle.alignment = TextAnchor.MiddleLeft;

        expanded = AddFoldOutListHeader<string>(label, list, expanded, indent);

        if (expanded == false)
            return expanded;
        // START. Will consist of one row with two columns. 
        //        The second column has the content
        EditorGUILayout.BeginHorizontal();

        // SPACER COLUMN / INDENT
        //EditorGUILayout.BeginVertical();
        //EditorGUILayout.BeginHorizontal(GUILayout.MinWidth((indent + 3) * 9));
        //GUILayout.FlexibleSpace();
        //EditorGUILayout.EndHorizontal();
        //EditorGUILayout.EndVertical();

        // CONTENT COLUMN...
        EditorGUILayout.BeginVertical();

        // Use a for, instead of foreach, to avoid the iterator since we will be
        //   be changing the loop in place when buttons are pressed. Even legal
        //   changes can throw an error when changes are detected
        for (int i = 0; i < list.Count; i++)
        {
            string item = list[i];

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            // FIELD...
            if (item == null) item = "";
            list[i] = EditorGUILayout.TextField(item);

            EListButtons listButtonPressed = AddFoldOutListItemButtons();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2);
            UpdateFoldOutListOnButtonPressed<string>(list, i, listButtonPressed);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel = indent;

        return expanded;
    }

    /// <summary>
    /// Adds a fold-out list GUI from a generic list of any serialized object type
    /// </summary>
    /// <param name="list">A generic List</param>
    /// <param name="expanded">A bool to determine the state of the primary fold-out</param>
    public static bool FoldOutObjList<T>(string label, List<T> list, bool expanded) where T : UnityEngine.Object
    {
        // Store the previous indent and return the flow to it at the end
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;  // Space will handle this for the header

        // A copy of toolbarButton with left alignment for foldouts
        var foldoutStyle = new GUIStyle(EditorStyles.toolbarButton);
        foldoutStyle.alignment = TextAnchor.MiddleLeft;

        if (!AddFoldOutListHeader<T>(label, list, expanded, indent))
            return false;


        // Use a for, instead of foreach, to avoid the iterator since we will be
        //   be changing the loop in place when buttons are pressed. Even legal
        //   changes can throw an error when changes are detected
        for (int i = 0; i < list.Count; i++)
        {
            T item = list[i];

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space((indent + 3) * 6); // Matches the content indent

            // OBJECT FIELD...
            // Count is always in sync bec
            T fieldVal = (T)EditorGUILayout.ObjectField(item, typeof(T), true);


            // This is weird but have to replace the item with the new value, can't
            //   find a way to set in-place in a more stable way
            list.RemoveAt(i);
            list.Insert(i, fieldVal);

            EListButtons listButtonPressed = AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2);


            #region Process List Changes

            // Don't allow 'up' presses for the first list item
            switch (listButtonPressed)
            {
                case EListButtons.None: // Nothing was pressed, do nothing
                    break;

                case EListButtons.Up:
                    if (i > 0)
                    {
                        T shiftItem = list[i];
                        list.RemoveAt(i);
                        list.Insert(i - 1, shiftItem);
                    }
                    break;

                case EListButtons.Down:
                    // Don't allow 'down' presses for the last list item
                    if (i + 1 < list.Count)
                    {
                        T shiftItem = list[i];
                        list.RemoveAt(i);
                        list.Insert(i + 1, shiftItem);
                    }
                    break;

                case EListButtons.Remove:
                    list.RemoveAt(i);
                    break;

                case EListButtons.Add:
                    list.Insert(i, null);
                    break;
            }
            #endregion Process List Changes

        }

        EditorGUI.indentLevel = indent;

        return true;
    }

    public static bool FoldOutObjList2(string label, List<DCUseChecker.Attribute> list, bool expanded)
    {
        // Store the previous indent and return the flow to it at the end
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;  // Space will handle this for the header

        // A copy of toolbarButton with left alignment for foldouts
        var foldoutStyle = new GUIStyle(EditorStyles.toolbarButton);
        foldoutStyle.alignment = TextAnchor.MiddleLeft;

        if (!AddFoldOutListHeader<DCUseChecker.Attribute>(label + " / Count : " + list.Count, list, expanded, indent))
            return false;


        // Use a for, instead of foreach, to avoid the iterator since we will be
        //   be changing the loop in place when buttons are pressed. Even legal
        //   changes can throw an error when changes are detected
        for (int i = 0; i < list.Count; i++)
        {
            DCUseChecker.Attribute item = list[i];

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space((indent + 3) * 6); // Matches the content indent

            // OBJECT FIELD...
            // Count is always in sync bec
            list[i].m_Object = EditorGUILayout.ObjectField(list[i].m_Object, typeof(GameObject), true) as GameObject;

            // This is weird but have to replace the item with the new value, can't
            //   find a way to set in-place in a more stable way
            list.RemoveAt(i);
            list.Insert(i, item);

            EListButtons listButtonPressed = AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2);


            #region Process List Changes

            // Don't allow 'up' presses for the first list item
            switch (listButtonPressed)
            {
                case EListButtons.None: // Nothing was pressed, do nothing
                    break;

                case EListButtons.Up:
                    if (i > 0)
                    {
                        DCUseChecker.Attribute shiftItem = list[i];
                        list.RemoveAt(i);
                        list.Insert(i - 1, shiftItem);
                    }
                    break;

                case EListButtons.Down:
                    // Don't allow 'down' presses for the last list item
                    if (i + 1 < list.Count)
                    {
                        DCUseChecker.Attribute shiftItem = list[i];
                        list.RemoveAt(i);
                        list.Insert(i + 1, shiftItem);
                    }
                    break;

                case EListButtons.Remove:
                    list.RemoveAt(i);
                    break;

                case EListButtons.Add:
                    list.Insert(i, null);
                    break;
            }
            #endregion Process List Changes

        }

        EditorGUI.indentLevel = indent;

        return true;
    }

    /// <summary>
    /// Adds the buttons which control a list item
    /// </summary>
    /// <returns>LIST_BUTTONS - The LIST_BUTTONS pressed or LIST_BUTTONS.None</returns>
    public static EListButtons AddFoldOutListItemButtons()
    {
        #region Layout
        int buttonSpacer = 6;

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        // The up arrow will move things towards the beginning of the List
        var upArrow = '\u25B2'.ToString();
        bool upPressed = GUILayout.Button(new GUIContent(upArrow, "Click to shift up"),
                                          EditorStyles.toolbarButton);

        // The down arrow will move things towards the end of the List
        var dnArrow = '\u25BC'.ToString();
        bool downPressed = GUILayout.Button(new GUIContent(dnArrow, "Click to shift down"),
                                            EditorStyles.toolbarButton);

        // A little space between button groups
        GUILayout.Space(buttonSpacer);

        // Remove Button - Process presses later
        bool removePressed = GUILayout.Button(new GUIContent("-", "Click to remove"),
                                              EditorStyles.toolbarButton);

        // Add button - Process presses later
        bool addPressed = GUILayout.Button(new GUIContent("+", "Click to insert new"),
                                           EditorStyles.toolbarButton);

        EditorGUILayout.EndHorizontal();
        #endregion Layout

        // Return the pressed button if any
        if (upPressed == true) return EListButtons.Up;
        if (downPressed == true) return EListButtons.Down;
        if (removePressed == true) return EListButtons.Remove;
        if (addPressed == true) return EListButtons.Add;

        return EListButtons.None;
    }

    /// <summary>
    /// Used by basic foldout lists to process any list item button presses which will alter
    /// the order or members of the ist
    /// </summary>
    /// <param name="listButtonPressed"></param>
    public static void UpdateFoldOutListOnButtonPressed<T>(List<T> list, int currentIndex, EListButtons listButtonPressed)
    {
        // Don't allow 'up' presses for the first list item
        switch (listButtonPressed)
        {
            case EListButtons.None: // Nothing was pressed, do nothing
                break;

            case EListButtons.Up:
                if (currentIndex > 0)
                {
                    T shiftItem = list[currentIndex];
                    list.RemoveAt(currentIndex);
                    list.Insert(currentIndex - 1, shiftItem);
                }
                break;

            case EListButtons.Down:
                // Don't allow 'down' presses for the last list item
                if (currentIndex + 1 < list.Count)
                {
                    T shiftItem = list[currentIndex];
                    list.RemoveAt(currentIndex);
                    list.Insert(currentIndex + 1, shiftItem);
                }
                break;

            case EListButtons.Remove:
                list.RemoveAt(currentIndex);
                break;

            case EListButtons.Add:
                list.Insert(currentIndex, default(T));
                break;
        }
    }

    /// <summary>
    /// Adds a foldout in 'LookLikeInspector' which has a full bar to click on, not just
    /// the little triangle. It also adds a default tool-tip.
    /// </summary>
    /// <param name="expanded"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public static bool Foldout(bool expanded, string label)
    {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
		EditorGUIUtility.LookLikeInspector();
#endif
        var content = new GUIContent(label, FOLD_OUT_TOOL_TIP);
        expanded = EditorGUILayout.Foldout(expanded, content);
        SetLabelWidth();

        return expanded;
    }

    public static void SetLabelWidth()
    {
        EditorGUIUtility.labelWidth = CONTROLS_DEFAULT_LABEL_WIDTH;
    }

    public static bool DrawAttributeSelectionHeader<T>(out bool _resultState, List<T> _dataList, int _index, 
        Dictionary<object, bool> _foldingList, string _headName)
    {
        #region Section Header
        if (!_foldingList.TryGetValue(_dataList[_index], out _resultState))
        {
            _foldingList[_dataList[_index]] = false;
            _resultState = false;
        }

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        _resultState = EditorGUILayout.Foldout(_resultState, new GUIContent(_headName));
        _foldingList[_dataList[_index]] = _resultState;

        if (_resultState == false)
        {
            EditorGUILayout.EndHorizontal();
            return false;
        }

        EListButtons listButtonPressed = AddFoldOutListItemButtons();

        EditorGUILayout.EndHorizontal();

        #region Process List Changes
        // Don't allow 'up' presses for the first list item
        UpdateFoldOutListOnButtonPressed(_dataList, _index, listButtonPressed);
        if (listButtonPressed != EListButtons.None)
            return false;
        #endregion Process List Changes
        #endregion Section Header

        return true;
    }

    public static void DrawMasterSelectionHeader<T>(ref bool _resultFoldingState, ref bool _resultCollapseState,
        ref bool _resultExpandState, List<T> _dataList, string _name)
    {
        //_resultFoldingState = false;
        //_resultCollapseState = false;
        //_resultExpandState = false;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        _resultFoldingState = EditorGUILayout.Foldout(_resultFoldingState, new GUIContent(_name));

        if (_resultFoldingState == false)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }

        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        if (_dataList.Count > 0)
        {
            //리스트 축소
            GUIContent content;
            var collapseIcon = '\u2261'.ToString();
            content = new GUIContent(collapseIcon, "Click to collapse all");
            _resultCollapseState = GUILayout.Button(content, EditorStyles.toolbarButton);

            //리스트 확장
            var expandIcon = '\u25A1'.ToString();
            content = new GUIContent(expandIcon, "Click to expand all");
            _resultExpandState = GUILayout.Button(content, EditorStyles.toolbarButton);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _dataList.Add(default(T));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
    }

    public static bool DrawMasterSelectionHeader<T>(ref bool _resultFoldingState, List<T> _dataList, string _name)
    {
        bool result = true;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        _resultFoldingState = EditorGUILayout.Foldout(_resultFoldingState, new GUIContent(_name));

        if (_resultFoldingState == false)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return true;
        }

        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
        {
            _dataList.Add(default(T));
            result = false;
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        return result;
    }

    public static void DrawMasterSelectionHeader<T>(Dictionary<object, bool> _resultFoldingStateList, Dictionary<object, bool> _resultCollapseStateList,
        Dictionary<object, bool> _resultExpandStateList, int _index, List<T> _dataList, string _name)
    {
        bool resultFoldingState = false;
        bool resultCollapseState = false;
        bool resultExpandState = false;

        if (!_resultFoldingStateList.TryGetValue(_dataList[_index], out resultFoldingState))
        {
            _resultFoldingStateList[_dataList[_index]] = false;
            resultFoldingState = false;
        }

        if (!_resultCollapseStateList.TryGetValue(_dataList[_index], out resultCollapseState))
        {
            _resultCollapseStateList[_dataList[_index]] = false;
            resultCollapseState = false;
        }

        if (!_resultExpandStateList.TryGetValue(_dataList[_index], out resultExpandState))
        {
            _resultExpandStateList[_dataList[_index]] = false;
            resultExpandState = false;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        resultFoldingState = EditorGUILayout.Foldout(resultFoldingState, new GUIContent(_name));
        _resultFoldingStateList[_dataList[_index]] = resultFoldingState;

        if (resultFoldingState == false)
        {
            EditorGUILayout.EndHorizontal();
            return;
        }

        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        if (_dataList.Count > 0)
        {
            //리스트 축소
            GUIContent content;
            var collapseIcon = '\u2261'.ToString();
            content = new GUIContent(collapseIcon, "Click to collapse all");
            _resultFoldingStateList[_dataList[_index]] = GUILayout.Button(content, EditorStyles.toolbarButton);

            //리스트 확장
            var expandIcon = '\u25A1'.ToString();
            content = new GUIContent(expandIcon, "Click to expand all");
            _resultExpandStateList[_dataList[_index]] = GUILayout.Button(content, EditorStyles.toolbarButton);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _dataList.Add(default(T));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
    }

    public static void DrawMasterSelectionHeader<T>(Dictionary<object, bool> _resultFoldingStateList, int _index, List<T> _dataList, string _name)
    {
        bool resultFoldingState = false;

        if (!_resultFoldingStateList.TryGetValue(_dataList[_index], out resultFoldingState))
        {
            _resultFoldingStateList[_dataList[_index]] = false;
            resultFoldingState = false;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        resultFoldingState = EditorGUILayout.Foldout(resultFoldingState, new GUIContent(_name));
        _resultFoldingStateList[_dataList[_index]] = resultFoldingState;

        if (resultFoldingState == false)
        {
            EditorGUILayout.EndHorizontal();
            return;
        }

        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        
        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _dataList.Add(default(T));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Adds a fold-out list GUI from a generic list of any serialized object type.
    /// Uses System.Reflection to add all fields for a passed serialized object
    /// instance. Handles most basic types including automatic naming like the 
    /// inspector does by default
    /// 
    /// Adds collapseBools (see docs below)
    /// </summary>
    /// <param name="label"> The field label</param>
    /// <param name="list">A generic List</param>
    /// <param name="expanded">A bool to determine the state of the primary fold-out</param>
    /// <param name="foldOutStates">Dictionary<object, bool> used to track list item states</param>
    /// <param name="collapseBools">
    /// If true, bools on list items will collapse fields which follow them
    /// </param>
    /// <returns>The new foldout state from user input. Just like Unity's foldout</returns>
    public static bool SerializedObjFoldOutList<T>(string label,
                                                   List<T> list,
                                                   bool expanded,
                                                   ref Dictionary<object, bool> foldOutStates,
                                                   bool collapseBools)
                                          where T : new()
    {
        // Store the previous indent and return the flow to it at the end
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        int buttonSpacer = 6;

        #region Header Foldout
        // Use a Horizanal space or the toolbar will extend to the left no matter what
        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel = 0;  // Space will handle this for the header
        GUILayout.Space(indent * 6); // Matches the content indent

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        expanded = Foldout(expanded, label);
        if (!expanded)
        {
            // Don't add the '+' button when the contents are collapsed. Just quit.
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel = indent;  // Return to the last indent
            return expanded;
        }

        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));

        // Add expand/collapse buttons if there are items in the list
        bool masterCollapse = false;
        bool masterExpand = false;
        if (list.Count > 0)
        {
            GUIContent content;
            var collapseIcon = '\u2261'.ToString();
            content = new GUIContent(collapseIcon, "Click to collapse all");
            masterCollapse = GUILayout.Button(content, EditorStyles.toolbarButton);

            var expandIcon = '\u25A1'.ToString();
            content = new GUIContent(expandIcon, "Click to expand all");
            masterExpand = GUILayout.Button(content, EditorStyles.toolbarButton);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(buttonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            list.Add(new T());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        #endregion Header Foldout


        #region List Items
        // Use a for, instead of foreach, to avoid the iterator since we will be
        //   be changing the loop in place when buttons are pressed. Even legal
        //   changes can throw an error when changes are detected
        for (int i = 0; i < list.Count; i++)
        {
            T item = list[i];

            #region Section Header
            // If there is a field with the name 'name' use it for our label
            string itemLabel = GetSerializedObjFieldName<T>(item);
            if (itemLabel == "") itemLabel = string.Format("Element {0}", i + 1);


            // Get the foldout state. 
            //   If this item is new, add it too (singleton)
            //   Singleton works better than multiple Add() calls because we can do 
            //   it all at once, and in one place.
            bool foldOutState;
            if (!foldOutStates.TryGetValue(item, out foldOutState))
            {
                foldOutStates[item] = true;
                foldOutState = true;
            }

            // Force states if master buttons were pressed
            if (masterCollapse) foldOutState = false;
            if (masterExpand) foldOutState = true;

            // Use a Horizanal space or the toolbar will extend to the start no matter what
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel = 0;  // Space will handle this for the header
            GUILayout.Space((indent + 3) * 6); // Matches the content indent

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            // Display foldout with current state
            foldOutState = Foldout(foldOutState, itemLabel);
            foldOutStates[item] = foldOutState;  // Used again below

            EListButtons listButtonPressed = AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();

            #endregion Section Header


            // If folded out, display all serialized fields
            if (foldOutState == true)
            {
                EditorGUI.indentLevel = indent + 3;

                // Display Fields for the list instance
                SerializedObjectFields<T>(item, collapseBools);
                GUILayout.Space(2);
            }



            #region Process List Changes
            // Don't allow 'up' presses for the first list item
            switch (listButtonPressed)
            {
                case EListButtons.None: // Nothing was pressed, do nothing
                    break;

                case EListButtons.Up:
                    if (i > 0)
                    {
                        T shiftItem = list[i];
                        list.RemoveAt(i);
                        list.Insert(i - 1, shiftItem);
                    }
                    break;

                case EListButtons.Down:
                    // Don't allow 'down' presses for the last list item
                    if (i + 1 < list.Count)
                    {
                        T shiftItem = list[i];
                        list.RemoveAt(i);
                        list.Insert(i + 1, shiftItem);
                    }
                    break;

                case EListButtons.Remove:
                    list.RemoveAt(i);
                    foldOutStates.Remove(item);  // Clean-up
                    break;

                case EListButtons.Add:
                    list.Insert(i, new T());
                    break;
            }
            #endregion Process List Changes

        }
        #endregion List Items


        EditorGUI.indentLevel = indent;

        return expanded;
    }

    /// <summary>
    /// Uses System.Reflection to add all fields for a passed serialized object
    /// instance. Handles most basic types including automatic naming like the 
    /// inspector does by default
    /// 
    /// Optionally, this will make a bool switch collapse the following members if they 
    /// share the first 4 characters in their name or are not a bool (will collapse from
    /// bool until it finds another bool that doesn't share the first 4 characters)
    /// </summary>
    /// <param name="instance">
    /// An instance of the given type. Must be System.Serializable.
    /// </param>
    public static void SerializedObjectFields<T>(T instance)
    {
        SerializedObjectFields<T>(instance, false);
    }

    public static void SerializedObjectFields<T>(T instance, bool collapseBools)
    {
        // get all public properties of T to see if there is one called 'name'
        System.Reflection.FieldInfo[] fields = typeof(T).GetFields();

        bool boolCollapseState = false;  // False until bool is found
        string boolCollapseName = "";    // The name of the last bool member
        string currentMemberName = "";   // The name of the member being processed

        // Display Fields Dynamically
        foreach (System.Reflection.FieldInfo fieldInfo in fields)
        {
            if (!collapseBools)
            {
                FieldInfoField<T>(instance, fieldInfo);
                continue;
            }

            // USING collapseBools...
            currentMemberName = fieldInfo.Name;

            // If this is a bool. Add the field and set collapse to true until  
            //   the end or until another bool is hit
            if (fieldInfo.FieldType == typeof(bool))
            {
                // If the first 4 letters of this bool match the last one, include this
                //   in the collapse group, rather than starting a new one.
                if (boolCollapseName.Length > 4 &&
                    currentMemberName.StartsWith(boolCollapseName.Substring(0, 4)))
                {
                    if (!boolCollapseState) FieldInfoField<T>(instance, fieldInfo);
                    continue;
                }

                FieldInfoField<T>(instance, fieldInfo);


                boolCollapseName = currentMemberName;
                boolCollapseState = !(bool)fieldInfo.GetValue(instance);
            }
            else
            {
                // Add the field unless collapse is true
                if (!boolCollapseState) FieldInfoField<T>(instance, fieldInfo);
            }

        }
    }

    /// <summary>
    /// Uses a System.Reflection.FieldInfo to add a field
    /// Handles most built-in types and components
    /// includes automatic naming like the inspector does 
    /// by default
    /// </summary>
    /// <param name="fieldInfo"></param>
    public static void FieldInfoField<T>(T instance, System.Reflection.FieldInfo fieldInfo)
    {
        string label = fieldInfo.Name.DeCamel();

        #region Built-in Data Types
        if (fieldInfo.FieldType == typeof(string))
        {
            var val = (string)fieldInfo.GetValue(instance);
            val = EditorGUILayout.TextField(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(int))
        {
            var val = (int)fieldInfo.GetValue(instance);
            val = EditorGUILayout.IntField(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(float))
        {
            var val = (float)fieldInfo.GetValue(instance);
            val = EditorGUILayout.FloatField(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(bool))
        {
            var val = (bool)fieldInfo.GetValue(instance);
            val = EditorGUILayout.Toggle(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        #endregion Built-in Data Types

        #region Basic Unity Types
        else if (fieldInfo.FieldType == typeof(GameObject))
        {
            var val = (GameObject)fieldInfo.GetValue(instance);
            val = ObjectField<GameObject>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(Transform))
        {
            var val = (Transform)fieldInfo.GetValue(instance);
            val = ObjectField<Transform>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(Rigidbody))
        {
            var val = (Rigidbody)fieldInfo.GetValue(instance);
            val = ObjectField<Rigidbody>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(Renderer))
        {
            var val = (Renderer)fieldInfo.GetValue(instance);
            val = ObjectField<Renderer>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(Mesh))
        {
            var val = (Mesh)fieldInfo.GetValue(instance);
            val = ObjectField<Mesh>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(Vector3))
        {
            var val = (Vector3)fieldInfo.GetValue(instance);
            val = EditorGUILayout.Vector3Field(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        #endregion Basic Unity Types

        #region Unity Collider Types
        else if (fieldInfo.FieldType == typeof(BoxCollider))
        {
            var val = (BoxCollider)fieldInfo.GetValue(instance);
            val = ObjectField<BoxCollider>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(SphereCollider))
        {
            var val = (SphereCollider)fieldInfo.GetValue(instance);
            val = ObjectField<SphereCollider>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(CapsuleCollider))
        {
            var val = (CapsuleCollider)fieldInfo.GetValue(instance);
            val = ObjectField<CapsuleCollider>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(MeshCollider))
        {
            var val = (MeshCollider)fieldInfo.GetValue(instance);
            val = ObjectField<MeshCollider>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        else if (fieldInfo.FieldType == typeof(WheelCollider))
        {
            var val = (WheelCollider)fieldInfo.GetValue(instance);
            val = ObjectField<WheelCollider>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        #endregion Unity Collider Types

        #region Other Unity Types
        else if (fieldInfo.FieldType == typeof(CharacterController))
        {
            var val = (CharacterController)fieldInfo.GetValue(instance);
            val = ObjectField<CharacterController>(label, val);
            fieldInfo.SetValue(instance, val);
            return;
        }
        #endregion Other Unity Types
    }

    /// <summary>
    /// Searches a serialized object for a field matching "name" (not case-sensitve),
    /// and if found, returns the value
    /// </summary>
    /// <param name="instance">
    /// An instance of the given type. Must be System.Serializable.
    /// </param>
    /// <returns>The name field's value or ""</returns>
    public static string GetSerializedObjFieldName<T>(T instance)
    {
        // get all public properties of T to see if there is one called 'name'
        System.Reflection.FieldInfo[] fields = typeof(T).GetFields();

        // If there is a field with the name 'name' return its value
        foreach (System.Reflection.FieldInfo fieldInfo in fields)
            if (fieldInfo.Name.ToLower() == "name")
                return ((string)fieldInfo.GetValue(instance)).DeCamel();

        // If a field type is a UnityEngine object, return its name
        //   This is done in a second loop because the first is fast as is
        foreach (System.Reflection.FieldInfo fieldInfo in fields)
        {
            try
            {
                var val = (UnityEngine.Object)fieldInfo.GetValue(instance);
                return val.name.DeCamel();
            }
            catch { }
        }

        return "";
    }

    /// <summary>
    /// A generic version of EditorGUILayout.ObjectField.
    /// Allows objects to be drag and dropped or picked.
    /// This version defaults to 'allowSceneObjects = true'.
    /// 
    /// Instead of this:
    ///     var script = (MyScript)target;
    ///     script.transform = (Transform)EditorGUILayout.ObjectField("My Transform", script.transform, typeof(Transform), true);        
    /// 
    /// Do this:    
    ///     var script = (MyScript)target;
    ///     script.transform = EditorGUILayout.ObjectField<Transform>("My Transform", script.transform);        
    /// </summary>
    /// <typeparam name="T">The type of object to use</typeparam>
    /// <param name="label">The label (text) to show to the left of the field</param>
    /// <param name="obj">The obj variable of the script this GUI field is for</param>
    /// <returns>A reference to what is in the field. An object or null.</returns>
    public static T ObjectField<T>(string label, T obj) where T : UnityEngine.Object
    {
        return ObjectField<T>(label, obj, true);
    }

    /// <summary>
    /// A generic version of EditorGUILayout.ObjectField.
    /// Allows objects to be drag and dropped or picked.
    /// </summary>
    /// <typeparam name="T">The type of object to use</typeparam>
    /// <param name="label">The label (text) to show to the left of the field</param>
    /// <param name="obj">The obj variable of the script this GUI field is for</param>
    /// <param name="allowSceneObjects">Allow scene objects. See Unity Docs for more.</param>
    /// <returns>A reference to what is in the field. An object or null.</returns>
    public static T ObjectField<T>(string label, T obj, bool allowSceneObjects)
            where T : UnityEngine.Object
    {
        return (T)EditorGUILayout.ObjectField(label, obj, typeof(T), allowSceneObjects);
    }

    /// <summary>
    /// Adds the GUI header line which contains the label and add buttons.
    /// </summary>
    /// <param name="label">The visible label in the GUI</param>
    /// <param name="list">Needed to add a new item if count is 0</param>
    /// <param name="expanded"></param>
    /// <param name="lastIndent"></param>
    private static bool AddFoldOutListHeader<T>(string label, List<T> list, bool expanded, int lastIndent)
    {
        int buttonSpacer = 6;

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        expanded = Foldout(expanded, label);
        if (!expanded)
        {
            // Don't add the '+' button when the contents are collapsed. Just quit.
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel = lastIndent;  // Return to the last indent
            return expanded;
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));   // 1/2 the item button width
        GUILayout.Space(buttonSpacer);

        // Master add at end button. List items will insert
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            list.Add(default(T));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        return expanded;
    }

    #endregion

    // (clone)를 제거하고 객체를 만들어서 반환한다.
    public static GameObject CreateGameObject(GameObject _obj, Transform _parent)
    {
        GameObject tempObject = GameObject.Instantiate(_obj, _parent) as GameObject;

        tempObject.name = tempObject.name.Replace("(Clone)", "");
        tempObject.name = tempObject.name.Replace(" (Clone)", "");

        return tempObject;
    }

    public static GameObject FindGameObject(string _index, bool includeChild = false)
    {
        // 하이얼라키의 전체를 디져서 동일한 이름의 오브젝트를 반환해준다.
        GameObject tempObject = null;
        GameObject[] findRoots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < findRoots.Length; i++)
        {
            tempObject = findRoots[i];

            if (tempObject.name == _index)
                return tempObject;

            if (includeChild == false)
                continue;

            tempObject = DCUtil.DCUnity.GetChild(findRoots[i], _index);

            if (tempObject == null)
                continue;

            return tempObject;
        }

        return null;
    }

    #region >> Excel 관련 <<
    public static void SetExcelCellValue(ISheet _sheet, int _rowIndex, int _cellIndex, object _value, CellType _cellType)
    {
        IRow row = _sheet.GetRow(_rowIndex);

        if( row == null )
            row = _sheet.CreateRow(_rowIndex);

        ICell cell = row.GetCell(_cellIndex);

        if (cell == null)
            cell = row.CreateCell(_cellIndex);

        cell.SetCellType(_cellType);
        switch (_cellType)
        {
            case CellType.Boolean:
                cell.SetCellValue((bool)_value);
                break;
            case CellType.Numeric:
                cell.SetCellValue((double)_value);
                break;
            case CellType.String:
                cell.SetCellValue((string)_value);
                break;
        }
    }

    public static void GetExcelRowOfRowValue(ISheet _sheet, int _culIndex, object _key, out IRow _result)
    {
        _result = null;
        for (int i = 0; i <= _sheet.LastRowNum; i++)
        {
            IRow row = _sheet.GetRow(i);

            if( row == null )
                continue;

            ICell cell_0 = row.GetCell(_culIndex);

            switch (cell_0.CellType)
            {
                case CellType.Boolean:
                    if (cell_0.BooleanCellValue != (bool)_key)
                        break;

                    _result = row;
                    return;
                case CellType.Numeric:
                    if (cell_0.NumericCellValue != (double)_key)
                        break;

                    _result = row;
                    return;
                case CellType.String:
                    if (cell_0.StringCellValue != (string)_key)
                        break;

                    _result = row;
                    return;
            }
        }
    }

    public static IWorkbook CreateExcel(string _path, string _sheetName, List<string> _cellNameList, List<CellType> _cellTypeList)
    {
        IWorkbook wb = null;
        if (System.IO.File.Exists(_path) == false)
        {
            FileStream stream = new FileStream(_path, FileMode.Create, FileAccess.Write);
            wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet(_sheetName);

            IRow row = sheet.CreateRow(0);

            for (int i = 0; i < _cellNameList.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellType(_cellTypeList[i]);
                cell.SetCellValue(_cellNameList[i]);
            }

            wb.Write(stream);
            stream.Close();
            stream.Dispose();
        }
        else
        {
            wb = OpenExcel(_path);
        }

        return wb;
    }

    public static IWorkbook OpenExcel(string _path)
    {
        //엑셀 파일을 Open
        FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);

        if (stream == null)
        {
            UnityEngine.Debug.LogError("error : " + _path);
            return null;
        }

        //Open된 엑셀 파일 메모리에 생성
        IWorkbook book = new HSSFWorkbook(stream);

        stream.Close();
        stream.Dispose();
        stream = null;

        return book;
    }

    public static ISheet OpenSheet(string _path)
    {
        //엑셀 파일을 Open
        FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);

        if (stream == null)
        {
            UnityEngine.Debug.LogError("error : " + _path);
            return null;
        }

        //Open된 엑셀 파일 메모리에 생성
        IWorkbook book = new HSSFWorkbook(stream);

        stream.Close();
        stream.Dispose();
        stream = null;

        return book.GetSheetAt(0);
    }

    public static void SaveSheet(string _path, IWorkbook _workBook)
    {
        //엑셀 파일을 Open
        FileStream stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write);

        if (stream == null)
        {
            UnityEngine.Debug.LogError("error : " + _path);
            return;
        }

        _workBook.Write(stream);

        stream.Close();
        stream.Dispose();
        stream = null;
    }

    public static void RemoveSheetRowRange(ISheet _sheet, int _start, int _last)
    {
        while (_start <= _last)
        {
            var row = _sheet.GetRow(_start);
            _sheet.RemoveRow(row);

            _start++;
        }
    }
    #endregion
}
