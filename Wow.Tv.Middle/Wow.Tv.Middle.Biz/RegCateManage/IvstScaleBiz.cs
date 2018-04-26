using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class IvstScaleBiz : BaseBiz
    {
        /// <summary>
        /// 투자규모 리스트
        /// </summary>
        /// <returns></returns>
        public List<IvstScale> IvstScaleList()
        {
            List<tblCodeInvestmentScale> codeList = (from tbl in db89_wowbill.tblCodeInvestmentScale select tbl).ToList();

            List<tblCodeInvestmentScaleDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeInvestmentScaleDetail select tbl).ToList();

            List<IvstScale> retval = (from tblA in codeList
                                     join tblB in codeDetailList on tblA.investmentScaleId equals tblB.investmentScaleId into _c
                                     from c in _c.DefaultIfEmpty()
                                     orderby (c != null ? c.sort : 0) ascending, tblA.investmentScaleId ascending
                                     select new IvstScale()
                                     {
                                         InvestmentScaleId = tblA.investmentScaleId,
                                         Descript = tblA.descript,
                                         RegistDate = tblA.registDt,
                                         UpdateDate = tblA.updateDt,
                                         AdminId = tblA.adminId,
                                         Apply = (tblA.apply != null ? tblA.apply.Value : false),
                                         Sort = (c != null ? c.sort : (byte)0)
                                     }).OrderBy(a => a.Sort).ToList();
            return retval;
        }

        /// <summary>
        /// 투자규모 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IvstScaleModifyResult> IvstScaleSave(List<IvstScale> list)
        {
            // 처리결과 저장 리스트
            List<IvstScaleModifyResult> retval = new List<IvstScaleModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (IvstScale item in list)
            {
                // 처리결과 저장 아이템
                IvstScaleModifyResult retvalItem = new IvstScaleModifyResult();
                retvalItem.UserChagned = false;

                if (item.InvestmentScaleId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeInvestmentScale orderby p.investmentScaleId descending select p.investmentScaleId).FirstOrDefault();
                    newId++;

                    tblCodeInvestmentScale newItem = new tblCodeInvestmentScale();
                    newItem.investmentScaleId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeInvestmentScale.Add(newItem);

                    tblCodeInvestmentScaleDetail newItemDetail = new tblCodeInvestmentScaleDetail();
                    newItemDetail.investmentScaleId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeInvestmentScaleDetail.Add(newItemDetail);

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
                else if (item.InvestmentScaleId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.InvestmentScaleId = item.InvestmentScaleId;
                    retvalItem.Descript = item.Descript;

                    tblCodeInvestmentScale dbItem = db89_wowbill.tblCodeInvestmentScale.Where(a => a.investmentScaleId == item.InvestmentScaleId).SingleOrDefault();
                    tblCodeInvestmentScaleDetail dbItemDetail = db89_wowbill.tblCodeInvestmentScaleDetail.Where(a => a.investmentScaleId == item.InvestmentScaleId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeInvestmentScaleDetail();
                        dbItemDetail.investmentScaleId = item.InvestmentScaleId.Value;
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
                            db89_wowbill.tblCodeInvestmentScaleDetail.Add(dbItemDetail);
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
                else if (item.InvestmentScaleId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.InvestmentScaleId = item.InvestmentScaleId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.investmentScaleId == item.InvestmentScaleId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeInvestmentScale.Where(a => a.investmentScaleId == item.InvestmentScaleId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentScale.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeInvestmentScaleDetail.Where(a => a.investmentScaleId == item.InvestmentScaleId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentScaleDetail.Remove(deleteItemDetail);

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
