public class GameBattleMode : GameModeBase
{
    public BattleActorSpawner BattleActorSpawner { get; private set; }
    public PlayerActorHandler PlayerActorHandler { get; private set; }
    public GameBattleHandler BattleHandler  { get; private set; }
    //public GameActormanager
    
    // 임시 엑터


    public GameBattleMode(GameModeType modeType) : base(modeType)
    {
    }
    
    public override void Init()
    {
        base.Init();
       
        // 유저 정보 load 
        
        // 맵로드
        
        // 적로드
        
        // 액터 spawner 
        BattleActorSpawner = new BattleActorSpawner();
            
        // 플레이어 로드
        PlayerActorHandler = new PlayerActorHandler();
        PlayerActorHandler.Init(GameUtil.PLAYER_ACTOR_ID);
        
        
        //spell load 
        
        
        
        
        // 배틀 핸들러
        BattleHandler = new GameBattleHandler();
        BattleHandler.Init();
        
        
        
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}