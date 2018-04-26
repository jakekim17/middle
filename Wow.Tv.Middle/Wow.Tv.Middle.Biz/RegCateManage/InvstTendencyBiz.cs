using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class InvstTendencyBiz : BaseBiz
    {
        /// <summary>
        /// 투자성향 리스트
        /// </summary>
        /// <returns></returns>
        public List<InvstTendency> InvstTendencyList()
        {
            List<tblCodeInvestmentPropensity> codeList = (from tbl in db89_wowbill.tblCodeInvestmentPropensity select tbl).ToList();

            List<tblCodeInvestmentPropensityDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeInvestmentPropensityDetail select tbl).ToList();

            List<InvstTendency> retval = (from tblA in codeList
                                     join tblB in codeDetailList on tblA.investmentPropensityId equals tblB.investmentPropensityId into _c
                                     from c in _c.DefaultIfEmpty()
                                     orderby (c != null ? c.sort : 0) ascending, tblA.investmentPropensityId ascending
                                     select new InvstTendency()
                                     {
                                         InvestmentPropensityId = tblA.investmentPropensityId,
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
        /// 투자성향 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<InvstTendencyModifyResult> InvstTendencySave(List<InvstTendency> list)
        {
            // 처리결과 저장 리스트
            List<InvstTendencyModifyResult> retval = new List<InvstTendencyModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (InvstTendency item in list)
            {
                // 처리결과 저장 아이템
                InvstTendencyModifyResult retvalItem = new InvstTendencyModifyResult();
                retvalItem.UserChagned = false;

                if (item.InvestmentPropensityId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeInvestmentPropensity orderby p.investmentPropensityId descending select p.investmentPropensityId).FirstOrDefault();
                    newId++;

                    tblCodeInvestmentPropensity newItem = new tblCodeInvestmentPropensity();
                    newItem.investmentPropensityId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeInvestmentPropensity.Add(newItem);

                    tblCodeInvestmentPropensityDetail newItemDetail = new tblCodeInvestmentPropensityDetail();
                    newItemDetail.investmentPropensityId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeInvestmentPropensityDetail.Add(newItemDetail);

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
                else if (item.InvestmentPropensityId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.InvestmentPropensityId = item.InvestmentPropensityId;
                    retvalItem.Descript = item.Descript;

                    tblCodeInvestmentPropensity dbItem = db89_wowbill.tblCodeInvestmentPropensity.Where(a => a.investmentPropensityId == item.InvestmentPropensityId).SingleOrDefault();
                    tblCodeInvestmentPropensityDetail dbItemDetail = db89_wowbill.tblCodeInvestmentPropensityDetail.Where(a => a.investmentPropensityId == item.InvestmentPropensityId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeInvestmentPropensityDetail();
                        dbItemDetail.investmentPropensityId = item.InvestmentPropensityId.Value;
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
                            db89_wowbill.tblCodeInvestmentPropensityDetail.Add(dbItemDetail);
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
                else if (item.InvestmentPropensityId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.InvestmentPropensityId = item.InvestmentPropensityId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.investmentPropensityId == item.InvestmentPropensityId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeInvestmentPropensity.Where(a => a.investmentPropensityId == item.InvestmentPropensityId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentPropensity.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeInvestmentPropensityDetail.Where(a => a.investmentPropensityId == item.InvestmentPropensityId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentPropensityDetail.Remove(deleteItemDetail);

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
