using Script.UI;
using UnityEngine;

public class UI_PopUp_Die : UIBase
{
    [SerializeField] private DTButton closeButton;
    
    
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
        SetHideEvent(HideAction);
    }

    private void HideAction()
    {
        GameInstanceManager.Instance.BattleStart();
    }
}
