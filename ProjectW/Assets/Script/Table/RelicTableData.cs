using System.Collections.Generic;
public class RelicTableData
{
    public int? relic_id { get; set; }			//어플리케이션 ID
    public RARITY? rarity { get; set; }			//등급
    public string relic_resource { get; set; }			//어플리케이션 이미지
    public string relic_name { get; set; }			//어플리케이션 이름
    public string relic_desc { get; set; }			//어플리케이션 설명
    public ACTIVE_CONDITION? active_condition_1 { get; set; }			//발동 조건 1
    public int? active_value_1 { get; set; }			//발동 조건 1 받는 값
    public ACTIVE_CONDITION? active_condition_2 { get; set; }			//발동 조건 2
    public int? active_value_2 { get; set; }			//발동 조건 2 받는 값
    public LOGICAL_OPERATOR? condition_logic { get; set; }			//발동 조건 1, 2의 처리 조건 (AND, OR)
    public TARGET_TYPE? target_1 { get; set; }			//적용대상
    public int? target_count_1 { get; set; }			//적용대상 개수
    public RELIC_EFFECT? relic_effect_1 { get; set; }			//효과 1
    public int? effect_value_1 { get; set; }			//효과 1 관련 값
    public TARGET_TYPE? target_2 { get; set; }			//적용대상
    public int? target_count_2 { get; set; }			//적용대상 개수
    public RELIC_EFFECT? relic_effect_2 { get; set; }			//효과 2
    public int? effect_value_2 { get; set; }			//효과 2 관련 값
    public TARGET_TYPE? target_3 { get; set; }			//적용대상
    public int? target_count_3 { get; set; }			//적용대상 개수
    public RELIC_EFFECT? relic_effect_3 { get; set; }			//효과 3
    public int? effect_value_3 { get; set; }			//효과 3 관련 값
}