using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "TvPlayerService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 TvPlayerService.svc나 TvPlayerService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class TvPlayerService : ITvPlayerService
    {
        /// <summary>
        /// 라이브 TV 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public LiveTvInfoModel LiveTvInfo(/*int userNumber*/)
        {
            return new TvPlayerBiz().LiveTvNowInfo(/*userNumber*/);
        }

        /// <summary>
        /// TV 다시보기 정보
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public TvReplayInfoModel TvReplayInfo(int num)
        {
            return new TvPlayerBiz().TvReplayInfo(num);
        }

        /// <summary>
        /// 증권영상 정보
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public VodInfoModel VodInfo(int num)
        {
            return new TvPlayerBiz().VodInfo(num);
        }
    }
}
