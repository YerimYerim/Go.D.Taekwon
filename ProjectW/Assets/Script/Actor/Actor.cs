using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public void SetActorName(string actorName)
    {
        gameObject.name = actorName;
    }
    
}
