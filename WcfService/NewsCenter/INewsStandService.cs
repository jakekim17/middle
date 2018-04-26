using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "INewsStandService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface INewsStandService
    {

        /// <summary>
        /// 뉴스 스탠드 해드라인 리스트 SP
        /// </summary>
        /// <returns>ListModel<usp_newsStandMetaXMLTopImg_Result></returns>
        [OperationContract]
        ListModel<usp_newsStandMetaXMLTopImg_Result> GetNewsStandHeadLine();

        /// <summary>
        /// 뉴스 스탠드 핫 키워드 SP
        /// </summary>
        /// <returns>ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result> GetNewsStandHotKeyword();
        
    }
}
