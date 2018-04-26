using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;

using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BroadLiveService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BroadLiveService.svc나 BroadLiveService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public partial class BroadService : IBroadLiveService
    {

        public NTB_BROAD_LIVE GetAtLive(int broadLiveID)
        {
            return new BroadLiveBiz().GetAt(broadLiveID);
        }

        public ListModel<NTB_BROAD_LIVE> SearchListLive(BroadLiveCondition condition)
        {
            return new BroadLiveBiz().SearchList(condition);
        }

        public int SaveLive(NTB_BROAD_LIVE model, LoginUser loginUser)
        {
            return new BroadLiveBiz().Save(model, loginUser);
        }

        public void DeleteLive(int broadLiveID)
        {
            new BroadLiveBiz().Delete(broadLiveID);
        }

        public void DeleteLiveList(List<int> seqList)
        {
            new BroadLiveBiz().DeleteLiveList(seqList);
        }
    }
}
