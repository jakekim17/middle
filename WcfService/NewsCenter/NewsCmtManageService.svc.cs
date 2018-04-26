using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article.NewsCmt;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "NewsCmtManage"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 NewsCmtManage.svc나 NewsCmtManage.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class NewsCmtManage : INewsCmtManageService
    {
        
        public NewsCmdCodeModel<NewsCmtManageModel> GetList(NewsCmtCondition condition)
        {
            return new NewsCmtBiz().GetList(condition);
        }

        public void Delete(int[] seq)
        {
            new NewsCmtBiz().Delete(seq);
        }

        public void Update(String seq)
        {
            new NewsCmtBiz().Update(seq);
        }

        public ListModel<NTB_ARTICLE_COMMENT> GetCommentList(CommentCondition condition)
        {
            return new NewsCmtBiz().GetCommentList(condition);
        }

        public void SaveComment(NTB_ARTICLE_COMMENT model, LoginUserInfo loginUserInfo)
        {
            new NewsCmtBiz().SaveComment(model, loginUserInfo);
        }

        public void DeleteComment(int deleteId)
        {
            new NewsCmtBiz().DeleteComment(deleteId);
        }
    }
}
