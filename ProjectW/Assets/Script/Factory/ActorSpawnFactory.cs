using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class ActorSpawnFactory
{    
    public static GameActor SpawnActorPrefab(ActorTableData actorTableData, string parentsObjectName, string actorName = "")
    {
        var rscTableData = GameTableManager.Instance._actorRscDatas.Find(_ => _.rsc_id == actorTableData.rsc_id);
        var prefab = GameResourceManager.Instance.GetLoadActorPrefab(rscTableData.actor_rsc_prefab);
        var parentsObjectTr = GameObject.Find(parentsObjectName).transform;
        if(parentsObjectTr == null)
            return null;
        
        prefab.transform.SetParent(parentsObjectTr);
        prefab.transform.position = parentsObjectTr.position;
        prefab.name = actorName.Equals(string.Empty) == false ? actorName : prefab.name;
        
        var actor = prefab.GetComponent<GameActor>();
        actor.SetResourceTable(rscTableData);
        actor.SetActorName(actorTableData);
        return actor;
    }
}
