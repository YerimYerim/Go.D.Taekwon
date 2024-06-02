using System.Collections.Generic;
public class ActorTableData
{
    public int? actor_id { get; set; }			//액터 ID
    public ACTOR_TYPE? actor_type { get; set; }			//액터 타입
    public int? actor_function_value { get; set; }			//액터 기능 관련 값
    public string actor_name { get; set; }			//액터 이름
    public int? rsc_id { get; set; }			//리소스 ID
}
