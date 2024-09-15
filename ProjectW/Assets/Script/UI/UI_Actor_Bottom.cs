using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Actor_Bottom : UIBase
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform moveRectTransform;

    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private RectTransform defRectTransform;
    [SerializeField] private Image defImage;
    [SerializeField] private TextMeshProUGUI defFloater;
    [SerializeField] private UIACGrid _uiacGrid;

    [SerializeField] private UIPredictAction _uiPredictAction;
    
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
        _text.text = $"{curHp.ToString()}/{maxHP.ToString()}"; //, ,).ToString();
    }

    public void SetDef(int def)
    {
        defRectTransform.gameObject.SetActive(def > 0);
        if (def > 0)
        {
            defText.text = def.ToString();
        }
    }

    public void SetPosition(Transform gameTransform)
    {
        if (Camera.main != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(gameTransform.position);
            moveRectTransform.transform.position = screenPos;
            defRectTransform.transform.position = screenPos;
            defFloater.text = "";
        }
    }

    public void ShowDefFloater(int addDef)
    {
        defFloater.text = "+" + addDef;
        LeanTween.alpha(defFloater.gameObject, 1, 0.8f).setOnComplete(() =>
        {
            LeanTween.alpha(defFloater.gameObject, 0, 1f).setOnComplete(() => { defFloater.text = ""; });
        });
    }
    
    public void SetGrid(ActorDataBase dataBase)
    {
        _uiacGrid.Set(dataBase);
    }
    
    public void ShowPredictAction(ActorDataBase dataBase)
    {
        var enemy = dataBase as ActorEnemyData;
        if (enemy == null)
        {
            _uiPredictAction.Hide();
            return;
        }

        _uiPredictAction.Show();
        //_uiPredictAction.ShowPredictAction();
    }
}
