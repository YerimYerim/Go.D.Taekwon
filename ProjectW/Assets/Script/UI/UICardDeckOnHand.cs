using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICardDeckOnHand : UIBase
{
    [SerializeField] private List<UISpell> uiCards = new List<UISpell>();
    [SerializeField] private GridLayoutGroup gridLayout;

    private void OnEnable()
    {

    }

    private void OnDestroy()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.OnEventRemoveCard -= RemoveCard;
        gameBattleMode.BattleHandler.OnUpdateCard -= SetUI;
    }

    private void Awake()
    {
        uiCards.AddRange(gridLayout.transform.GetComponentsInChildren<UISpell>());

        foreach (var card in uiCards)
        {
            card._parents = this;
        }
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if(gameBattleMode == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.OnEventRemoveCard += RemoveCard;
        gameBattleMode.BattleHandler.OnUpdateCard += SetUI;
        //SetUI();
    }

    public void SetUI()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        for (int i = 0; i < uiCards.Count; ++i)
        {
            if (i < gameBattleMode.BattleHandler.spellDatas.Count)
            {
                uiCards[i].gameObject.SetActive(true);
                uiCards[i].SetUI(gameBattleMode.BattleHandler.spellDatas[i]);
            }
            else
            {
                uiCards[i].gameObject.SetActive(false);
            }
            
        }
    }

    public void MergeSpell(int spellID01, int spellID2 ,int resultSpellID)
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.spellDatas.Remove(gameBattleMode.BattleHandler.spellDatas.Find(_ => _.tableData.spell_id == spellID01));
        gameBattleMode.BattleHandler.spellDatas.Remove(gameBattleMode.BattleHandler.spellDatas.Find(_ => _.tableData.spell_id == spellID2));
        gameBattleMode.BattleHandler.AddSpell(resultSpellID, 1);
        SetUI();
    }

    public void RemoveCard()
    {
        SetUI();
    }
}
