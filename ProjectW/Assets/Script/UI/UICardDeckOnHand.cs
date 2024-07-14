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
        
        GameBattleManager.Instance.OnEventRemoveCard += RemoveCard;
    }

    private void OnDisable()
    {
        GameBattleManager.Instance.OnEventRemoveCard -= RemoveCard;
    }

    private void Awake()
    {
        uiCards.AddRange(gridLayout.transform.GetComponentsInChildren<UISpell>());

        foreach (var card in uiCards)
        {
            card._parents = this;
        }
        // ?? : 단순 기능 확인을 위한 테스트 나중에 삭제할 부분
        GameDataManager.Instance.LoadData();
        

        GameBattleManager.Instance.spellIDs.AddRange(new []{10101,10103, 20104,20107,});
        SetUI();
    }

    public void SetUI()
    {
        GameBattleManager.Instance.spellDatas.Clear();
        foreach (var cardKey in  GameBattleManager.Instance.spellIDs)
        {
            SpellTableData spellTableData = GameDataManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
            var spellData = new GameDeckManager.SpellData(spellTableData);
            GameBattleManager.Instance.spellDatas.Add(spellData);
        }

        for (int i = 0; i < uiCards.Count; ++i)
        {
            if (i < GameBattleManager.Instance.spellDatas.Count)
            {
                uiCards[i].gameObject.SetActive(true);
                uiCards[i].SetUI(GameBattleManager.Instance.spellDatas[i]);
            }
            else
            {
                uiCards[i].gameObject.SetActive(false);
            }
            
        }
    }

    public void MergeSpell(int spellID01, int spellID2 ,int resultSpellID)
    {
        GameBattleManager.Instance.spellIDs.Remove(spellID01);
        GameBattleManager.Instance.spellIDs.Remove(spellID2);
        GameBattleManager.Instance.spellIDs.Add(resultSpellID);
        SetUI();
    }

    public void RemoveCard(int id)
    {
        SetUI();
    }
}
