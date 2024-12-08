using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// remain 타임을 stack 으로 사용하여 한번발동시마다 하나씩깎는 방식
/// </summary>
public interface ISkillStackable
{
    public abstract void RemoveStack();
    public abstract void DoStackEffect(GameActor target);
}
