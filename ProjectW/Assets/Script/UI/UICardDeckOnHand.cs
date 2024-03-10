using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICardDeckOnHand : MonoBehaviour
{
    public class SpellData
    {
        public SpellTableData tableData;
    }
    [SerializeField] private List<UISpell> uiCards = new List<UISpell>();
    [SerializeField] private GridLayoutGroup gridLayout;

    private List<SpellData> spellDatas = new List<SpellData>();


    private void Awake()
    {
        uiCards.AddRange(gridLayout.transform.GetComponentsInChildren<UISpell>());
        
        
        // ?? 예림  : 단순 기능 확인을 위한 테스트 나중에 삭제할 부분
        GameDataManager.Instance.LoadData();
        SetUI(new []{1,2,3,4});
    }

    public void SetUI(int[] spellCardKey)
    {
        spellDatas.Clear();
        
        foreach (var cardKey in spellCardKey)
        {
            SpellTableData spellTableData = GameDataManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
            spellDatas.Add(new SpellData()
            {
                tableData = spellTableData
            });
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
}
