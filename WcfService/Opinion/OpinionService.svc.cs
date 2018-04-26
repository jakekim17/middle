using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Opinion;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article.Opinion;

namespace Wow.Tv.Middle.WcfService.Opinion
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "OpinionService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 OpinionService.svc나 OpinionService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class OpinionService : IOpinionService
    {
        public tblPlanArticle ColumnBannerImg(OpinionCondition condition)
        {
            return new OpinionBiz().ColumnBannerImg(condition);
        }

        public ListModel<OpinionColumnModel> GetColumnList(OpinionCondition condition)
        {
            return new OpinionBiz().GetColumnList(condition);
        }


        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetDetailList(NewsCenterCondition condition, string text)
        {
            return new OpinionBiz().GetDetailList(condition, text);
        }

        public IQueryable<tblPlanArticle> GetPlanList(OpinionCondition condition)
        {
            return new OpinionBiz().GetPlanList(condition);
        }
    }
}
