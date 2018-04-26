using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Group;
using Wow.Tv.Middle.Biz.Group;

namespace Wow.Tv.Middle.WcfService.Group
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "GroupService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 GroupService.svc나 GroupService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class GroupService : IGroupService
    {
        public ListModel<NTB_GROUP> SearchList(GroupCondition condition)
        {
            return new GroupBiz().SearchList(condition);
        }


        public NTB_GROUP GetAt(int groupSeq)
        {
            return new GroupBiz().GetAt(groupSeq);
        }

        public int Save(NTB_GROUP model, LoginUser loginUser)
        {
            return new GroupBiz().Save(model, loginUser);
        }

        public void Delete(int groupSeq, LoginUser loginUser)
        {
            new GroupBiz().Delete(groupSeq, loginUser);
        }


        public void Copy(int groupSeq, LoginUser loginUser)
        {
            new GroupBiz().Copy(groupSeq, loginUser);
        }

        public List<CCC> B(int seq)
        {
            return new GroupBiz().B(seq);
        }

        public CCC BB(int seq)
        {
            return new GroupBiz().BB(seq);
        }

        public AAA BBB(int seq)
        {
            return new GroupBiz().BBB(seq);
        }

        public My BBBB(int seq)
        {
            return new GroupBiz().BBBB(seq);
        }

    }
}
