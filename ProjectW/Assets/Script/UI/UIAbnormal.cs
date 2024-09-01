using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbnormal : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    
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
}
