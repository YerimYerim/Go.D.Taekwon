using System.Collections.Generic;
public class ContentMapTableData
{
    public int? map_id { get; set; }			//식별자
    public MAP_TYPE? map_type { get; set; }			//맵 타입
    public int[] actor_id { get; set; }			//등장 액터 ID
    public string map_bg { get; set; }			//맵 배경 리소스
    public string map_img { get; set; }			//맵 대표 이미지 - 선택지 표시 용
    public string map_name { get; set; }			//맵 이름 - 선택지 표시 용
    public string map_desc { get; set; }			//맵 설명 - 선택지 표시 용
}
