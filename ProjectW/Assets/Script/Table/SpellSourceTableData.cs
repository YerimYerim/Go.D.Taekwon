using System.Collections.Generic;
public class SpellSourceTableData
{
    public int? source_id { get; set; }			//근원 ID
    public int? product_ap { get; set; }			//생산 텀
    public int? product_value_init { get; set; }			//인게임 진입 시 생산량
    public int? product_value { get; set; }			//생산량
    public int? spell_id { get; set; }			//생산하는 원소
}
