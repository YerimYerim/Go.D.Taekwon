using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardDeckOnHand : UIBase
{
    [SerializeField] private List<UISpell> uiCards = new List<UISpell>();
    [SerializeField] private GridLayoutGroup gridLayout;
    
    private void OnEnable()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if(gameBattleMode == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.OnEventRemoveCard += RemoveCard;
        gameBattleMode.BattleHandler.OnUpdateCard += SetUI;
    }

    private void OnDisable()
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
        SetUI();
    }

    public void SetUI()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        for (int i = 0; i < uiCards.Count; ++i)
        {
            if (i < gameBattleMode.BattleHandler.spellDatas.Count)
            {
                //uiCards[i].gameObject.SetActive(true);
                uiCards[i].SetIndex(i);
                uiCards[i].SetUI(gameBattleMode.BattleHandler.spellDatas[i], i);
            }
            else
            {
                uiCards[i].gameObject.SetActive(false);
                uiCards[i].SetIndex(i);
            }
        }
    }

    public void MergeSpell(int fromIndex, int toIndex ,int resultSpellID)
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.AddSpell(resultSpellID, 1, toIndex);
        
        gameBattleMode.BattleHandler.spellDatas.RemoveAt(toIndex + 1);
        gameBattleMode.BattleHandler.spellDatas.RemoveAt(fromIndex);
        
        gameBattleMode.BattleHandler.MinusResourceAP(1);
        gameBattleMode.ActorHandler.MinusAP(1);
        gameBattleMode.BattleHandler.UpdateUIApGauge();
        GameTurnManager.Instance.TurnStart();
        SetUI();
    }

    public void RemoveCard()
    {
        SetUI();
    }
}
