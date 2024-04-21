using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICardDeckOnHand : MonoBehaviour
{
    [SerializeField] private List<UISpell> uiCards = new List<UISpell>();
    [SerializeField] private GridLayoutGroup gridLayout;

    private List<GameDeckManager.SpellData> spellDatas = new();
    public List<int> spellIDs = new();
    private int _yearOfBirth;
    private void Awake()
    {
        uiCards.AddRange(gridLayout.transform.GetComponentsInChildren<UISpell>());

        foreach (var card in uiCards)
        {
            card._parents = this;
        }
        // ?? 예림  : 단순 기능 확인을 위한 테스트 나중에 삭제할 부분
        GameDataManager.Instance.LoadData();
        SetUI(new []{1,2,3,4});
    }

    public void SetUI(int[] spellCardKey)
    {
        spellDatas.Clear();
        
        spellIDs.Clear();
        spellIDs.AddRange(spellCardKey);
        foreach (var cardKey in spellCardKey)
        {
            SpellTableData spellTableData = GameDataManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
            var spellData = new GameDeckManager.SpellData(spellTableData);
            spellDatas.Add(spellData);
        }

        for (int i = 0; i < uiCards.Count; ++i)
        {
            if (i < spellDatas.Count)
            {
                uiCards[i].gameObject.SetActive(true);
                uiCards[i].SetUI(spellDatas[i]);
            }
            else
            {
                uiCards[i].gameObject.SetActive(false);
            }
            
        }
    }

    public void MergeSpell(int spellID01, int spellID2 ,int resultSpellID)
    {
        spellIDs.Remove(spellID01);
        spellIDs.Remove(spellID2);
        spellIDs.Add(resultSpellID);
        SetUI(spellIDs.ToArray());
    }
}
