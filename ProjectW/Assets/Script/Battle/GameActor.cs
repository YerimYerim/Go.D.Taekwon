using System.Collections.Generic;
using System.Linq;
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
        if (data.Hp <= 0)
        {
            this.gameObject.SetActive(false);
            uiActorBottom.Hide();
        }
        else
        {
            uiActorBottom.SetHPUI(data.MaxHp, data.Hp);
        }
    }

    public void OnSelected()
    {
        renderer.material = outlineMaterial;
    }
    public void OnDeselected()
    {
        renderer.material = normalMaterrial;
    }
    
    
    public void UpdateDebuff()
    {
        foreach (var debuff in data.debuffs)
        {
            debuff.DoDebuff(this);
        }
        RemoveTurnOverDebuff();
    }

    public void UpdateBuff()
    {
        foreach (var buff in data.buffs)
        {
            buff.DoBuff(this);
        }
    }

    public void RemoveTurnOverDebuff()
    {
        var currentNode = data.debuffs.First;
        while (currentNode != null)
        {
            var node = (SkillEffectBase)currentNode.Value;
            if (node.IsNotRemainTurn())
            {
                var nextNode = currentNode.Next; // 다음 노드를 미리 저장
                data.debuffs.Remove(currentNode); // 현재 노드 제거
                currentNode = nextNode; // 현재 노드를 다음 노드로 업데이트
            }
            else
            {
                currentNode = currentNode.Next; // 다음 노드로 이동
            }
        }
    }
}
