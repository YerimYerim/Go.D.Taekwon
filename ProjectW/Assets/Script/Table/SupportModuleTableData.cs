using System.Collections.Generic;
public class SupportModuleTableData
{
    public int? support_module_id { get; set; }			//식별자
    public int? source_id { get; set; }			//속성값(근원 ID) spell_source 참조
    public int? level { get; set; }			//레벨
    public SUPPORT_MODULE_EFFECT? support_module_effect { get; set; }			//효과
    public int? effect_value { get; set; }			//효과 관련 값
    public string support_module_img { get; set; }			//서포트 모듈 포트레이트
    public string support_module_name { get; set; }			//서포트 모듈 이름
    public string support_module_desc { get; set; }			//서포트 모듈 설명
}
