using System.Collections.Generic;
public class AbnormalTableData
{
    public int? abnormal_id { get; set; }			//상태이상ID
    public EFFECT_TYPE? effect_type { get; set; }			//효과 타입
    public string abnormal_icon { get; set; }			//앱노멀 효과
    public string abnormal_bg { get; set; }			//앱노멀 배경
    public string abnormal_name { get; set; }			//앱노멀 이름
    public string abnormal_desc { get; set; }			//앱노멀 설명
}
