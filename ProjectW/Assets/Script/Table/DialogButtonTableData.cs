using System.Collections.Generic;
public class DialogButtonTableData
{
    public int? button_id { get; set; }			//버튼 ID
    public LOGICAL_OPERATOR? function_condition { get; set; }			//AND, OR 구분자
    public BUTTON_FUNCTION? function_type_1 { get; set; }			//버튼 기능 1
    public int? value_1 { get; set; }			//기능 1 관련 input 값
    public BUTTON_FUNCTION? function_type_2 { get; set; }			//버튼 기능 2
    public int? value_2 { get; set; }			//기능 2 관련 input 값
    public int[] function_prob { get; set; }			//기능 1, 2가 OR일 때 확률 (AND면 null)
}
