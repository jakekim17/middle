using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class MainStockTraderBiz : BaseBiz
    {
        /// <summary>
        /// 주요증권거래처 리스트
        /// </summary>
        /// <returns></returns>
        public List<MainStockTrader> MainStockTraderList()
        {
            List<tblCodeStockCompany> codeList = (from tbl in db89_wowbill.tblCodeStockCompany select tbl).ToList();

            List<tblCodeStockCompanyDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeStockCompanyDetail select tbl).ToList();

            List<MainStockTrader> retval = (from tblA in codeList
                                     join tblB in codeDetailList on tblA.stockCompanyId equals tblB.stockCompanyId into _c
                                     from c in _c.DefaultIfEmpty()
                                     orderby (c != null ? c.sort : 0) ascending, tblA.stockCompanyId ascending
                                     select new MainStockTrader()
                                     {
                                         StockCompanyId = tblA.stockCompanyId,
                                         Descript = tblA.descript,
                                         RegistDate = tblA.registDt,
                                         UpdateDate = tblA.updateDt,
                                         AdminId = tblA.adminId,
                                         Apply = tblA.apply,
                                         Sort = (c != null ? c.sort : (byte)0)
                                     }).OrderBy(a => a.Sort).ToList();
            return retval;
        }

        /// <summary>
        /// 주요증권거래처 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<MainStockTraderModifyResult> MainStockTraderSave(List<MainStockTrader> list)
        {
            // 처리결과 저장 리스트
            List<MainStockTraderModifyResult> retval = new List<MainStockTraderModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (MainStockTrader item in list)
            {
                // 처리결과 저장 아이템
                MainStockTraderModifyResult retvalItem = new MainStockTraderModifyResult();
                retvalItem.UserChagned = false;

                if (item.StockCompanyId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeStockCompany orderby p.stockCompanyId descending select p.stockCompanyId).FirstOrDefault();
                    newId++;

                    tblCodeStockCompany newItem = new tblCodeStockCompany();
                    newItem.stockCompanyId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeStockCompany.Add(newItem);

                    tblCodeStockCompanyDetail newItemDetail = new tblCodeStockCompanyDetail();
                    newItemDetail.stockCompanyId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeStockCompanyDetail.Add(newItemDetail);

                    try
                    {
                        db89_wowbill.SaveChanges();

                        retvalItem.IsSuccess = true;
                        retvalItem.ReturnMessage = "";
                    }
                    catch (Exception ex)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = ex.Message;
                    }
                }
                else if (item.StockCompanyId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.StockCompanyId = item.StockCompanyId;
                    retvalItem.Descript = item.Descript;

                    tblCodeStockCompany dbItem = db89_wowbill.tblCodeStockCompany.Where(a => a.stockCompanyId == item.StockCompanyId).SingleOrDefault();
                    tblCodeStockCompanyDetail dbItemDetail = db89_wowbill.tblCodeStockCompanyDetail.Where(a => a.stockCompanyId == item.StockCompanyId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeStockCompanyDetail();
                        dbItemDetail.stockCompanyId = item.StockCompanyId.Value;
                    }

                    if (item.Apply != dbItem.apply || item.Descript != dbItem.descript || item.Sort != dbItemDetail.sort || dbItemDetailAdded == true)
                    {
                        retvalItem.UserChagned = true;

                        dbItem.apply = item.Apply;
                        dbItem.descript = item.Descript;
                        dbItem.updateDt = now;

                        dbItemDetail.sort = item.Sort;
                        if (dbItemDetailAdded == true)
                        {
                            db89_wowbill.tblCodeStockCompanyDetail.Add(dbItemDetail);
                        }

                        try
                        {
                            db89_wowbill.SaveChanges();

                            retvalItem.IsSuccess = true;
                            retvalItem.ReturnMessage = "";
                        }
                        catch (Exception ex)
                        {
                            retvalItem.IsSuccess = false;
                            retvalItem.ReturnMessage = ex.Message;
                        }
                    }
                    else
                    {
                        retvalItem.IsSuccess = true;
                        retvalItem.ReturnMessage = "";
                    }
                }
                else if (item.StockCompanyId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.StockCompanyId = item.StockCompanyId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.stockCompanyId == item.StockCompanyId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeStockCompany.Where(a => a.stockCompanyId == item.StockCompanyId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeStockCompany.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeStockCompanyDetail.Where(a => a.stockCompanyId == item.StockCompanyId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeStockCompanyDetail.Remove(deleteItemDetail);

                        try
                        {
                            db89_wowbill.SaveChanges();

                            retvalItem.IsSuccess = true;
                            retvalItem.ReturnMessage = "";
                        }
                        catch (Exception ex)
                        {
                            retvalItem.IsSuccess = false;
                            retvalItem.ReturnMessage = ex.Message;
                        }
                    }
                }

                retval.Add(retvalItem);
            }

            return retval;
        }
    }
}
