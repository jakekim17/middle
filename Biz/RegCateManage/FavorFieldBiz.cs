using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class FavorFieldBiz : BaseBiz
    {
        /// <summary>
        /// 관심분야 리스트
        /// </summary>
        /// <returns></returns>
        public List<FavorField> FavorFieldList()
        {
            List<tblCodeInterest> codeList = (from tbl in db89_wowbill.tblCodeInterest select tbl).ToList();

            List<tblCodeInterestDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeInterestDetail select tbl).ToList();

            List<FavorField> retval = (from tblA in codeList
                                             join tblB in codeDetailList on tblA.interestId equals tblB.interestId into _c
                                             from c in _c.DefaultIfEmpty()
                                             orderby (c != null ? c.sort : 0) ascending, tblA.interestId ascending
                                             select new FavorField()
                                             {
                                                 InterestId = tblA.interestId,
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
        /// 관심분야 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<FavorFieldModifyResult> FavorFieldSave(List<FavorField> list)
        {
            // 처리결과 저장 리스트
            List<FavorFieldModifyResult> retval = new List<FavorFieldModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (FavorField item in list)
            {
                // 처리결과 저장 아이템
                FavorFieldModifyResult retvalItem = new FavorFieldModifyResult();
                retvalItem.UserChagned = false;

                if (item.InterestId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeInterest orderby p.interestId descending select p.interestId).FirstOrDefault();
                    newId++;

                    tblCodeInterest newItem = new tblCodeInterest();
                    newItem.interestId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeInterest.Add(newItem);

                    tblCodeInterestDetail newItemDetail = new tblCodeInterestDetail();
                    newItemDetail.interestId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeInterestDetail.Add(newItemDetail);

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
                else if (item.InterestId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.InterestId = item.InterestId;
                    retvalItem.Descript = item.Descript;

                    tblCodeInterest dbItem = db89_wowbill.tblCodeInterest.Where(a => a.interestId == item.InterestId).SingleOrDefault();
                    tblCodeInterestDetail dbItemDetail = db89_wowbill.tblCodeInterestDetail.Where(a => a.interestId == item.InterestId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeInterestDetail();
                        dbItemDetail.interestId = item.InterestId.Value;
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
                            db89_wowbill.tblCodeInterestDetail.Add(dbItemDetail);
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
                else if (item.InterestId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.InterestId = item.InterestId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    var deleteItem = db89_wowbill.tblCodeInterest.Where(a => a.interestId == item.InterestId.Value).SingleOrDefault();
                    db89_wowbill.tblCodeInterest.Remove(deleteItem);

                    var deleteItemDetail = db89_wowbill.tblCodeInterestDetail.Where(a => a.interestId == item.InterestId.Value).SingleOrDefault();
                    db89_wowbill.tblCodeInterestDetail.Remove(deleteItemDetail);

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

                retval.Add(retvalItem);
            }

            return retval;
        }
    }
}
