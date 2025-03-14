using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PopUp_Dialog : UIBase
{
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private UIDialogButton _dialogButton;
    [SerializeField] private List<UIDialogButton> _dialogButtons;
    
    [SerializeField] private TextMeshProUGUI _dialogTitle;
    [SerializeField] private TextMeshProUGUI _dialogText;
    
    public void SetUI(int mapId)
    {
        List<DialogTableData> dialogData = GameTableManager.Instance._dialogTable.FindAll(_ => _.dialog_id == mapId);
        
        _dialogText.transform.gameObject.SetActive(false);
        _dialogTitle.transform.gameObject.SetActive(false);
        
        for(int i = 0; i < dialogData.Count; ++i)
        {
            var dialog = dialogData[i]; 
            switch (dialog.dialog_type)
            {
                case DIALOG_TYPE.DIALOG_TYPE_BUTTON:
                {
                    var dialogButton = Instantiate(_dialogButton, _dialogPanel.transform);
                    dialogButton.Set(dialog);
                    _dialogButtons.Add(dialogButton);
                } break;
                case DIALOG_TYPE.DIALOG_TYPE_TITLE:
                {
                    _dialogTitle.transform.gameObject.SetActive(true);
                    _dialogTitle.text = GameUtil.GetString(dialog.dialog_string);
                } break;
                case DIALOG_TYPE.DIALOG_TYPE_TEXT:
                {
                    _dialogText.transform.gameObject.SetActive(true);
                    _dialogText.text = GameUtil.GetString(dialog.dialog_string);
                } break;
            }
            _dialogText.text = GameUtil.GetString(dialog.dialog_string);
        }
        _dialogButton.transform.gameObject.SetActive(false);
    }

    protected override void OnHide(params object[] param)
    {
        base.OnHide(param);
        for(int i = 0; i < _dialogButtons.Count; ++i)
        {
            Destroy(_dialogButtons[i].gameObject);
        }
        _dialogButtons.Clear();
    }
}
