public class GameBattleMode : GameModeBase
{
    public BattleActorSpawner BattleActorSpawner { get; private set; }
    public PlayerActorHandler PlayerActorHandler { get; private set; }
    public GameBattleHandler BattleHandler  { get; private set; }
    
    public MapHandler MapHandler { get; private set; }
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
        MapHandler ??= new MapHandler();
        MapHandler.Init();
        // 적로드
        
        // 액터 spawner 
        
        BattleActorSpawner ??= new BattleActorSpawner();
            
        // 플레이어 로드
        if(PlayerActorHandler == null)
        {
            PlayerActorHandler = new PlayerActorHandler();
            PlayerActorHandler.Init(GameUtil.PLAYER_ACTOR_ID);
        }

        //spell load 
        
        
        
        
        // 배틀 핸들러
        if(BattleHandler == null)
        {
            BattleHandler = new GameBattleHandler();
            BattleHandler.Init();
            MapHandler.SpawnActors();
        }

    }
    
    public override void Exit()
    {
        base.Exit();
    }
}