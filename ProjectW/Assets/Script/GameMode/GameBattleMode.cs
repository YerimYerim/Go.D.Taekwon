public class GameBattleMode : GameModeBase
{
    public ActorSpawner ActorSpawner { get; private set; } = new();
    public PlayerActorHandler PlayerActorHandler { get; private set; } = new();
    public GameBattleHandler BattleHandler  { get; private set; } = new();
    public MapHandler MapHandler { get; private set; } = new();
    //public GameActormanager
    
    // 임시 엑터


    public GameBattleMode(GameModeType modeType) : base(modeType)
    {
    }
    
    public override void Init()
    {
        // 순서 중요
        base.Init();
       
        // 유저 정보 load 
        
        // 맵로드
        MapHandler.Init();
        // 적로드
        
        // 액터 spawner 
        
        // 액터 스폰


        // 플레이어 로드

        PlayerActorHandler.Init(GameUtil.PLAYER_ACTOR_ID);
        

        //spell load 
        
        
        
        
        // 배틀 핸들러

        BattleHandler.Init();
        ActorSpawner.SpawnEnemyActors(MapHandler.GetMapId());

    }
    
    public override void Exit()
    {
        base.Exit();
    }
}