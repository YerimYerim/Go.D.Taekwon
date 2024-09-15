using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public class UIPredictAction : UIBase
{
    [SerializeField] private Image iconImage;

    public void ShowPredictAction(string imageName)
    {
        Show();
        SetIcon(imageName);
    }
    public void HidePredictAction()
    {
        Hide();
    }

    private void SetIcon(string imageName)
    {
        var icon = GameResourceManager.Instance.GetImage(imageName);
        iconImage.sprite = icon;
    }
}
