using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameSpriteAni : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ACTOR_TAG _actorTag;
    [SerializeField] private bool isLoop;

    private Dictionary<ANI_STATE, Sprite[]> anim = new();
    private ANI_STATE _curState = ANI_STATE.IDLE;

    private int _curFrame;
    private int _frameCount;
    private float _timePerFrame;
    
    private Coroutine _coroutine;
    private WaitForSeconds _seconds;
    private void Awake()
    {
        anim = GameAniManager.Instance.GetActorAnimation(_actorTag);
        StartAnimation(_curState);
    }

    public void StartAnimation(ANI_STATE aniState)
    {
        _curState = aniState;
        _frameCount = anim[aniState].Length;
        _timePerFrame = 1.0f / _frameCount;
        _curFrame = 0;
        _seconds = new WaitForSeconds(_timePerFrame);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(CoroutineAni());
    }
    
    IEnumerator CoroutineAni()
    {
        while (isLoop)
        {
            if (_curFrame >= _frameCount)
                _curFrame = 0;
            
            _spriteRenderer.sprite = anim[_curState][_curFrame];
            ++_curFrame;
            yield return _seconds;
        }
    }
}
