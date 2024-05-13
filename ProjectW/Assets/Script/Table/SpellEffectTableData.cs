using System.Collections.Generic;
public class SpellEffectTableData
{
    public int? effect_id { get; set; }			//효과 id
    public EFFECT_TYPE? effect_type { get; set; }			//효과 타입
    public int? value_1 { get; set; }			//효과 관련 값
    public int? remain_turn_count { get; set; }			//지속 턴
    public TARGET_TYPE? target { get; set; }			//적용대상
    public int? target_count { get; set; }			//적용대상 개수
}
