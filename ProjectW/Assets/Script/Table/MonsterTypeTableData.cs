using System.Collections.Generic;
public class MonsterTypeTableData
{
    public  MONSTER_TYPE? monster_type { get; set; }			//몬스터 타입
    public int? level { get; set; }			//레벨
    public EFFECT_TAG? effect_tag { get; set; }			//효과 태그
    public float scale_factor_1 { get; set; }			//a
    public float scale_factor_2 { get; set; }			//b
}
