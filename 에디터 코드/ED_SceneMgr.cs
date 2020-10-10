using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using GlobalEnum;
using System;

[CustomEditor(typeof(DCSceneMgr))]
public class ED_SceneMgr : Editor
{
    private DCSceneMgr m_Data;

    private int m_Blank = 10;
    private int m_ButtonSpacer = 6;

    #region SceneData
    private bool m_SceneDataListfoldOutState;
    private bool m_ActiveUIListfoldOutState;
    private bool m_ActiveUIListDatafoldOutState;
    private bool m_UICtrListfoldOutState;
    private bool m_UICtrListDatafoldOutState;
    private bool m_UIEffectListfoldOutState;
    private bool m_UIEffectListDatafoldOutState;
    // Add expand/collapse buttons if there are items in the list
    private bool m_SceneDataMasterCollapse = false;
    private bool m_SceneDataMasterExpand = false;
    #endregion

    #region SceneGroupData
    private bool m_SceneGroupDataListfoldOutState;
    private bool m_SceneGroupDataUIEffectListfoldOutState;
    private bool m_SceneGroupDataUIEffectListDatafoldOutState;
    // Add expand/collapse buttons if there are items in the list
    private bool m_SceneGroupDataMasterCollapse = false;
    private bool m_SceneGroupDataMasterExpand = false;
    #endregion

    //저장용 폴드아웃 상태변수
    #region SceneData
    [HideInInspector]
    public bool m_SceneDataListFolding = false;
    [HideInInspector]
    public bool m_ActiveUIListFolding = false;

    [HideInInspector]
    public Dictionary<object, bool> m_editorSceneDataListItemStates = new Dictionary<object, bool>();
    [HideInInspector]
    public Dictionary<object, bool> m_editorActiveUILListItemStates = new Dictionary<object, bool>();
    [HideInInspector]
    public Dictionary<object, bool> m_editorUICtrLListItemStates = new Dictionary<object, bool>();
    [HideInInspector]
    public Dictionary<object, bool> m_editorActiveUILListDataItemStates = new Dictionary<object, bool>();
    #endregion

    #region SceneGroupData
    [HideInInspector]
    public bool m_SceneGroupDataListFolding = true;
    //[HideInInspector]
    //public bool m_ActiveUIListFolding = false;

    [HideInInspector]
    public Dictionary<object, bool> m_editorSceneGroupDataListItemStates = new Dictionary<object, bool>();
    [HideInInspector]
    public Dictionary<object, bool> m_editorUIEffectDataLListItemStates = new Dictionary<object, bool>();
    //[HideInInspector]
    public Dictionary<object, bool> m_editorUIEffectDataListDataItemStates = new Dictionary<object, bool>();
    [HideInInspector]
    public Dictionary<object, bool> m_editorChildSceneDataListItemStates = new Dictionary<object, bool>();
    #endregion

    private string m_UIName = string.Empty;
    private string m_UICtrName = string.Empty;
    private string m_EffectName = string.Empty;

    private DCSceneData m_TempSceneData = null;
    private DCSceneGroupData m_TempSceneGroupData = null;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Data = (DCSceneMgr)target;

        DrawSceneData(m_Data.m_SceneData);

        DrawSceneGroupData();

        DrawAutoRemove();

        FindPrefab();

