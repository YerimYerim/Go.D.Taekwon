using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIAPGaugeIconBase : MonoBehaviour
{
    [SerializeField] protected Image _image;

    public abstract int GetRemainSpellAP();

    protected void SetImage(string image)
    {
        _image.sprite = GameResourceManager.Instance.GetImage(image);
    }
    public void SetPosition(Vector3 position)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        transform.gameObject.SetActive(GetRemainSpellAP()<= 5);
    }

    public void SetPositionSmooth(Vector3 position)
    {
        transform.gameObject.SetActive(GetRemainSpellAP()<= 5);
        LeanTween.move(gameObject, position, 0.2f);
    }
}
