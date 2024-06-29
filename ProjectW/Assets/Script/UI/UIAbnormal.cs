using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbnormal : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    
    private void SetImage(string imageName)
    {
        image.sprite = GameResourceManager.Instance.GetImage(imageName);
    }

    private void SetText(string str)
    {
        text.SetText(str);
    }
}
