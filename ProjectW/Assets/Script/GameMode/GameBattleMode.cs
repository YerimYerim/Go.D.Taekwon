public class GameBattleMode : GameModeBase
{
    public GameBattleHandler BattleHandler  { get; private set; }
    
    public GameBattleMode(GameModeType modeType) : base(modeType)
    {
    }
    
    public override void Init()
    {
        base.Init();
        
        BattleHandler = new GameBattleHandler();
        BattleHandler.Init();
        
        
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}