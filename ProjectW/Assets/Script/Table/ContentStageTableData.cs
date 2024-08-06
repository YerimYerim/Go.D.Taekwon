using System.Collections.Generic;
public class ContentStageTableData
{
    public int? stage_id { get; set; }			//식별자
    public int[] map_group_1 { get; set; }			//등장할 랜덤 풀의 map_id 그룹
    public int? advent_cnt_1 { get; set; }			//map_group_1에서 선택지로 제시될 개수
    public int[] map_group_2 { get; set; }			//등장할 랜덤 풀의 map_id 그룹
    public int? advent_cnt_2 { get; set; }			//map_group_2에서 선택지로 제시될 개수
}
