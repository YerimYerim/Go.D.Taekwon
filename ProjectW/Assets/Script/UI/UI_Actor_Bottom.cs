using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Actor_Bottom : UIBase
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform moveRectTransform;
    
    [SerializeField] private Transform apParent;
    
    private const string AbnormalPrefabName = "UIAbnormal";
    private List<UIAbnormal> _coolTimeAbnormal = new();
    private int hp;
    public int _curHp
    {
        get => hp;
        private set => hp = value;
    }
    public void SetHPUI(int maxHP, int curHp)
    {
        _curHp = curHp;
        _progressBar.fillAmount = (float) curHp / maxHP;
        _text.text = $"{curHp.ToString()}/{ maxHP.ToString()}"; //, ,).ToString();
    }

    public void SetPosition(Transform gameTransform)
    {
        if (Camera.main != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(gameTransform.position);
            moveRectTransform.transform.position = screenPos;
        }
    }

    public void UpdateAbnormal()
    {
        //_coolTimeAbnormal
    }
}
