using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int chapterId;
    public List<int> curStageList;
    public List<int> enemyActor;
    public int stageId;
    public int mapId;
    public MAP_TYPE mapType;

    public MapData()
    {
    }

    public MapData(int mapId, int chapterId, List<int> curStageList, int stageId, List<int> enemyActor)
    {
        this.mapId = mapId;
        this.chapterId = chapterId;
        this.curStageList = curStageList;
        this.stageId = stageId;
        this.enemyActor = enemyActor;
    }
}
