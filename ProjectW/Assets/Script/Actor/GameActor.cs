using Script.Manager;
using UnityEngine;

public class GameActor : MonoBehaviour
{
    private UI_Actor_Bottom uiActorBottom;
    private UI_Actor_DMGFloater uiActorDMGFloater;
    private UI_Actor_Name _uiActorName;
    
    [SerializeField] private Transform uiHpBarSocket;
    [SerializeField] private Transform uiDMGFloaterSocket;
    [SerializeField] private Transform uiPredictSocket;
    [SerializeField] private Transform uiNameSocket;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterrial;
    [SerializeField] private SPUM_SpriteList spriteList;
    [SerializeField] private SPUM_Prefabs animatingPrefab;
    
    public ActorDataBase data = new();
    public ActorRscTableData resourceData = new();
    private void Awake()
    {
        CreateUIActorBottom();
        CreateUIActorDMGFloater();
        CreateUIActorName();
        
        spriteList.LoadRenderer();
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
    private void CreateUIActorName()
    {
        if (GameUIManager.Instance.TryCreate<UI_Actor_Name>(true, UILayer.LEVEL_3, out var ui))
        {
            _uiActorName = ui;
            _uiActorName.Show();
        }
    }

    private void Start()
    {
        uiActorBottom.SetPosition(this.uiHpBarSocket);
        uiActorBottom.SetPredictPosition(this.uiPredictSocket);
        uiActorDMGFloater.SetPosition(this.uiDMGFloaterSocket);
        _uiActorName.SetPosition(this.uiNameSocket);
        uiActorBottom.ShowDefFloater(0);
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
            uiActorBottom.Show();
            if (uiActorBottom._curHp != 0)
            {
                uiActorDMGFloater.ShowDamage(lastData.Hp - uiActorBottom._curHp);
            }
            uiActorBottom.SetHPUI(lastData.MaxHp, lastData.Hp);
            uiActorBottom.SetDef(lastData.GetAmor());
            uiActorBottom.SetGrid(lastData);
            uiActorBottom.ShowPredictAction(lastData);
            Debug.Log(gameObject.name + "의 체력이 " + lastData.Hp + "방어도 " + lastData.GetAmor());
        }
    }

    public void OnAddDef(int addDef)
    {
        uiActorBottom.ShowDefFloater(addDef);
    }
    public void OnSelected()
    {
        for(int i = 0; i< spriteList._outlineRender.Count; ++i)
        {
            spriteList._outlineRender[i].material = outlineMaterial;
        }
    }
    public void OnDeselected()
    {
        for(int i = 0; i< spriteList._outlineRender.Count; ++i)
        {
            spriteList._outlineRender[i].material = normalMaterrial;
        }
    }
    
    public void UpdateTurnSkill()
    {
        foreach (var turnSkill in data.turnSkill)
        {
            if (turnSkill is ISkillTurnSkill skill)
            {
                skill.DoTurnSkill(this);
            }
        }
        RemoveTurnOverSkill();
    }
    public void RemoveTurnOverSkill()
    {
        for(int i = 0; i< data.turnSkill.Count; ++i)
        {
            SkillEffectBase node = data.turnSkill[i] as SkillEffectBase;
            if (node?.IsNotRemainTurn() == true)
            {
                var turnSkill = node as ISkillTurnSkill;
                turnSkill?.DoTurnEndSkill(this);
                data.turnSkill.RemoveAt(i);
                --i;
            }
        }
    }
    
    public void RemoveAllTurnSkill()
    {
        data.turnSkill.Clear();
    }
    private void OnDestroy()
    {
        if (uiActorBottom != null)
        {
            uiActorBottom.SetHPUI(0, 0);
            uiActorBottom.Hide();
        }
        if (uiActorDMGFloater != null)
            uiActorDMGFloater.Hide();
        if (_uiActorName != null)
            _uiActorName.Hide();
    }
    
    public void SetResourceTable(ActorRscTableData table)
    {
        resourceData = table;
    }

    public void SetActorName(ActorTableData actorTableData)
    {
        //_uiActorName.ShowName(GameUtil.GetString(actorTableData.actor_name));
        _uiActorName.ShowName(actorTableData.actor_id.ToString() + "\n" + actorTableData.actor_type.ToString());
    }

    public void DoAnim(SPUM_Prefabs.AnimationType type)
    {
        animatingPrefab.PlayAnimation(type);
    }
}
