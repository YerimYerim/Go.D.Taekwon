using System.Collections.Generic;
public class MonsterTableData
{
    public int? actor_id { get; set; }			//액터 ID
    public int? level { get; set; }			//레벨
    public int? stat_hp { get; set; }			//체력 팩터
    public MONSTER_TYPE? monster_type { get; set; }			//
    public int? skill_pattern_group { get; set; }			//스킬 패턴 그룹
}
