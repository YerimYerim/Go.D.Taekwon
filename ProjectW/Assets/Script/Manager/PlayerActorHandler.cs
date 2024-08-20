using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorHandler
{
    public GameActor player
    {
        get;
        private set;
    }
    
    public ActorDataBase playerData
    {
        get;
        private set;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void Init(int selectActorId)
    {
        ActorTableData actorTableData = GameDataManager.Instance._actorDatas.Find(_ => _.actor_id == selectActorId);
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var actorSpawner = gameBattleMode?.BattleActorSpawner;
        
        if(actorSpawner == null)
            return;
        
        var actorPrefab = actorSpawner.SpawnActorPrefab(actorTableData, GameUtil.PLAEYER_PARENT_NAME);
        
        player = actorSpawner.GetActor(actorPrefab.name);
        playerData = new ActorPlayerData();
        var playableTableData = GameDataManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == selectActorId);
        playerData.Init(playableTableData?.stat_hp ?? 0);
        player.data = playerData;
        player.OnUpdateHp(playerData);
    }

    public int GetPlayerHp()
    {
        return playerData.GetHp();
    }
}
