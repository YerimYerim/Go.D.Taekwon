using System.Collections.Generic;
public class RewardTableData
{
    public int? reward_id { get; set; }			//보상 ID
    public int? index { get; set; }			//row index
    public REWARD_TYPE? reward_type { get; set; }			//지급할 보상 타입
    public int? content_id { get; set; }			//보상 타입에 따른 ID
    public int? cnt_min { get; set; }			//지급 최소값
    public int? cnt_max { get; set; }			//지급 최대값
    public int? prob_weight { get; set; }			//확률 가중치
    public EXCEPTION_TYPE? exception_type { get; set; }			//보상 포함 조건 (null이면 무조건 포함)
    public int[] exception_value { get; set; }			//보상 조건 관련 값
}
