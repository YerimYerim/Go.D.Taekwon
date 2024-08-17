using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEvent
{
    public event Action EventAction;
    public float delay;
    
    public void DoEvent()
    {
        EventAction?.Invoke();
    }
    public void AddEvent(Action action, float delay)
    {
        EventAction += action;
        this.delay = delay;
    }
    
    public void RemoveEvent(Action action)
    {
        EventAction -= action;
    }
    
    public void ClearEvent()
    {
        EventAction = null;
        delay = 0;
    }
}
