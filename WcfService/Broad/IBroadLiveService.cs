using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;


namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IBroadLiveService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IBroadLiveService
    {
        [OperationContract]
        NTB_BROAD_LIVE GetAtLive(int broadLiveID);

        [OperationContract]
        ListModel<NTB_BROAD_LIVE> SearchListLive(BroadLiveCondition condition);

        [OperationContract]
        int SaveLive(NTB_BROAD_LIVE model, LoginUser loginUser);

        [OperationContract]
        void DeleteLive(int broadLiveID);

        [OperationContract]
        void DeleteLiveList(List<int> seqList);
    }
}
