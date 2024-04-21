using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Actor_Bottom : UIBase
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _text;
    [FormerlySerializedAs("transform")] [SerializeField] private RectTransform moveRectTransform;
    public void SetHPUI(int maxHP, int curHp)
    {
        StringBuilder stringBuilder = new();
        _progressBar.fillAmount = (float) curHp / maxHP;
        _text.text = stringBuilder.AppendFormat("{0}/{1}", curHp.ToString(), maxHP.ToString()).ToString();
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
