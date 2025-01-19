using System;
using System.Linq;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;

public class UIDialogButton : MonoBehaviour
{
    [SerializeField] private DTButton button;
    [SerializeField] private TextMeshProUGUI textMesh;
    
    
    public void Set(DialogTableData dialogTableData)
    {
        SetText(dialogTableData.dialog_string);
        SetOnClickEvent(()=>ClickEvent(dialogTableData));
    }
    
    private void SetOnClickEvent(Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(()=>action());
    }
    
    private void SetText(string stringKey)
    {
        string text = GameUtil.GetString(stringKey);
        textMesh.text = text;
    }
    
    public void ClickEvent(DialogTableData dialogTableData)
    {
        switch (dialogTableData.dialog_type)
        {
            case DIALOG_TYPE.DIALOG_TYPE_BUTTON:
            {
                var buttonData = GameTableManager.Instance._dialogButtonTable.Find(_=>_.button_id == dialogTableData.button_id);

                var condition = buttonData.function_condition;

                if (condition == LOGICAL_OPERATOR.LOGICAL_OPERATOR_OR)
                {
                    // 해당 prob에 따라 버튼을 누르면 다음 다이얼로그로 넘어감
                    var prob = buttonData.function_prob;
                    var random = UnityEngine.Random.Range(0, prob.Sum());
                    if(random < prob[0])
                    {
                        OnActButtonFunction(buttonData.function_type_1 ?? BUTTON_FUNCTION.BUTTON_FUNCTION_EXIT, buttonData.value_1);
                    }
                    else if(random < prob[0] + prob[1])
                    {
                        OnActButtonFunction(buttonData.function_type_2 ?? BUTTON_FUNCTION.BUTTON_FUNCTION_EXIT, buttonData.value_2);
                    }
                }
                else if (condition == LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND)
                {
                    OnActButtonFunction(buttonData.function_type_1 ?? BUTTON_FUNCTION.BUTTON_FUNCTION_EXIT, buttonData.value_1);
                    OnActButtonFunction(buttonData.function_type_2 ?? BUTTON_FUNCTION.BUTTON_FUNCTION_EXIT, buttonData.value_2);
                }
            } break;
        }
    }

    private void OnActButtonFunction(BUTTON_FUNCTION buttonFunction, int? value)
    {
        switch (buttonFunction)
        {
            case BUTTON_FUNCTION.BUTTON_FUNCTION_REWARD:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_SELECT_REWARD:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_DELETE_SOURCE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_DELETE_SUPPORT_MODULE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_DELETE_RELIC:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_SHOP:
                if (GameUIManager.Instance.TryGetOrCreate<UI_Window_SpecialMap>(true, UILayer.LEVEL_4, out var ui))
                {
                    ui.SetUI(BUTTON_FUNCTION.BUTTON_FUNCTION_SHOP, value ?? 0);
                    ui.Show();
                }
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_ENHANCE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_EXCHANGE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_EXIT:
                var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                if (gameBattleMode == null)
                {
                    return;
                }
                gameBattleMode?.ActorHandler?.RemoveAllMonsterActors();
                gameBattleMode?.MapHandler?.ShowMapSelect();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonFunction), buttonFunction, null);
        }
    }
}
