using System.Collections.Generic;
public class SkillPatternGroupTableData
{
    public int? skill_pattern_group_id { get; set; }			//스킬 패턴 그룹 ID
    public int? phase { get; set; }			//페이즈
    public PHASE_CONDITION? phase_condition { get; set; }			//다음 페이즈로 넘어갈 조건
    public float? phase_condition_value { get; set; }			//조건 값
    public PATTERN_TYPE? pattern_type { get; set; }			//스킬 패턴 타입
    public int? skill_group { get; set; }			//스킬 그룹
}
