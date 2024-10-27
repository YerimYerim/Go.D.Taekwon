using Script.Manager;


/// <summary>
/// 게임 시작시 생성되는 인스턴스들을 관리하는 클래스
/// </summary>
public class GameInstanceManager : Singleton<GameInstanceManager>
{
    GameModeBase currentGameMode;
    protected override void Awake()
    {
        base.Awake();
        GameTableManager.Instance.LoadData();
        GameRewardManager.Instance.Init();
        GameDataManager.Instance.Init();
    }

    public void BattleStart()
    {
        ChangeGameMode(GameModeType.Battle);
        
    }
    public void ChangeGameMode(GameModeType modeType)
    {
        currentGameMode?.Exit();

        switch (modeType)
        {
            case GameModeType.None:
                break;
            case GameModeType.Shop:
                break;
            case GameModeType.Battle:
                currentGameMode = new GameBattleMode(modeType);
                break;
            case GameModeType.Story:
                break;
        }
        
        currentGameMode?.Init();
    }
    
    public T GetGameMode<T>() where T : GameModeBase
    {
        return currentGameMode as T;
    }
}