        if ( GUI.changed )
            EditorUtility.SetDirty( m_Data );

    }

    private bool HasSameEfx( List<DCUIEffecData> _list )
    {
        foreach( var _efx in _list )
        {
            if( true == m_EffectName.Equals( _efx.m_EffectName ) )
                return true;
        }
        return false;
    }

    private void DrawSceneData(List<DCSceneData> _data)
    {

        #region Header Foldout
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (m_editorChildSceneDataListItemStates.ContainsKey(_data) == false)
            m_editorChildSceneDataListItemStates[_data] = true;

        m_editorChildSceneDataListItemStates[_data] = EditorGUILayout.Foldout(m_editorChildSceneDataListItemStates[_data], "SceneData");

        if (m_editorChildSceneDataListItemStates[_data] == false)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }

        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        if (_data.Count > 0)
        {
            //리스트 축소
            GUIContent content;
            var collapseIcon = '\u2261'.ToString();
            content = new GUIContent(collapseIcon, "Click to collapse all");
            m_SceneDataMasterCollapse = GUILayout.Button(content, EditorStyles.toolbarButton);
            //리스트 확장
            var expandIcon = '\u25A1'.ToString();
            content = new GUIContent(expandIcon, "Click to expand all");
            m_SceneDataMasterExpand = GUILayout.Button(content, EditorStyles.toolbarButton);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(m_ButtonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _data.Add(new DCSceneData());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        #endregion

        #region List Items
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(m_Blank);
        EditorGUILayout.BeginVertical();

        for (int i = 0; i < _data.Count; i++)
        {
            #region Section Header

            if (!m_editorSceneDataListItemStates.TryGetValue(_data[i], out m_SceneDataListfoldOutState))
            {
                m_editorSceneDataListItemStates[_data[i]] = true;
                m_SceneDataListfoldOutState = false;
            }

            // Force states if master buttons were pressed
            if (m_SceneDataMasterCollapse) m_SceneDataListfoldOutState = false;
            if (m_SceneDataMasterExpand) m_SceneDataListfoldOutState = true;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_SceneDataListfoldOutState = EditorGUILayout.Foldout(m_SceneDataListfoldOutState, _data[i].m_KeyName);
            m_editorSceneDataListItemStates[_data[i]] = m_SceneDataListfoldOutState;

            if (m_SceneDataListfoldOutState == false)
            {
                EditorGUILayout.EndHorizontal();
                continue;
            }

            ED_Util.EListButtons listButtonPressed = ED_Util.AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();

            #region Process List Changes
            // Don't allow 'up' presses for the first list item
            ED_Util.UpdateFoldOutListOnButtonPressed(_data, i, listButtonPressed);
            if (listButtonPressed != ED_Util.EListButtons.None)
                return;
            #endregion Process List Changes

            #endregion

            #region ActiveUI List Items
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_Blank);
            GUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            _data[i].m_KeyName = EditorGUILayout.TextField("Key Name", _data[i].m_KeyName);

            if (GUILayout.Button(new GUIContent("해당 내용 추가하기", "Click to insert data"), EditorStyles.toolbarButton) == true)
            {
                m_TempSceneData = new DCSceneData();
                m_TempSceneData.Copy(_data[i]);
                _data.Insert(i, m_TempSceneData);

                m_TempSceneData = null;
            }

            EditorGUILayout.EndHorizontal();

            //_data[i].m_KeyName = EditorGUILayout.TextField("Key Name", _data[i].m_KeyName);

            #region ActiveUI List Header Foldout
            DrawActiveUIList(_data[i]);
            #endregion

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            #endregion

            #region UICtr List Header Foldout
            DrawUICtrlList(_data[i]);
            #endregion

            #region UIEffect List Header Foldout
            DrawUIEffectData(_data[i]);
            #endregion

        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

    }

    private void DrawActiveUIList(DCSceneData _data)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (!m_editorActiveUILListItemStates.TryGetValue(_data, out m_ActiveUIListfoldOutState))
        {
            m_editorActiveUILListItemStates[_data] = true;
            m_ActiveUIListfoldOutState = false;
        }

        // Force states if master buttons were pressed
        if (m_SceneDataMasterCollapse) m_ActiveUIListfoldOutState = false;
        if (m_SceneDataMasterExpand) m_ActiveUIListfoldOutState = true;

        m_ActiveUIListfoldOutState = EditorGUILayout.Foldout(m_ActiveUIListfoldOutState, "ActiveUIList");
        m_editorActiveUILListItemStates[_data] = m_ActiveUIListfoldOutState;

        if (m_ActiveUIListfoldOutState == false)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        if (_data.m_ActiveUIList.Count > 0)
        {
            //리스트 축소
            GUIContent content;
            var collapseIcon = '\u2261'.ToString();
            content = new GUIContent(collapseIcon, "Click to collapse all");
            m_SceneDataMasterCollapse = GUILayout.Button(content, EditorStyles.toolbarButton);
            //리스트 확장
            var expandIcon = '\u25A1'.ToString();
            content = new GUIContent(expandIcon, "Click to expand all");
            m_SceneDataMasterExpand = GUILayout.Button(content, EditorStyles.toolbarButton);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(m_ButtonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _data.m_ActiveUIList.Add(new DCAttachUIData());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();


        for (int j = 0; j < _data.m_ActiveUIList.Count; j++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_Blank);
            GUILayout.BeginVertical("box");
            if (!m_editorActiveUILListDataItemStates.TryGetValue(_data.m_ActiveUIList[j], out m_ActiveUIListDatafoldOutState))
            {
                m_editorActiveUILListDataItemStates[_data.m_ActiveUIList[j]] = true;
                m_ActiveUIListDatafoldOutState = false;
            }

            // Force states if master buttons were pressed
            if (m_SceneDataMasterCollapse) m_ActiveUIListDatafoldOutState = false;
            if (m_SceneDataMasterExpand) m_ActiveUIListDatafoldOutState = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_ActiveUIListDatafoldOutState = EditorGUILayout.Foldout(m_ActiveUIListDatafoldOutState, _data.m_ActiveUIList[j].m_Name);
            m_editorActiveUILListDataItemStates[_data.m_ActiveUIList[j]] = m_ActiveUIListDatafoldOutState;
            ED_Util.EListButtons listActiveUIButtonPressed = ED_Util.AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            if (m_ActiveUIListDatafoldOutState == true)
            {
                // Display Fields for the list instance
                ED_Util.SerializedObjectFields<DCAttachUIData>(_data.m_ActiveUIList[j]);
                GUILayout.Space(2);
            }

            #region Process ActiveUI List Changes
            // Don't allow 'up' presses for the first list item
            ED_Util.UpdateFoldOutListOnButtonPressed(_data.m_ActiveUIList, j, listActiveUIButtonPressed);
            #endregion Process tActiveUI List Changes

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawUICtrlList(DCSceneData _data)
    {
#region UICtr List Header Foldout
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(m_Blank);
        GUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (!m_editorUICtrLListItemStates.TryGetValue(_data, out m_UICtrListfoldOutState))
        {
            m_editorUICtrLListItemStates[_data] = true;
            m_UICtrListfoldOutState = false;
        }

        // Force states if master buttons were pressed
        if (m_SceneDataMasterCollapse) m_UICtrListfoldOutState = false;
        if (m_SceneDataMasterExpand) m_UICtrListfoldOutState = true;

        m_UICtrListfoldOutState = EditorGUILayout.Foldout(m_UICtrListfoldOutState, "UICtrList");
        m_editorUICtrLListItemStates[_data] = m_UICtrListfoldOutState;

        if (m_UICtrListfoldOutState == false)
        {
            EditorGUILayout.EndHorizontal();
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(m_ButtonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _data.m_UICtrData.Add(new DCUICtrData());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
#endregion

        for (int j = 0; j < _data.m_UICtrData.Count; j++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_Blank);
            GUILayout.BeginVertical("box");
            if (!m_editorActiveUILListDataItemStates.TryGetValue(_data.m_UICtrData[j], out m_UICtrListDatafoldOutState))
            {
                m_editorActiveUILListDataItemStates[_data.m_UICtrData[j]] = true;
                m_UICtrListDatafoldOutState = true;
            }

            // Force states if master buttons were pressed
            if (m_SceneDataMasterCollapse) m_UICtrListDatafoldOutState = false;
            if (m_SceneDataMasterExpand) m_UICtrListDatafoldOutState = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_UICtrListDatafoldOutState = EditorGUILayout.Foldout(m_UICtrListDatafoldOutState, _data.m_UICtrData[j].m_UICtrName);
            m_editorActiveUILListDataItemStates[_data.m_UICtrData[j]] = m_UICtrListDatafoldOutState;
            ED_Util.EListButtons listUICtrButtonPressed = ED_Util.AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            if (m_UICtrListDatafoldOutState == true)
            {
                // Display Fields for the list instance
                ED_Util.SerializedObjectFields<DCUICtrData>(_data.m_UICtrData[j]);
                GUILayout.Space(2);
            }

            #region Process UICtr List Changes
            // Don't allow 'up' presses for the first list item
            ED_Util.UpdateFoldOutListOnButtonPressed(_data.m_UICtrData, j, listUICtrButtonPressed);
            #endregion Process UICtr List Changes

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawUIEffectData(DCSceneData _data)
    {
        #region UICtr List Header Foldout
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(m_Blank);
        GUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (!m_editorUIEffectDataListDataItemStates.TryGetValue(_data, out m_UIEffectListfoldOutState))
        {
            m_editorUIEffectDataListDataItemStates[_data] = true;
            m_UIEffectListfoldOutState = false;
        }

        // Force states if master buttons were pressed
        if (m_SceneDataMasterCollapse) m_UIEffectListfoldOutState = false;
        if (m_SceneDataMasterExpand) m_UIEffectListfoldOutState = true;

        m_UIEffectListfoldOutState = EditorGUILayout.Foldout(m_UIEffectListfoldOutState, "UIEffectDataList");
        m_editorUIEffectDataListDataItemStates[_data] = m_UIEffectListfoldOutState;

        if (m_UIEffectListfoldOutState == false)
        {
            EditorGUILayout.EndHorizontal();
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(m_ButtonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _data.m_UIEffectData.Add(new DCUIEffecData());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        #endregion

        for (int j = 0; j < _data.m_UIEffectData.Count; j++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_Blank);
            GUILayout.BeginVertical("box");
            if (!m_editorActiveUILListDataItemStates.TryGetValue(_data.m_UIEffectData[j], out m_UIEffectListDatafoldOutState))
            {
                m_editorActiveUILListDataItemStates[_data.m_UIEffectData[j]] = true;
                m_UIEffectListDatafoldOutState = true;
            }

            // Force states if master buttons were pressed
            if (m_SceneDataMasterCollapse) m_UIEffectListDatafoldOutState = false;
            if (m_SceneDataMasterExpand) m_UIEffectListDatafoldOutState = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_UIEffectListDatafoldOutState = EditorGUILayout.Foldout(m_UIEffectListDatafoldOutState, _data.m_UIEffectData[j].m_EffectName);
            m_editorActiveUILListDataItemStates[_data.m_UIEffectData[j]] = m_UIEffectListDatafoldOutState;
            ED_Util.EListButtons listUICtrButtonPressed = ED_Util.AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            if (m_UIEffectListDatafoldOutState == true)
            {
                // Display Fields for the list instance
                ED_Util.SerializedObjectFields<DCUIEffecData>(_data.m_UIEffectData[j]);
                GUILayout.Space(2);
            }

            #region Process UICtr List Changes
            // Don't allow 'up' presses for the first list item
            ED_Util.UpdateFoldOutListOnButtonPressed(_data.m_UIEffectData, j, listUICtrButtonPressed);
            #endregion Process UICtr List Changes

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawUIEffectData(DCSceneGroupData _groupData, List<DCUIEffecData> _uiEffectData)
    {
        #region ActiveUI List Header Foldout
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (!m_editorUIEffectDataLListItemStates.TryGetValue(_groupData, out m_SceneGroupDataUIEffectListfoldOutState))
        {
            m_editorUIEffectDataLListItemStates[_groupData] = true;
            m_SceneGroupDataUIEffectListfoldOutState = true;
        }

        // Force states if master buttons were pressed
        if (m_SceneGroupDataMasterCollapse) m_SceneGroupDataUIEffectListfoldOutState = false;
        if (m_SceneGroupDataMasterExpand) m_SceneGroupDataUIEffectListfoldOutState = true;

        m_SceneGroupDataUIEffectListfoldOutState = EditorGUILayout.Foldout(m_SceneGroupDataUIEffectListfoldOutState, "UI Effect Data");
        m_editorUIEffectDataLListItemStates[_groupData] = m_SceneGroupDataUIEffectListfoldOutState;

        if (m_SceneGroupDataUIEffectListfoldOutState == false)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(m_ButtonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            _uiEffectData.Add(new DCUIEffecData());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        #endregion

        for (int j = 0; j < _uiEffectData.Count; j++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_Blank);
            GUILayout.BeginVertical("box");
            if (!m_editorUIEffectDataListDataItemStates.TryGetValue(_uiEffectData[j], out m_SceneGroupDataUIEffectListDatafoldOutState))
            {
                m_editorUIEffectDataListDataItemStates[_uiEffectData[j]] = true;
                m_SceneGroupDataUIEffectListDatafoldOutState = true;
            }

            // Force states if master buttons were pressed
            if (m_SceneDataMasterCollapse) m_SceneGroupDataUIEffectListDatafoldOutState = false;
            if (m_SceneDataMasterExpand) m_SceneGroupDataUIEffectListDatafoldOutState = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_SceneGroupDataUIEffectListDatafoldOutState = EditorGUILayout.Foldout(m_SceneGroupDataUIEffectListDatafoldOutState, _uiEffectData[j].m_EffectName);
            m_editorUIEffectDataListDataItemStates[_uiEffectData[j]] = m_SceneGroupDataUIEffectListDatafoldOutState;
            ED_Util.EListButtons listActiveUIButtonPressed = ED_Util.AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            if (m_SceneGroupDataUIEffectListDatafoldOutState == true)
            {
                // Display Fields for the list instance
                ED_Util.SerializedObjectFields<DCUIEffecData>(_uiEffectData[j]);
                GUILayout.Space(2);
            }

            #region Process ActiveUI List Changes
            // Don't allow 'up' presses for the first list item
            ED_Util.UpdateFoldOutListOnButtonPressed(_uiEffectData, j, listActiveUIButtonPressed);
            #endregion Process tActiveUI List Changes

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawSceneGroupData()
    {

        #region Header Foldout
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        m_SceneGroupDataListFolding = EditorGUILayout.Foldout(m_SceneGroupDataListFolding, "SceneGroupData");

        if (m_SceneGroupDataListFolding == false)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            return;
        }
        // BUTTONS...
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));
        if (m_Data.m_SceneData.Count > 0)
        {
            //리스트 축소
            GUIContent content;
            var collapseIcon = '\u2261'.ToString();
            content = new GUIContent(collapseIcon, "Click to collapse all");
            m_SceneGroupDataMasterCollapse = GUILayout.Button(content, EditorStyles.toolbarButton);
            //리스트 확장
            var expandIcon = '\u25A1'.ToString();
            content = new GUIContent(expandIcon, "Click to expand all");
            m_SceneGroupDataMasterExpand = GUILayout.Button(content, EditorStyles.toolbarButton);
        }
        else
        {
            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
        // A little space between button groups
        GUILayout.Space(m_ButtonSpacer);

        // Main Add button
        if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
            m_Data.m_SceneGroupData.Add(new DCSceneGroupData());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        #endregion

        #region List Items
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(m_Blank);
        EditorGUILayout.BeginVertical();

        for (int i = 0; i < m_Data.m_SceneGroupData.Count; i++)
        {
            #region Section Header

            if (!m_editorSceneGroupDataListItemStates.TryGetValue(m_Data.m_SceneGroupData[i], out m_SceneGroupDataListfoldOutState))
            {
                m_editorSceneGroupDataListItemStates[m_Data.m_SceneGroupData[i]] = true;
                m_SceneGroupDataListfoldOutState = false;
            }

            // Force states if master buttons were pressed
            if (m_SceneGroupDataMasterCollapse) m_SceneGroupDataListfoldOutState = false;
            if (m_SceneGroupDataMasterExpand) m_SceneGroupDataListfoldOutState = true;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_SceneGroupDataListfoldOutState = EditorGUILayout.Foldout(m_SceneGroupDataListfoldOutState, m_Data.m_SceneGroupData[i].m_ESceneGroupID.ToString());
            m_editorSceneGroupDataListItemStates[m_Data.m_SceneGroupData[i]] = m_SceneGroupDataListfoldOutState;

            if (m_SceneGroupDataListfoldOutState == false)
            {
                EditorGUILayout.EndHorizontal();
                continue;
            }

            ED_Util.EListButtons listButtonPressed = ED_Util.AddFoldOutListItemButtons();

            EditorGUILayout.EndHorizontal();

            #region Process List Changes
            // Don't allow 'up' presses for the first list item
            ED_Util.UpdateFoldOutListOnButtonPressed(m_Data.m_SceneGroupData, i, listButtonPressed);
            if (listButtonPressed != ED_Util.EListButtons.None)
                return;
            #endregion Process List Changes

            #endregion

            #region Data
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(m_Blank);
            GUILayout.BeginVertical("box");
            m_Data.m_SceneGroupData[i].m_ESceneGroupID = (GlobalEnum.ESceneGroupID)EditorGUILayout.EnumPopup("SceneGroupID", m_Data.m_SceneGroupData[i].m_ESceneGroupID);
            m_Data.m_SceneGroupData[i].m_EMainSceneID = (GlobalEnum.ESceneID)EditorGUILayout.EnumPopup("MainSceneID", m_Data.m_SceneGroupData[i].m_EMainSceneID);

            if (GUILayout.Button(new GUIContent("해당 내용 추가하기", "Click to insert data"), EditorStyles.toolbarButton) == true)
            {
                m_TempSceneGroupData = new DCSceneGroupData();
                m_TempSceneGroupData.Copy(m_Data.m_SceneGroupData[i]);
                m_Data.m_SceneGroupData.Insert(i, m_TempSceneGroupData);

                m_TempSceneGroupData = null;
            }

            #region ActiveUI List Items
            DrawUIEffectData(m_Data.m_SceneGroupData[i], m_Data.m_SceneGroupData[i].m_UIEffectData);
            #endregion

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            #endregion
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

    }

    private void DrawAutoRemove()
    {
        if (GUILayout.Button("불필요한 이펙트 리스트 지우기") == true)
        {
            List<DCSceneData> sceneDataList = DCSceneMgr.Instance.m_SceneData;

            for( int i = 0; i < sceneDataList.Count; i++ )
            {
                DCSceneData sceneData = sceneDataList[i];

                List<DCUIEffecData> effectList = sceneData.m_UIEffectData;

                for( int j = effectList.Count - 1; j > -1; j-- )
                {
                    DCUIEffecData effect = effectList[j];
                    bool remove = true;
                    foreach (EEffectName element in Enum.GetValues(typeof(EEffectName)))
                    {
                        if (effect.m_EffectName != element.ToString())
                            continue;

                        remove = false;
                        break;
                    }

                    if( remove == true )
                        effectList.RemoveAt(j);
                }
            }

            List<DCSceneGroupData> sceneGroupData = DCSceneMgr.Instance.m_SceneGroupData;

            for (int i = 0; i < sceneGroupData.Count; i++)
            {
                DCSceneGroupData sceneData = sceneGroupData[i];

                List<DCUIEffecData> effectList = sceneData.m_UIEffectData;

                for (int j = effectList.Count - 1; j > -1; j--)
                {
                    DCUIEffecData effect = effectList[j];
                    bool remove = true;
                    foreach (EEffectName element in Enum.GetValues(typeof(EEffectName)))
                    {
                        if (effect.m_EffectName != element.ToString())
                            continue;

                        remove = false;
                        break;
                    }

                    if (remove == true)
                        effectList.RemoveAt(j);
                }
            }
        }
    }

    private void FindPrefab()
    {
        EditorGUILayout.BeginVertical();

        m_UIName = EditorGUILayout.TextField( "ActiveUI", m_UIName );
        m_UICtrName = EditorGUILayout.TextField("UICtr", m_UICtrName);
        m_EffectName = EditorGUILayout.TextField("Effect", m_EffectName);

        if ( GUILayout.Button( "Find" ) == true )
        {
            string rtn = "";

            foreach( var _data in m_Data.m_SceneData )
            {
                if( false == string.IsNullOrEmpty( m_UIName ) )
                {
                    foreach( var AUI in _data.m_ActiveUIList )
                        if( true == m_UIName.Equals( AUI.m_Name ) )
                        {
                            rtn = string.Format( "{0}, {1} {2}", rtn, "[SceneData]", _data.m_KeyName );
                            break;
                        }

                }
                else if( false == string.IsNullOrEmpty( m_UICtrName ) )
                {
                    foreach( var UICtr in _data.m_UICtrData )
                        if( true == m_UICtrName.Equals( UICtr.m_UICtrName ) )
                        {
                            rtn = string.Format("{0}, {1} {2}", rtn, "[SceneData]", _data.m_KeyName );
                            break;
                        }
                }
                else if (false == string.IsNullOrEmpty(m_EffectName))
                {
                    foreach (var UIEffect in _data.m_UIEffectData)
                        if (true == m_EffectName.Equals(UIEffect.m_EffectName))
                        {
                            rtn = string.Format("{0}, {1} {2}", rtn, "[SceneData]", _data.m_KeyName);
                            break;
                        }
                }
            }

            foreach (var _data in m_Data.m_SceneGroupData)
            {
                if (false == string.IsNullOrEmpty(m_EffectName))
                {
                    foreach (var UIEffect in _data.m_UIEffectData)
                        if (true == m_EffectName.Equals(UIEffect.m_EffectName))
                        {
                            rtn = string.Format("{0}, {1} {2}", rtn, "[SceneGroupData]", _data.m_ESceneGroupID.ToString());
                            break;
                        }
                }
            }
            
            ConsoleProDebug.LogToFilterFormat( "세종", "{0}", rtn );
        }

        if (GUILayout.Button("Delete") == true)
        {
            List<DCSceneData> sceneDataList = DCSceneMgr.Instance.m_SceneData;

            for (int i = 0; i < sceneDataList.Count; i++)
            {
                DCSceneData sceneData = sceneDataList[i];

                List<DCAttachUIData> attachUIList = sceneData.m_ActiveUIList;
                
                for (int j = attachUIList.Count - 1; j > -1; j--)
                {
                    DCAttachUIData data = attachUIList[j];

                    if (true == m_UIName.Equals(data.m_Name))
                    {
                        attachUIList.RemoveAt(j);
                        ConsoleProDebug.LogToFilterFormat("두혁", "[SceneData] attachUI {0}에 있는 {1}를 지웠습니다.", sceneData.m_KeyName, data.m_Name);
                        break;
                    }
                }

                List<DCUICtrData> uICtrList = sceneData.m_UICtrData;

                for (int j = uICtrList.Count - 1; j > -1; j--)
                {
                    DCUICtrData data = uICtrList[j];

                    if (true == m_UICtrName.Equals(data.m_UICtrName))
                    {
                        uICtrList.RemoveAt(j);
                        ConsoleProDebug.LogToFilterFormat("두혁", "[SceneData] UICtr {0}에 있는 {1}를 지웠습니다.", sceneData.m_KeyName, data.m_UICtrName);
                        break;
                    }
                }

                List<DCUIEffecData> effectData = sceneData.m_UIEffectData;

                for (int j = effectData.Count - 1; j > -1; j--)
                {
                    DCUIEffecData data = effectData[j];

                    if (true == m_EffectName.Equals(data.m_EffectName))
                    {
                        effectData.RemoveAt(j);
                        ConsoleProDebug.LogToFilterFormat("두혁", "[SceneData] Effect {0}에 있는 {1}를 지웠습니다.", sceneData.m_KeyName, data.m_EffectName);
                        break;
                    }
                }
            }

            List<DCSceneGroupData> sceneGroupData = DCSceneMgr.Instance.m_SceneGroupData;

            for (int i = 0; i < sceneGroupData.Count; i++)
            {
                DCSceneGroupData sceneData = sceneGroupData[i];

                List<DCUIEffecData> effectData = sceneData.m_UIEffectData;

                for (int j = effectData.Count - 1; j > -1; j--)
                {
                    DCUIEffecData data = effectData[j];

                    if (true == m_EffectName.Equals(data.m_EffectName))
                    {
                        effectData.RemoveAt(j);
                        ConsoleProDebug.LogToFilterFormat("두혁", "[SceneGroupData] Effect {0}에 있는 {1}를 지웠습니다.", sceneData.m_ESceneGroupID.ToString(), data.m_EffectName);
                        break;
                    }
                }
            }
        }
        
        if ( false == string.IsNullOrEmpty( m_UIName ) ||
            false == string.IsNullOrEmpty( m_UICtrName ) ||
            false == string.IsNullOrEmpty(m_EffectName))
        {
            if( GUILayout.Button( "Add" ) == true &&
                false == string.IsNullOrEmpty( m_EffectName ) )
            {
                DCUIEffecData _newEfx = new DCUIEffecData();
                _newEfx.m_EffectName = m_EffectName;

                foreach( var _data in m_Data.m_SceneData )
                {

                    if( false == string.IsNullOrEmpty( m_UIName ) )
                    {
                        foreach( var AUI in _data.m_ActiveUIList )
                            if( true == m_UIName.Equals( AUI.m_Name ) )
                            {
                                if( false == HasSameEfx( _data.m_UIEffectData ) )
                                {
                                    _data.m_UIEffectData.Add( _newEfx );
                                    ConsoleProDebug.LogToFilterFormat( "세종", "Add efx in {0}", m_UIName );
                                }
                                break;
                            }
                    }
                    else if( false == string.IsNullOrEmpty( m_UICtrName ) )
                    {
                        foreach( var UICtr in _data.m_UICtrData )
                            if( true == m_UICtrName.Equals( UICtr.m_UICtrName ) )
                            {
                                if( false == HasSameEfx( _data.m_UIEffectData ) )
                                {
                                    _data.m_UIEffectData.Add( _newEfx );
                                    ConsoleProDebug.LogToFilterFormat( "세종", "Add efx in {0}", m_UICtrName );
                                }
                                break;
                            }
                    }
                    else if (false == string.IsNullOrEmpty(m_EffectName))
                    {
                        if (false == HasSameEfx(_data.m_UIEffectData))
                        {
                            _data.m_UIEffectData.Add(_newEfx);
                            ConsoleProDebug.LogToFilterFormat("세종", "Add efx in {0}", m_EffectName);
                        }
                    }
                }

                _newEfx = null;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

}
