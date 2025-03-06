using System.Collections.Generic;
public class FirstRewardTableData
{
    public int? first_reward_id { get; set; }			//리워드 ID
    public REWARD_TYPE? reward_type { get; set; }			//리워드 타입
    public int? content_id { get; set; }			//지급할 근원 ID
    public int? amount { get; set; }			//수량
}
