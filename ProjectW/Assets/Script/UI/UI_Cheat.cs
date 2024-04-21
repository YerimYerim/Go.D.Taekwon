using System;
using System.Collections;
using System.Collections.Generic;
using Script.UI;
using TMPro;
using UnityEngine;

public class UI_Cheat : MonoBehaviour
{
    [SerializeField] private DTButton[] _buttons;
    [SerializeField] private TextMeshProUGUI[] _text;
    private void Awake()
    {
        _buttons[0].onClick.AddListener(OnClick1);
        _buttons[1].onClick.AddListener(OnClick2);
        _buttons[2].onClick.AddListener(OnClick3);
        _buttons[3].onClick.AddListener(OnClick4);
        _buttons[4].onClick.AddListener(OnClick5);
        //_buttons[5].onClick.AddListener(OnClick6); 
    }

    private void SetText()
    {
    }
    void OnClick1()
    {
        GameBattleManager.Instance.Attack(5);   
        SetText();
    }
    void OnClick2()
    {
        GameBattleManager.Instance.Damaged(5);
        SetText();
    }
    void OnClick3()
    {
        GameBattleManager.Instance.HealEnemyActor(5);
        SetText();
    }
    void OnClick4()
    {
        GameBattleManager.Instance.HealMyActor(5);
        SetText();
    }
    void OnClick5()
    {
        GameTurnManager.Instance.TurnNodeInit();
        SetText();
    }
    void OnClick6()
    {
        GameTurnManager.Instance.TurnNodeInit();
        GameTurnManager.Instance.TurnStart();
        SetText();
    }
}
