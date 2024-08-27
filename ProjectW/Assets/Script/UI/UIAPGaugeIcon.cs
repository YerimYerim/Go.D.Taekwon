using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public class UIAPGaugeIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    private GameSpellSource _gameSpellSource;
    
    public int GetRemainSpellAP()
    {
        return _gameSpellSource.GetRemainAp();
    }
    
    public void SetSpellSource(GameSpellSource gameSpellSource)
    {
        _gameSpellSource = gameSpellSource;
        _image.sprite = GameResourceManager.Instance.GetImage(gameSpellSource.GetSourceImage());
    }
    
    public void SetPosition(Vector3 position)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        transform.gameObject.SetActive(_gameSpellSource.GetRemainAp()<= 5);
    }
    
    public void SetPositionSmooth(Vector3 position)
    {
        transform.gameObject.SetActive(_gameSpellSource.GetRemainAp()<= 5);
        LeanTween.move(gameObject, position, 0.2f);
    }
}
