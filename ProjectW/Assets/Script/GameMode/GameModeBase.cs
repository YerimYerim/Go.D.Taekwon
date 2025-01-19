using System.Collections.Generic;
using Script.Manager;
using UnityEngine;


/// <summary>
/// 게임 모드의 기본 클래스 -ex) 상점 모드, 전투 모드, 스토리 진행모드 등등
/// 한 화면에는 한 모드만 존재해야함.
/// 
/// </summary>
public enum GameModeType
{
    None,
    Shop,
    Battle,
    Story
}

public class GameModeBase : IGameModeBase
{
    public GameModeType ModeType { get; private set;}
    
    // 필요한 해당 모드에서 관리 필요한 Handler 들을 추가해준다.
    
    public GameModeBase(GameModeType modeType)
    {
        ModeType = modeType;
    }

    public virtual void Init()
    {
        
    }
    /// <summary>
    /// 각종 핸들러 DisPose
    /// </summary>
    public virtual void Exit()
    {
       
    }
}

public interface IGameModeBase
{
    public abstract void Init();
    public abstract void Exit();
}