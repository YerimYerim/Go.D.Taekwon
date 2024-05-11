using Script.Manager;
using UnityEngine;

public class GameActor : MonoBehaviour
{
    private UI_Actor_Bottom uiActorBottom;
    [SerializeField] private Transform uiHpBarSocket;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterrial;
    [SerializeField] private SpriteRenderer renderer;
    public ActorDataBase data = new();

    private void Awake()
    {
        GameActormanager.Instance.AddActors(transform.gameObject.name, this);
        CreateUIActorBottom();
    }

    private void CreateUIActorBottom()
    {
        if (GameUIManager.Instance.TryCreate<UI_Actor_Bottom>(true, UILayer.LEVEL_2, out var ui))
        {
            uiActorBottom = ui;
            uiActorBottom.Show();
        }
    }
    
    private void Start()
    {
        uiActorBottom.SetPosition(this.uiHpBarSocket);
    }

    public void OnUpdateHp()
    {
        uiActorBottom.SetHPUI(data.MaxHp, data.Hp);
    }

    public void OnSelected()
    {
        renderer.material = outlineMaterial;
    }
    public void OnDeselected()
    {
        renderer.material = normalMaterrial;
    }
}
