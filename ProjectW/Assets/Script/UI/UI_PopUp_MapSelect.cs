using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class UI_PopUp_MapSelect : UIBase
{
    [SerializeField] UIPopUpMapSelectObject[] _mapSelectObjects;

    public void SetData(List<ContentMapTableData> data)
    {
        for(int i = 0; i< _mapSelectObjects.Length; ++i)
        {
            if (i < data.Count)
            {
                _mapSelectObjects[i].SetData(GameResourceManager.Instance.GetImage(data[i].map_img), data[i].map_name, data[i].map_desc, data[i].map_type ?? MAP_TYPE.MAP_TYPE_BATTLE_NORMAL);
                var captureIndex = i;
                _mapSelectObjects[i].SetButtonEvent(()=>OnClickMapSelect(data[captureIndex]));
                _mapSelectObjects[i].gameObject.SetActive(true);
            }
            else
            {
                _mapSelectObjects[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnClickMapSelect(ContentMapTableData data)
    {

        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode == null)
        {
            return;
        }
        gameBattleMode?.MapHandler?.OnClickMapSelect(data);
        Hide();
    }
    
}
