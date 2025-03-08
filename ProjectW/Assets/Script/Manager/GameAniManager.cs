using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GameAniManager : Singleton<GameAniManager>
{
    
    private Dictionary<ACTOR_TAG, Dictionary<ANI_STATE, Sprite[]>> actorTagTree = new();

    protected override void Awake()
    {
        base.Awake();
        var  loadScriptableObject = ResourceImporter.GetLoadScriptableObject<AnimationPresets>("Animation_DB");
        
        for (int i = 0; i < loadScriptableObject.presets.Count; ++i)
        {
            var preset = loadScriptableObject.presets[i];
            var dic = new Dictionary<ANI_STATE, Sprite[]> {{preset._state, preset._images}};
            actorTagTree.Add(preset._actorTag, dic);
        }
    }

    public Dictionary<ANI_STATE, Sprite[]> GetActorAnimation(ACTOR_TAG actorTag)
    {
        return actorTagTree.GetValueOrDefault(actorTag);
    }
}
