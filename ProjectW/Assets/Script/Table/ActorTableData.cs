using System.Collections.Generic;
public class ActorTableData
{
    public int? actor_id { get; set; }			//액터 ID
    public ACTOR_TYPE? actor_type { get; set; }			//주문 타입
    public string actor_resource { get; set; }			//주문 이미지
    public int? actor_stat_hp { get; set; }			//액터 체력
}
