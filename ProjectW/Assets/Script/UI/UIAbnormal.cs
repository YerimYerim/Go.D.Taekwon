using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbnormal : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private DTButton button;
    
    public void SetImage(string imageName)
    {
        image.sprite = GameResourceManager.Instance.GetImage(imageName);
    }

    public void SetText(string str)
    {
        text.SetText(str);
    }
    
    public void SetBGColor(string str)
    {
        BG.sprite = GameResourceManager.Instance.GetImage(str);
    }
    
    public void SetData(AbnormalTableData data)
    {
        button.SetHoverEvent(
            () =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var ui))
                {
                    ui.CreateInfo(data.abnormal_name, data.abnormal_desc, rectTransform);
                    ui.Show();
                }
            } , 
            () =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var ui))
                {
                    ui.Hide();
                }
            });
    }
    
}
