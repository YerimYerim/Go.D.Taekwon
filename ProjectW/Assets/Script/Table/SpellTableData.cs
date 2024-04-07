using System.Collections.Generic;
public class SpellTableData
{
    public int? spell_id { get; set; }			//주문 ID
    public string spell_img { get; set; }			//주문 이미지
    public string spell_name { get; set; }			//주문 이름
    public string spell_desc { get; set; }			//주문 설명
    public int[] spell_effect { get; set; }			//효과 ID 연결
}