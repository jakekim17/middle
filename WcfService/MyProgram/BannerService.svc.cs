using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MyProgram;

using Wow.Tv.Middle.Biz.MyProgram;

namespace Wow.Tv.Middle.WcfService.MyProgram
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BannerService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BannerService.svc나 BannerService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class BannerService : IBannerService
    {
        public NTB_PROGRAM_BANNER GetAt(int programBannerSeq)
        {
            return new BannerBiz().GetAt(programBannerSeq);
        }

        public ListModel<NTB_PROGRAM_BANNER> SearchList(BannerCondition condition)
        {
            return new BannerBiz().SearchList(condition);
        }

        public int Save(NTB_PROGRAM_BANNER model, LoginUser loginUser)
        {
            return new BannerBiz().Save(model, loginUser);
        }

        public void Delete(int programBannerSeq)
        {
            new BannerBiz().Delete(programBannerSeq);
        }

        public void DeleteList(List<int> seqList)
        {
            new BannerBiz().DeleteList(seqList);
        }


    }
}
