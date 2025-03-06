using System.Collections.Generic;
public class SpellCombineTableData
{
    public int? spell_combine_id { get; set; }			//융합 id
    public int[] matarial { get; set; }			//재료
    public int? result_spell { get; set; }			//결과 스펠의 스펠 ID
}
