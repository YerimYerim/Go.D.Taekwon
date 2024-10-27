using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewardBase
{
    private int _rewardID;
    private int _index;
    protected int _count;
    protected int _itemID;


    protected RewardBase(RewardTableData reward)
    {
        _rewardID = reward?.reward_id ?? 0;
        _index = reward?.index ?? 0;
        _count = Random.Range(reward?.cnt_min ?? 0, reward?.cnt_max ?? 0 + 1);
        _itemID = reward?.content_id ?? 0;
    }
    
}
