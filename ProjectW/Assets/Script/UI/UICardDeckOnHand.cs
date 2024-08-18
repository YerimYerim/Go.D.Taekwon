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
        GameBattleManager.Instance.OnUpdateCard += SetUI;
    }

    private void OnDisable()
    {
        GameBattleManager.Instance.OnEventRemoveCard -= RemoveCard;
        GameBattleManager.Instance.OnUpdateCard -= SetUI;
    }

    private void Awake()
    {
        uiCards.AddRange(gridLayout.transform.GetComponentsInChildren<UISpell>());

        foreach (var card in uiCards)
        {
            card._parents = this;
        }
        // ?? : 단순 기능 확인을 위한 테스트 나중에 삭제할 부분
 
        SetUI();
    }

    public void SetUI()
    {
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
        GameBattleManager.Instance.spellDatas.Remove(GameBattleManager.Instance.spellDatas.Find(_ => _.tableData.spell_id == spellID01));
        GameBattleManager.Instance.spellDatas.Remove(GameBattleManager.Instance.spellDatas.Find(_ => _.tableData.spell_id == spellID2));
        GameBattleManager.Instance.AddSpell(resultSpellID, 1);
        SetUI();
    }

    public void RemoveCard()
    {
        SetUI();
    }
}
