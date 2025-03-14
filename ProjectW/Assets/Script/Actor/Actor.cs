using System;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public bool isHidden = false;
    public bool isCanBeDamaged = true;
    
    private float _lifeTime = 0.0f;
    private float _creatTime = 0.0f;
    
    // damaged actor, damage, damage type, instigated by, damage causer
    Action<Actor, float> OnActorDamage;
    Action<Actor, float> OnActorTakeDamage;
    
    public void Init(Action<Actor, float> onActorDamage, Action<Actor, float> onActorTakeDamage)
    {
        _creatTime = Time.time;
    }
    
    public float GetLiftTime()
    {
        _lifeTime = Time.time - _creatTime;
        return _lifeTime;
    }
    
    public void SetActorName(string actorName)
    {
        gameObject.name = actorName;
    }
    
    public void SetIsCanBeDamaged(bool canBeDamaged)
    {
        isCanBeDamaged = canBeDamaged;
    }
    
    protected virtual float GetActualDamage(float damage)
    {
        return damage;
    }
    
    public virtual void TakeDamage(Actor actor, float damage)
    {
        if (isCanBeDamaged == false)
        {
            return;
        }
        var actualDamage = GetActualDamage(damage);
        OnActorTakeDamage?.Invoke(actor, actualDamage);
    }

    public virtual void Doamage(Actor actor, float damage)
    {
        if (isCanBeDamaged == false)
        {
            return;
        }
        OnActorDamage?.Invoke(actor, damage);
    }
}