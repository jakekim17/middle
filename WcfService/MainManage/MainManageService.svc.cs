using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MainManage;
using Wow.Tv.Middle.Biz.MainManage;

namespace Wow.Tv.Middle.WcfService.MainManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MainManageService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MainManageService.svc나 MainManageService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MainManageService : IMainManageService
    {
        public ListModel<NTB_MAIN_MANAGE> SearchList(MainManageCondition condition)
        {
            return new MainManageBiz().SearchList(condition);
        }

        public NTB_MAIN_MANAGE GetAt(int mainManageSeq)
        {
            return new MainManageBiz().GetAt(mainManageSeq);
        }

        public int Save(NTB_MAIN_MANAGE model, LoginUser loginUser)
        {
            return new MainManageBiz().Save(model, loginUser);
        }

        public void Delete(int mainManageSeq)
        {
            new MainManageBiz().Delete(mainManageSeq);
        }
        
        public void UpDown(int mainManageSeq, bool isUp, LoginUser loginUser)
        {
            new MainManageBiz().UpDown(mainManageSeq, isUp, loginUser);
        }


        public ListModel<NTB_MAIN_MANAGE> GetFrontList(string mainTypeCode)
        {
            return new MainManageBiz().GetFrontList(mainTypeCode);
        }
    }
}
