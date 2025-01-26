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
    public int[] relic_effect { get; set; }			//효과 (spell_effect의 effect_id 참조)
}
