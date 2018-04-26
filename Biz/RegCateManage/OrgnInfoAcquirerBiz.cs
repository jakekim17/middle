using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.Biz.RegCateManage
{
    public class OrgnInfoAcquirerBiz : BaseBiz
    {
        /// <summary>
        /// 기존정보 습득처 리스트
        /// </summary>
        /// <returns></returns>
        public List<OrgnInfoAcquirer> OrgnInfoAcquirerList()
        {
            List<tblCodeInfoAcquirement> codeList = (from tbl in db89_wowbill.tblCodeInfoAcquirement select tbl).ToList();

            List<tblCodeInfoAcquirementDetail> codeDetailList = (from tbl in db89_wowbill.tblCodeInfoAcquirementDetail select tbl).ToList();

            List<OrgnInfoAcquirer> retval = (from tblA in codeList
                                         join tblB in codeDetailList on tblA.infoAcquirementId equals tblB.infoAcquirementId into _c
                                         from c in _c.DefaultIfEmpty()
                                         orderby (c != null ? c.sort : 0) ascending, tblA.infoAcquirementId ascending
                                         select new OrgnInfoAcquirer()
                                         {
                                             InfoAcquirementId = tblA.infoAcquirementId,
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
        /// 기존정보 습득처 저장
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<OrgnInfoAcquirerModifyResult> OrgnInfoAcquirerSave(List<OrgnInfoAcquirer> list)
        {
            // 처리결과 저장 리스트
            List<OrgnInfoAcquirerModifyResult> retval = new List<OrgnInfoAcquirerModifyResult>();

            // 처리날짜 선언
            DateTime now = DateTime.Now;

            // 전체 수정 리스트 이터레이션 (추가/수정/삭제 아이템)
            foreach (OrgnInfoAcquirer item in list)
            {
                // 처리결과 저장 아이템
                OrgnInfoAcquirerModifyResult retvalItem = new OrgnInfoAcquirerModifyResult();
                retvalItem.UserChagned = false;

                if (item.InfoAcquirementId == null && item.SaveType == "ADD")
                {// 추가
                    retvalItem.Descript = item.Descript;
                    retvalItem.UserChagned = true;

                    byte newId = (from p in db89_wowbill.tblCodeInfoAcquirement orderby p.infoAcquirementId descending select p.infoAcquirementId).FirstOrDefault();
                    newId++;

                    tblCodeInfoAcquirement newItem = new tblCodeInfoAcquirement();
                    newItem.infoAcquirementId = newId;
                    newItem.descript = item.Descript;
                    newItem.apply = item.Apply;
                    newItem.adminId = item.AdminId;
                    newItem.registDt = now;
                    db89_wowbill.tblCodeInfoAcquirement.Add(newItem);

                    tblCodeInfoAcquirementDetail newItemDetail = new tblCodeInfoAcquirementDetail();
                    newItemDetail.infoAcquirementId = newId;
                    newItemDetail.sort = item.Sort;
                    db89_wowbill.tblCodeInfoAcquirementDetail.Add(newItemDetail);

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
                else if (item.InfoAcquirementId.HasValue == true && item.SaveType == "MODIFY")
                {// 수정
                    retvalItem.InfoAcquirementId = item.InfoAcquirementId;
                    retvalItem.Descript = item.Descript;

                    tblCodeInfoAcquirement dbItem = db89_wowbill.tblCodeInfoAcquirement.Where(a => a.infoAcquirementId == item.InfoAcquirementId).SingleOrDefault();
                    tblCodeInfoAcquirementDetail dbItemDetail = db89_wowbill.tblCodeInfoAcquirementDetail.Where(a => a.infoAcquirementId == item.InfoAcquirementId).SingleOrDefault();
                    bool dbItemDetailAdded = false;
                    if (dbItemDetail == null)
                    {
                        dbItemDetailAdded = true;
                        dbItemDetail = new tblCodeInfoAcquirementDetail();
                        dbItemDetail.infoAcquirementId = item.InfoAcquirementId.Value;
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
                            db89_wowbill.tblCodeInfoAcquirementDetail.Add(dbItemDetail);
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
                else if (item.InfoAcquirementId.HasValue == true && item.SaveType == "DELETE")
                {// 삭제
                    retvalItem.InfoAcquirementId = item.InfoAcquirementId;
                    retvalItem.Descript = item.Descript;

                    retvalItem.UserChagned = true;

                    IQueryable<tblUserDetail> data = db89_wowbill.tblUserDetail.Where(a => a.infoAcquirementId == item.InfoAcquirementId.Value).Take(1);
                    if (data.Count() > 0)
                    {
                        retvalItem.IsSuccess = false;
                        retvalItem.ReturnMessage = "사용중인 항목";
                    }
                    else
                    {
                        var deleteItem = db89_wowbill.tblCodeInfoAcquirement.Where(a => a.infoAcquirementId == item.InfoAcquirementId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInfoAcquirement.Remove(deleteItem);

                        var deleteItemDetail = db89_wowbill.tblCodeInfoAcquirementDetail.Where(a => a.infoAcquirementId == item.InfoAcquirementId.Value).SingleOrDefault();
                        db89_wowbill.tblCodeInfoAcquirementDetail.Remove(deleteItemDetail);

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
