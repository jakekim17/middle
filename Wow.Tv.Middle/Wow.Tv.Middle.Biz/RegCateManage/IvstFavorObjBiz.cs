using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class IvstFavorObjBiz : BaseBiz
    {
        /// <summary>
        /// 투자선호대상 리스트
        /// </summary>
        /// <returns></returns>
        public List<IvstFavorObj> IvstFavorObjList()
        {
            List<tblCodeInvestmentPreferenceObject> codeList = (from tbl in db89_wowbill.tblCodeInvestmentPreferenceObject select tbl).ToList();

            List<tblCodeInvestmentPreferenceObjectDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeInvestmentPreferenceObjectDetail select tbl).ToList();

            List<IvstFavorObj> retval = (from tblA in codeList
                                         join tblB in codeDetailList on tblA.investmentPreferenceObjectId equals tblB.investmentPreferenceObjectId into _c
                                       from c in _c.DefaultIfEmpty()
                                       orderby (c != null ? c.sort : 0) ascending, tblA.investmentPreferenceObjectId ascending
                                       select new IvstFavorObj()
                                       {
                                           InvestmentPreferenceObjectId = tblA.investmentPreferenceObjectId,
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
        /// 투자선호대상 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IvstFavorObjModifyResult> IvstFavorObjSave(List<IvstFavorObj> list)
        {
            // 처리결과 저장 리스트
            List<IvstFavorObjModifyResult> retval = new List<IvstFavorObjModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (IvstFavorObj item in list)
            {
                // 처리결과 저장 아이템
                IvstFavorObjModifyResult retvalItem = new IvstFavorObjModifyResult();
                retvalItem.UserChagned = false;

                if (item.InvestmentPreferenceObjectId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeInvestmentPreferenceObject orderby p.investmentPreferenceObjectId descending select p.investmentPreferenceObjectId).FirstOrDefault();
                    newId++;

                    tblCodeInvestmentPreferenceObject newItem = new tblCodeInvestmentPreferenceObject();
                    newItem.investmentPreferenceObjectId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeInvestmentPreferenceObject.Add(newItem);

                    tblCodeInvestmentPreferenceObjectDetail newItemDetail = new tblCodeInvestmentPreferenceObjectDetail();
                    newItemDetail.investmentPreferenceObjectId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeInvestmentPreferenceObjectDetail.Add(newItemDetail);

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
                else if (item.InvestmentPreferenceObjectId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.InvestmentPreferenceObjectId = item.InvestmentPreferenceObjectId;
                    retvalItem.Descript = item.Descript;

                    tblCodeInvestmentPreferenceObject dbItem = db89_wowbill.tblCodeInvestmentPreferenceObject.Where(a => a.investmentPreferenceObjectId == item.InvestmentPreferenceObjectId).SingleOrDefault();
                    tblCodeInvestmentPreferenceObjectDetail dbItemDetail = db89_wowbill.tblCodeInvestmentPreferenceObjectDetail.Where(a => a.investmentPreferenceObjectId == item.InvestmentPreferenceObjectId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeInvestmentPreferenceObjectDetail();
                        dbItemDetail.investmentPreferenceObjectId = item.InvestmentPreferenceObjectId.Value;
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
                            db89_wowbill.tblCodeInvestmentPreferenceObjectDetail.Add(dbItemDetail);
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
                else if (item.InvestmentPreferenceObjectId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.InvestmentPreferenceObjectId = item.InvestmentPreferenceObjectId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.investmentPreferenceObjectId == item.InvestmentPreferenceObjectId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeInvestmentPreferenceObject.Where(a => a.investmentPreferenceObjectId == item.InvestmentPreferenceObjectId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentPreferenceObject.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeInvestmentPreferenceObjectDetail.Where(a => a.investmentPreferenceObjectId == item.InvestmentPreferenceObjectId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInvestmentPreferenceObjectDetail.Remove(deleteItemDetail);

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
