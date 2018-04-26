using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class YearIncomeBiz : BaseBiz
    {
        /// <summary>
        /// 연간수입 리스트
        /// </summary>
        /// <returns></returns>
        public List<YearIncome> YearIncomeList()
        {
            List<tblCodeSalary> salary = (from tbl in db89_wowbill.tblCodeSalary select tbl).ToList();

            List<tblCodeSalaryDetail> salaryDetail = (from tbl in db89_wowbill.tblCodeSalaryDetail select tbl).ToList();

            List<YearIncome> retval = (from tblA in salary
                                       join tblB in salaryDetail on tblA.salaryId equals tblB.salaryId into _c
                                       from c in _c.DefaultIfEmpty()
                                       orderby (c != null ? c.sort : 0) ascending, tblA.salaryId ascending
                                       select new YearIncome()
                                       {
                                           SalaryId = tblA.salaryId,
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
        /// 연간수입 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<YearIncomeModifyResult> YearIncomeSave(List<YearIncome> list)
        {
            // 처리결과 저장 리스트
            List<YearIncomeModifyResult> retval = new List<YearIncomeModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (YearIncome item in list)
            {
                // 처리결과 저장 아이템
                YearIncomeModifyResult retvalItem = new YearIncomeModifyResult();
                retvalItem.UserChagned = false;

                if (item.SalaryId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeSalary orderby p.salaryId descending select p.salaryId).FirstOrDefault();
                    newId++;

                    tblCodeSalary newItem = new tblCodeSalary();
                    newItem.salaryId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeSalary.Add(newItem);

                    tblCodeSalaryDetail newItemDetail = new tblCodeSalaryDetail();
                    newItemDetail.salaryId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeSalaryDetail.Add(newItemDetail);

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
                else if (item.SalaryId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.SalaryId = item.SalaryId;
                    retvalItem.Descript = item.Descript;

                    tblCodeSalary dbItem = db89_wowbill.tblCodeSalary.Where(a => a.salaryId == item.SalaryId).SingleOrDefault();
                    tblCodeSalaryDetail dbItemDetail = db89_wowbill.tblCodeSalaryDetail.Where(a => a.salaryId == item.SalaryId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeSalaryDetail();
                        dbItemDetail.salaryId = item.SalaryId.Value;
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
                            db89_wowbill.tblCodeSalaryDetail.Add(dbItemDetail);
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
                else if (item.SalaryId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.SalaryId = item.SalaryId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.salaryId == item.SalaryId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeSalary.Where(a => a.salaryId == item.SalaryId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeSalary.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeSalaryDetail.Where(a => a.salaryId == item.SalaryId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeSalaryDetail.Remove(deleteItemDetail);

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
