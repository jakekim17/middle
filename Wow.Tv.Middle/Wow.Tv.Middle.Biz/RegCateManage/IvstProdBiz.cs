using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class IvstProdBiz : BaseBiz
    {
        /// <summary>
        /// 투자기간 리스트
        /// </summary>
        /// <returns></returns>
        public List<IvstProd> IvstProdList()
        {
            List<tblCodeInvestmentPeriod> codeList = (from tbl in db89_wowbill.tblCodeInvestmentPeriod select tbl).ToList();

            List<tblCodeInvestmentPeriodDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeInvestmentPeriodDetail select tbl).ToList();

            List<IvstProd> retval = (from tblA in codeList
                                             join tblB in codeDetailList on tblA.investmentPeriodId equals tblB.investmentPeriodId into _c
                                             from c in _c.DefaultIfEmpty()
                                             orderby (c != null ? c.sort : 0) ascending, tblA.investmentPeriodId ascending
                                             select new IvstProd()
                                             {
                                                 InvestmentPeriodId = tblA.investmentPeriodId,
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
        /// 투자기간 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IvstProdModifyResult> IvstProdSave(List<IvstProd> list)
        {
            // 처리결과 저장 리스트
            List<IvstProdModifyResult> retval = new List<IvstProdModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (IvstProd item in list)
            {
                // 처리결과 저장 아이템
                IvstProdModifyResult retvalItem = new IvstProdModifyResult();
                retvalItem.UserChagned = false;

                if (item.InvestmentPeriodId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeInvestmentPeriod orderby p.investmentPeriodId descending select p.investmentPeriodId).FirstOrDefault();
                    newId++;

                    tblCodeInvestmentPeriod newItem = new tblCodeInvestmentPeriod();
                    newItem.investmentPeriodId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeInvestmentPeriod.Add(newItem);

                    tblCodeInvestmentPeriodDetail newItemDetail = new tblCodeInvestmentPeriodDetail();
                    newItemDetail.investmentPeriodId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeInvestmentPeriodDetail.Add(newItemDetail);

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
                else if (item.InvestmentPeriodId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.InvestmentPeriodId = item.InvestmentPeriodId;
                    retvalItem.Descript = item.Descript;

                    tblCodeInvestmentPeriod dbItem = db89_wowbill.tblCodeInvestmentPeriod.Where(a => a.investmentPeriodId == item.InvestmentPeriodId).SingleOrDefault();
                    tblCodeInvestmentPeriodDetail dbItemDetail = db89_wowbill.tblCodeInvestmentPeriodDetail.Where(a => a.investmentPeriodId == item.InvestmentPeriodId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeInvestmentPeriodDetail();
                        dbItemDetail.investmentPeriodId = item.InvestmentPeriodId.Value;
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
                            db89_wowbill.tblCodeInvestmentPeriodDetail.Add(dbItemDetail);
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
                else if (item.InvestmentPeriodId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.InvestmentPeriodId = item.InvestmentPeriodId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.investmentPeriodId == item.InvestmentPeriodId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeInvestmentPeriod.Where(a => a.investmentPeriodId == item.InvestmentPeriodId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentPeriod.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeInvestmentPeriodDetail.Where(a => a.investmentPeriodId == item.InvestmentPeriodId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentPeriodDetail.Remove(deleteItemDetail);

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
