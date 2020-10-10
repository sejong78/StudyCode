using Marlboro.StringTable;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using Champion.Util;
using static ContainerLoader;
using Marlboro.GameItem;
using Champion.UI.Gear;

public class TestUIScript : MonoBehaviour
{

    public UIGearExchangeResultPopup popup;
    private bool isInit = false;

    private IEnumerator Start()
    {
        Configuration.SetLanguage(LangId.ko_KR);

        StringBuilder stb = new StringBuilder();
        using (var commonTableLoader = new CommonTableLoader(true, LocalTablePath, TablePath, "", ExtensionName))
        {
            while (false == commonTableLoader.IsDone)
            {
                yield return null;
            }

            if (commonTableLoader.Error != null)
            {
                throw commonTableLoader.Error;
            }

            if (string.IsNullOrEmpty(commonTableLoader.Message) == false)
            {
                stb.Append(commonTableLoader.Message);
            }
        }

        StringTable.ShowErrorStringId = ShowErrorStringId;
        yield return Marlboro.Xml.XmlUtil.TableBuildAllAsync(stb);
        GearOptionTable.Load();
        isInit = true;
        Excute();
    }

	// 에디터의 인스펙터 창에서 마우스 오른쪽 클릭으로 실행가능한 코드
    public int testId = 21042; 
    [ContextMenu("Excute")]
    public void Excute()
    {
        StopCoroutine("Call");
        StartCoroutine(Call());
    }

    IEnumerator Call()
    {
        while (isInit)
        {
            popup.gameObject.SetActive(false);
            yield return null;
            Intent it = new Intent();
            var gearItem = CreateFakeGearItem();
            it["RESULT_GEAR"] = gearItem;
            popup.OnCreate(it);
            popup.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
        }
    }


    private GearItem CreateFakeGearItem()
    {
        GearItem resultGlove = new GearItem();
        var gearData = GearItemTable.Instance.Get(testId);
        resultGlove.id = testId;
        resultGlove.level = (byte)Random.Range(1, 10);
        List<int> groups = new List<int>(gearData.OptionGroupIDs);

        for (int i = 0; i < groups.Count; ++i)
        {
            int level_idx = 0;
            GearOption option = GambleOption(resultGlove, groups[i], out level_idx);
            if (option == null)
            {
                continue;
            }

            if (option.GroupType == GearOptionGroupType.MAIN)
            {
                resultGlove.MainOption_ID = GearOption.MakeOptionId(option);
            }
            else if (option.GroupType == GearOptionGroupType.FIXED)
            {
                resultGlove.Option1_ID = GearOption.MakeOptionId(option);
                resultGlove.Option1_LvIdx = (byte)level_idx;
            }
            else if(option.GroupType == GearOptionGroupType.SUB)
            {
                if (resultGlove.Option1_ID == 0)
                {
                    resultGlove.Option1_ID = GearOption.MakeOptionId(option);
                    resultGlove.Option1_LvIdx = (byte)level_idx;
                }
                else if(resultGlove.Option2_ID == 0)
                {
                    resultGlove.Option2_ID = GearOption.MakeOptionId(option);
                    resultGlove.Option2_LvIdx = (byte)level_idx;
                }
                else if (resultGlove.Option3_ID == 0)
                {
                    resultGlove.Option3_ID = GearOption.MakeOptionId(option);
                    resultGlove.Option3_LvIdx = (byte)level_idx;
                }
                else if (resultGlove.Option4_ID == 0)
                {
                    resultGlove.Option4_ID = GearOption.MakeOptionId(option);
                    resultGlove.Option4_LvIdx = (byte)level_idx;
                }
            }

            resultGlove.Options.Add(option);
        }

        return resultGlove;
    }

    private GearOption GambleOption(GearItem item, int groupId, out int level_idx)
    {
        level_idx = 0;

        var optionGroup = Marlboro.GameItem.GearOptionTable.Instance.GetOptionGroup(groupId);
        List<GearOption> exceptGroup = new List<GearOption>();

        foreach (var option in optionGroup)
        {
            var finded = item.Options.FindAll((o) =>
            {
                if (option.Value != null && o != null)
                    return (option.Value.Type == o.Type && option.Value.Param == o.Param);
                else
                    return false;
            });

            if (finded == null || finded.Count == 0)
            {
                exceptGroup.Add(option.Value);
            }
        }

        if (exceptGroup == null || exceptGroup.Count == 0)
        {
            return null;
        }

        List<GearOption> optionList = new List<GearOption>();
        foreach (var option in exceptGroup)
        {
            option.Rarity = Random.Range(2, (int)Rarity.Max+1);
            optionList.Add(option);
        }

        optionList.Sort((o1, o2) =>
        {
            return o1.OptionWeight.CompareTo(o2.OptionWeight);
        });

        if (optionList == null || optionList.Count <= 0)
        {
            return null;
        }

        int selectedOptionIdx = Random.Range(0, optionList.Count);
        level_idx = Random.Range(1, optionList[selectedOptionIdx].OptionValues.Count);
        return optionList[selectedOptionIdx];
    }
}
