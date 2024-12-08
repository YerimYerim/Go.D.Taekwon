using TMPro;
using UnityEngine;

public class UI_Actor_Name : UIBase
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform moveRectTransform;

    public void ShowName(string actorName)
    {
        text.text = actorName;
        Show();
    }

    public void SetPosition(Transform gameTransform)
    {
        if (Camera.main != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(gameTransform.position);
            moveRectTransform.transform.position = screenPos;
        }
    }

}
