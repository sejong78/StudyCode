using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Marlboro.StringTable;
//using MaldivesUnity.LitJson;
/// MaldivesUnity.LitJson 는 Parsing Error 가 발생합니다.
using LitJson;

/*
/// <summary>
/// 구글 스프레드 시트 정보를 받아와 스트링 테이블 정보를 업데이트 합니다.
/// 시트 이름은 LangId 로 접근합니다.
/// </summary>
public class StringTableUpdater : MonoSingleton<StringTableUpdater>
{
	public string webServiceUrl = "";
	public string spreadsheetId = "";
	public string worksheetName
    {
        get
        {
            return EnumUtil<LangId>.Instance.GetEnumString(StringTable.Language);
        }
    }

	public string password = "";
	public float maxWaitTime = 60f;
	public bool debugMode;

	bool updating;
    bool use;
    
    /// <summary>
    /// 텍스트가 아닌 사용중인 스트링 아이디를 노출합니다.
    /// </summary>
    bool showId;

	string currentStatus;

	Rect guiBoxRect;
	Rect guiButtonRect;
	Rect guiButtonRect2;
	Rect guiButtonRect3;

    /// <summary>
    /// 터치 업 이벤트 처리를 위한 정보
    /// </summary>
    private bool touchDown;
    private int touchCount;

    private Dictionary<string, string> stringTable = new Dictionary<string,string>();
	
	void Start ()
	{
		updating = false;
		currentStatus = "Offline";

        int buttonHeight = Screen.currentResolution.height / 20;
        int buttonWidth = buttonHeight * 4;

        int top = 10;
		
        guiBoxRect = new Rect(10, top, buttonWidth + 40, buttonHeight * 2 + 40);
        top += 20;

		guiButtonRect = new Rect(30, top, buttonWidth, buttonHeight);
        top += (buttonHeight + 10);

		guiButtonRect2 = new Rect(30, top, buttonWidth, buttonHeight);
		//guiButtonRect3 = new Rect(30, 310, 270, 130);

        if (StringTable.Instance != null)
        {
            use = true;
            SetEditing(true);
        }
	}
	
	void OnGUI()
	{
		GUI.Box(guiBoxRect, currentStatus);

        string useButton = use ? "Off" : "On";
        if (GUI.Button(guiButtonRect, useButton))
        {
            ToggleUse();
        }

        GUI.enabled = !updating && use;
		if (GUI.Button(guiButtonRect2, "Update"))
		{
			Connect();
		}

        //GUI.enabled = !updating;
        //string showButton = showId ? "Show Text" : "Show ID";
        //if (GUI.Button(guiButtonRect3, showButton))
        //{
        //    showId = !showId;
        //    if (UIPanelManager.available)
        //    {
        //        string message = showId ? "OnShowWithId" : "OnShowWithText";
        //        UIPanelManager.instance.BroadcastToAll(message);
        //    }
        //}

        GUI.enabled = true;
	}

    void ToggleUse()
    {
        use = !use;
        SetEditing(use);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touchDown = true;
            touchCount = Mathf.Max(Input.touchCount, touchCount);
        }
        else if (Input.touchCount == 0 && touchDown)
        {
            switch (touchCount)
            {
            case 3:
                Connect();
                break;
            case 4:
                ToggleUse();
                break;
            default:
                break;
            }
            touchDown = false;
        }
    }
	
	void Connect()
	{
		if (updating)
        {
            return;
        }
		
		updating = true;
		StartCoroutine(GetData());
	}
	
	IEnumerator GetData()
	{
        string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + worksheetName + "&pass=" + password + "&action=GetData";
		if (debugMode)
        {
#if DEBUG
            Debug.Log("Connecting to webservice on " + connectionString);
#endif
        }

		WWW www = new WWW(connectionString);
		
		float elapsedTime = 0.0f;
		currentStatus = "Stablishing Connection... ";
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				currentStatus = "Max wait time reached, connection aborted.";
				Debug.Log(currentStatus);
				updating = false;
				break;
			}
			
			yield return null;  
		}
	
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}

		string response = www.text;
#if DEBUG
		//Debug.Log(string.Format("elapsedTime : {0}\n{1}", elapsedTime, response));
#endif
		currentStatus = "Connection stablished, parsing data...";

		if (response == "\"Incorrect Password.\"")
		{
			currentStatus = "Connection error: Incorrect Password.";
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}

		try 
		{
            var items = JsonMapper.ToObject<JsonData[]>(response);
            UpdateTable(items);
            currentStatus = "StringTable Update Success";
        }
		catch (System.Exception e)
		{
			currentStatus = "Data error: could not parse retrieved data as json.";
#if DEBUG
			Debug.LogError(e.Message);
#endif
		}
        finally
        {
            updating = false;
        }
	}

    private void UpdateTable(JsonData[] items)
    {
        stringTable.Clear();

        foreach (var o in items)
        {
            if (o == null || !o.Keys.Contains("Key") || !o.Keys.Contains("Text"))
            //if (o == null || !o.ContainsKey("Key") || !o.ContainsKey("Text"))
            {
                continue;
            }

            string stringKey = o["Key"].ToString();
            string stringValue = o["Text"].ToString();

            if (string.IsNullOrEmpty(stringKey))
            {
                continue;
            }

            if (stringTable.ContainsKey(stringKey))
            {
#if DEBUG
                Debug.LogError("[StringTable] 중복된 키 입니다: " + stringKey);
#endif
                continue;
            }

            stringValue = stringValue.Replace("\\n", "\n");

            stringTable.Add(stringKey, stringValue);
            //Debug.Log(string.Format("[StringTagble] {0} => {1}", stringKey, stringValue));
        }

        SetEditing(true);
    }

    /// <summary>
    /// 스트링 수정 기능을 활성화/비활성화 합니다.
    /// </summary>
    /// <param name="use"></param>
    private void SetEditing(bool use)
    {
        if (StringTable.Instance == null)
        {
            return;
        }

        if (use)
        {
            StringTable.Instance.SetEditingFunc(GetEditingText);
        }
        else
        {
            StringTable.Instance.ResetEditing();
        }

        if (UIPanelManager.available)
        {
            UIPanelManager.instance.BroadcastToAll("OnStringTableUpdate");
        }
    }

    /// <summary>
    /// 수정중인 텍스트가 있을 경우 반환합니다.
    /// </summary>
    /// <param name="stringId"></param>
    /// <returns></returns>
    private string GetEditingText(string stringId)
    {
        string s = string.Empty;
        stringTable.TryGetValue(stringId, out s);
        return s;
    }

    public override void OnReset()
    {
        stringTable.Clear();
        updating = false;
    }
}
*/