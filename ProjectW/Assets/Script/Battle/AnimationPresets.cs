using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation_DB" , menuName = "Scriptable Object/AnimationPreset", order = int.MaxValue)]


public class AnimationPresets : ScriptableObject
{
    public List<AnimationPreset> presets;

}
public enum ANI_STATE
{
    IDLE,
}

public enum ACTOR_TAG
{
    PLAYER,
    Enemy1,
}

[Serializable]
public class AnimationPreset
{
    public Sprite[] _images;
    public ACTOR_TAG _actorTag;
    public ANI_STATE _state;
}
