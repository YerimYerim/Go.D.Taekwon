using System.Collections.Generic;
public class DialogTableData
{
    public int? dialog_id { get; set; }			//다이얼로그 ID (그룹 형)
    public int? index { get; set; }			//그룹 내 index
    public DIALOG_TYPE? dialog_type { get; set; }			//다이얼로그 구분 타입 : 타이틀, 내용, 버튼 등
    public int? button_id { get; set; }			//dialog_type = button에만 사용 dialog_button_id 참조
    public string dialog_string { get; set; }			//string 테이블의 key 참조
}
