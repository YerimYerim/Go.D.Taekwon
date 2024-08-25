using System;
using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISpellSource : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Image _image;

    [SerializeField] private int sourceIndex = 0;
    public void SetText(int remain)
    {
        _textMeshProUGUI.text = remain.ToString();
    }

    private void OnEnable()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode?.BattleHandler == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.GetSource(sourceIndex).OnUpdateUI += SetText;
    }

    private void OnDestroy()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode?.BattleHandler == null)
        {
            return;
        }
        gameBattleMode.BattleHandler.GetSource(sourceIndex).OnUpdateUI -= SetText;
    }
}
