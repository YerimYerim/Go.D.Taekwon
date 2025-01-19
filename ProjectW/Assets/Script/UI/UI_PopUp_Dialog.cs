using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using TMPro;
using UnityEngine;

public class UI_PopUp_Dialog : UIBase
{
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private UIDialogButton _dialogButton;
    [SerializeField] private List<UIDialogButton> _dialogButtons;
    
    [SerializeField] private TextMeshProUGUI _dialogTitle;
    [SerializeField] private TextMeshProUGUI _dialogText;
    
    public void SetUI(int MapId)
    {
        List<DialogTableData> dialogData = GameTableManager.Instance._dialogTable.FindAll(_ => _.dialog_id == MapId);
        
        for(int i = 0; i < dialogData.Count; ++i)
        {
            var dialog = dialogData[i];
            switch (dialog.dialog_type)
            {
                case DIALOG_TYPE.DIALOG_TYPE_BUTTON:
                    var dialogButton = Instantiate(_dialogButton, _dialogPanel.transform);
                    dialogButton.Set(dialog);
                    _dialogButtons.Add(dialogButton);
                    break;
            }

            _dialogText.text = GameUtil.GetString(dialog.dialog_string);
        }
    }
}
