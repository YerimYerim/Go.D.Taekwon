using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillOnHeal 
{
    public abstract void OnHeal(int heal, int overHeal);
}