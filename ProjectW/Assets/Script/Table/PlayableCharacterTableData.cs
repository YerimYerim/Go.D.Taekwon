using System.Collections.Generic;
public class PlayableCharacterTableData
{
    public int? actor_id { get; set; }			//액터 ID
    public int? stat_hp { get; set; }			//체력 스탯
    public int? first_reward_id { get; set; }			//리워드 ID
    public int? slot { get; set; }			//근원 슬롯 초기값
    public int? slot_max { get; set; }			//근원 슬롯 최대값
}
