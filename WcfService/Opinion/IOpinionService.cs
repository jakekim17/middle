using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article.Opinion;

namespace Wow.Tv.Middle.WcfService.Opinion
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IOpinionService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IOpinionService
    {
        [OperationContract]
        ListModel<OpinionColumnModel> GetColumnList(OpinionCondition condition);

        [OperationContract]
        ListModel<NUP_NEWS_SECTION_SELECT_Result> GetDetailList(NewsCenterCondition condition, string text);

        [OperationContract]
        IQueryable<tblPlanArticle> GetPlanList(OpinionCondition condition);

        [OperationContract]
        tblPlanArticle ColumnBannerImg(OpinionCondition condition);
    }
}
