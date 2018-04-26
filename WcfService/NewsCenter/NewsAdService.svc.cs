using System;
using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "NewsAdService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 NewsAdService.svc나 NewsAdService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class NewsAdService : INewsAdService
    {


        #region 하드 코딩 광고       
        /// <summary>
        /// 하드 코딩 광고 리스트
        /// </summary>
        /// <returns>ListModel<NTB_HARD_CODING_AD></returns>
        public ListModel<NTB_HARD_CODING_AD> GetHardCodingAdList()
        {
            return new NewsAdBiz().GetHardCodingAdList();
        }

        /// <summary>
        /// 하드 코딩 광고 정보
        /// </summary>
        /// <param name="SEQ">일렬번호</param>
        /// <returns>하드 코딩 광고 내용</returns>
        public NTB_HARD_CODING_AD GetHardCodingAdInfo(int SEQ)
        {
            return new NewsAdBiz().GetHardCodingAdInfo(SEQ);
        }

        /// <summary>
        /// 하드 코딩 광고 등록,수정
        /// </summary>
        /// <param name="hardCodingAdInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        public bool SetHardCodingAd(NTB_HARD_CODING_AD hardCodingAdInfo, LoginUser loginUser)
        {
            return new NewsAdBiz().SetHardCodingAd(hardCodingAdInfo, loginUser);
        }
        #endregion


    }
}
