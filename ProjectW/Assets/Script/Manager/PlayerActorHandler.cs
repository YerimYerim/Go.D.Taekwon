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
        ActorTableData actorTableData = GameTableManager.Instance._actorDatas.Find(_ => _.actor_id == selectActorId);
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var actorSpawner = gameBattleMode?.ActorHandler;
        
        if(actorSpawner == null)
            return;
        
        var actorPrefab = actorSpawner.SpawnActor(actorTableData, GameUtil.PLAEYER_PARENT_NAME);
        player = actorSpawner.GetActor(actorPrefab.name);
        
        var playableTableData = GameTableManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == selectActorId);
        
        playerData = new ActorPlayerData();
        playerData.Init(playableTableData?.stat_hp ?? 0);
        //playerData.Init(10);
        
        player.data = playerData;
        player.OnUpdateHp(playerData);
    }
    public int GetPlayerHp()
    {
        return playerData.GetHp();
    }

    public void ResetStage()
    {
        player.data.InitAmorStat(0,0,0,0);
        player.RemoveAllTurnSkill();
        player.OnUpdateHp(playerData);
    }

    public void OnDispose()
    {
        ResetStage();
    }
}
