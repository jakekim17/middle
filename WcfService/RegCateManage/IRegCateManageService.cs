 using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.WcfService.RegCateManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IRegCateManageService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IRegCateManageService
    {
        /// <summary>
        /// 연간수입 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<YearIncome> YearIncomeList();

        /// <summary>
        /// 연간수입 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<YearIncomeModifyResult> YearIncomeSave(List<YearIncome> list);

        /// <summary>
        /// 투자선호대상 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<IvstFavorObj> IvstFavorObjList();

        /// <summary>
        /// 투자선호대상 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<IvstFavorObjModifyResult> IvstFavorObjSave(List<IvstFavorObj> list);

        /// <summary>
        /// 기존정보 습득처 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<OrgnInfoAcquirer> OrgnInfoAcquirerList();

        /// <summary>
        /// 기존정보 습득처 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<OrgnInfoAcquirerModifyResult> OrgnInfoAcquirerSave(List<OrgnInfoAcquirer> list);

        /// <summary>
        /// 투자기간 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<IvstProd> IvstProdList();

        /// <summary>
        /// 투자기간 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<IvstProdModifyResult> IvstProdSave(List<IvstProd> list);

        /// <summary>
        /// 투자성향 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<InvstTendency> InvstTendencyList();

        /// <summary>
        /// 투자성향 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<InvstTendencyModifyResult> InvstTendencySave(List<InvstTendency> list);

        /// <summary>
        /// 주요증권거래처 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MainStockTrader> MainStockTraderList();

        /// <summary>
        /// 주요증권거래처 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<MainStockTraderModifyResult> MainStockTraderSave(List<MainStockTrader> list);

        /// <summary>
        /// 투자규모 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<IvstScale> IvstScaleList();

        /// <summary>
        /// 투자규모 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<IvstScaleModifyResult> IvstScaleSave(List<IvstScale> list);

        /// <summary>
        /// 관심분야 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<FavorField> FavorFieldList();

        /// <관심분야>
        /// 투자규모 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<FavorFieldModifyResult> FavorFieldSave(List<FavorField> list);
    }
}
