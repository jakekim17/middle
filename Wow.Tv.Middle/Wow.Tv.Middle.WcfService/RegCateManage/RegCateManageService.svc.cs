using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.RegCateManage;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.WcfService.RegCateManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "RegCateManageService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 RegCateManageService.svc나 RegCateManageService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class RegCateManageService : IRegCateManageService
    {
        /// <summary>
        /// 연간수입 리스트
        /// </summary>
        /// <returns></returns>
        public List<YearIncome> YearIncomeList()
        {
            return new YearIncomeBiz().YearIncomeList();
        }

        /// <summary>
        /// 연간수입 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<YearIncomeModifyResult> YearIncomeSave(List<YearIncome> list)
        {
            return new YearIncomeBiz().YearIncomeSave(list);
        }

        /// <summary>
        /// 투자선호대상 리스트
        /// </summary>
        /// <returns></returns>
        public List<IvstFavorObj> IvstFavorObjList()
        {
            return new IvstFavorObjBiz().IvstFavorObjList();
        }

        /// <summary>
        /// 투자선호대상 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IvstFavorObjModifyResult> IvstFavorObjSave(List<IvstFavorObj> list)
        {
            return new IvstFavorObjBiz().IvstFavorObjSave(list);
        }

        /// <summary>
        /// 기존정보 습득처 리스트
        /// </summary>
        /// <returns></returns>
        public List<OrgnInfoAcquirer> OrgnInfoAcquirerList()
        {
            return new OrgnInfoAcquirerBiz().OrgnInfoAcquirerList();
        }

        /// <summary>
        /// 기존정보 습득처 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<OrgnInfoAcquirerModifyResult> OrgnInfoAcquirerSave(List<OrgnInfoAcquirer> list)
        {
            return new OrgnInfoAcquirerBiz().OrgnInfoAcquirerSave(list);
        }

        /// <summary>
        /// 투자기간 리스트
        /// </summary>
        /// <returns></returns>
        public List<IvstProd> IvstProdList()
        {
            return new IvstProdBiz().IvstProdList();
        }

        /// <summary>
        /// 투자기간 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IvstProdModifyResult> IvstProdSave(List<IvstProd> list)
        {
            return new IvstProdBiz().IvstProdSave(list);
        }

        /// <summary>
        /// 투자성향 리스트
        /// </summary>
        /// <returns></returns>
        public List<InvstTendency> InvstTendencyList()
        {
            return new InvstTendencyBiz().InvstTendencyList();
        }

        /// <summary>
        /// 투자성향 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<InvstTendencyModifyResult> InvstTendencySave(List<InvstTendency> list)
        {
            return new InvstTendencyBiz().InvstTendencySave(list);
        }

        /// <summary>
        /// 주요증권거래처 리스트
        /// </summary>
        /// <returns></returns>
        public List<MainStockTrader> MainStockTraderList()
        {
            return new MainStockTraderBiz().MainStockTraderList();
        }

        /// <summary>
        /// 주요증권거래처 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<MainStockTraderModifyResult> MainStockTraderSave(List<MainStockTrader> list)
        {
            return new MainStockTraderBiz().MainStockTraderSave(list);
        }

        /// <summary>
        /// 투자규모 리스트
        /// </summary>
        /// <returns></returns>
        public List<IvstScale> IvstScaleList()
        {
            return new IvstScaleBiz().IvstScaleList();
        }

        /// <summary>
        /// 투자규모 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IvstScaleModifyResult> IvstScaleSave(List<IvstScale> list)
        {
            return new IvstScaleBiz().IvstScaleSave(list);
        }


        /// <summary>
        /// 관심분야 리스트
        /// </summary>
        /// <returns></returns>
        public List<FavorField> FavorFieldList()
        {
            return new FavorFieldBiz().FavorFieldList();
        }

        /// <summary>
        /// 관심분야 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<FavorFieldModifyResult> FavorFieldSave(List<FavorField> list)
        {
            return new FavorFieldBiz().FavorFieldSave(list);
        }
    }
}