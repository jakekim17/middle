using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "INewsAdService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface INewsAdService
    {
        
        #region 하드 코딩 광고
        /// <summary>
        /// 하드 코딩 광고 리스트
        /// </summary>
        /// <returns>ListModel<NTB_HARD_CODING_AD></returns>
        [OperationContract]
        ListModel<NTB_HARD_CODING_AD> GetHardCodingAdList();

        /// <summary>
        /// 하드 코딩 광고 정보
        /// </summary>
        /// <param name="SEQ">일렬번호</param>
        /// <returns>하드 코딩 광고 내용</returns>
        [OperationContract]
        NTB_HARD_CODING_AD GetHardCodingAdInfo(int SEQ);

        /// <summary>
        /// 하드 코딩 광고 등록,수정
        /// </summary>
        /// <param name="hardCodingAdInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetHardCodingAd(NTB_HARD_CODING_AD hardCodingAdInfo, LoginUser loginUser);
        #endregion
     
        

    }
}
