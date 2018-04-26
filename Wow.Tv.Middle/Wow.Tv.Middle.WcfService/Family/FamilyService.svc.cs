using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Family;

using Wow.Tv.Middle.Biz.Family;
using System.IO;

namespace Wow.Tv.Middle.WcfService.Family
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "FamilyService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 FamilyService.svc나 FamilyService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class FamilyService : IFamilyService
    {
        public ListModel<NTB_FAMILY> SearchList(FamilyCondition condition)
        {
            return new FamilyBiz().SearchList(condition);
        }

        public NTB_FAMILY GetAt(int familySeq)
        {
            return new FamilyBiz().GetAt(familySeq);
        }

        public int Save(NTB_FAMILY model, LoginUser loginUser)
        {
            return new FamilyBiz().Save(model, loginUser);
        }

        public void Delete(int familySeq, LoginUser loginUser)
        {
            new FamilyBiz().Delete(familySeq, loginUser);
        }

        public void DeleteList(List<int> seqList, LoginUser loginUser)
        {
            new FamilyBiz().DeleteList(seqList, loginUser);
        }

        public void UpDown(int familySeq, bool isUp, LoginUser loginUser)
        {
            new FamilyBiz().UpDown(familySeq, isUp, loginUser);
        }

        public MemoryStream ExcelExport(FamilyCondition condition)
        {
            return new FamilyBiz().ExcelExport(condition);
        }

    }
}
