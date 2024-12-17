using System.Collections.Generic;
public class ContentSubShopTableData
{
    public int? shop_id { get; set; }			//상점 ID (그룹 형)
    public int? index { get; set; }			//그룹 내 index
    public ITEM_TYPE? item_type { get; set; }			//근원, 서포트 모듈, 어플리케이션 구분자
    public int? item_id { get; set; }			//item_type의 ID
    public int? cost { get; set; }			//상품 비용(골드 차감)
}
