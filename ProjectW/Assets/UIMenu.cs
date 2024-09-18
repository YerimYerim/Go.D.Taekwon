using Script.Manager;
using Script.UI;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField]private DTButton _button;
    
    private void Awake()
    {
        _button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        GameUIManager.Instance.TryGetOrCreate<UI_PopUp_BattleMenu>(true, UILayer.LEVEL_3, out var ui);   
        ui.Show();
    }
}
