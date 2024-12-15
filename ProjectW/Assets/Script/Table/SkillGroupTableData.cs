using System.Collections.Generic;
public class SkillGroupTableData
{
    public int? skill_group_id { get; set; }			//스킬 그룹 ID
    public int[] effect_id { get; set; }			//spell_effect ID 참조
    public int? action_point { get; set; }			//스킬 사용 기준 AP
    public SKIP_CONDITION? skip_condition { get; set; }			//해당 스킬 스킵 조건
    public float? skip_condition_value { get; set; }			//조건값
}
