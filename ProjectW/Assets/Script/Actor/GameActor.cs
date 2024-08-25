using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameActor : MonoBehaviour
{
    private UI_Actor_Bottom uiActorBottom;
    private UI_Actor_DMGFloater uiActorDMGFloater;
    
    [SerializeField] private Transform uiHpBarSocket;
    [SerializeField] private Transform uiDMGFloaterSocket;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterrial;
    [SerializeField] private SpriteRenderer renderer;
    public ActorDataBase data = new();
    
    private void Awake()
    {
        CreateUIActorBottom();
        CreateUIActorDMGFloater();
    }
    
    private void CreateUIActorBottom()
    {
        if (GameUIManager.Instance.TryCreate<UI_Actor_Bottom>(true, UILayer.LEVEL_2, out var ui))
        {
            uiActorBottom = ui;
            uiActorBottom.Show();
        }
    }
    
    private void CreateUIActorDMGFloater()
    {
        if (GameUIManager.Instance.TryCreate<UI_Actor_DMGFloater>(true, UILayer.LEVEL_3, out var ui))
        {
            uiActorDMGFloater = ui;
            uiActorDMGFloater.Show();
        }
    }
    
    
    
    private void Start()
    {
        uiActorBottom.SetPosition(this.uiHpBarSocket);
        uiActorDMGFloater.SetPosition(this.uiDMGFloaterSocket);
    }

    public void OnUpdateHp(ActorDataBase lastData)
    {
        if (lastData.Hp <= 0)
        {
            data = lastData;
            this.gameObject.SetActive(false);
            uiActorBottom.Hide();
        }
        else
        {
            data = lastData;
            uiActorDMGFloater.ShowDamage(lastData.Hp -uiActorBottom._curHp);
            uiActorBottom.SetHPUI(lastData.MaxHp, lastData.Hp);
            uiActorBottom.SetDef(lastData.GetAmor());
            Debug.Log(gameObject.name + "의 체력이 " + lastData.Hp + "방어도 " + lastData.GetAmor());
        }
    }

    public void OnAddDef(int addDef)
    {
        uiActorBottom.ShowDefFloater(addDef);
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

    private void OnDestroy()
    {
        if (uiActorBottom != null)
            uiActorBottom.Hide();
    }
}
