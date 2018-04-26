using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv.MemberManage;

namespace Wow.Tv.Middle.Biz.MemberManage
{
    public class MemberMonitoringBiz : BaseBiz
    {
        public ListModel<TotalListResult> TotalList(TotalListCondition condition)
        {
            if (condition.PageSize <= 0)
            {
                condition.PageSize = 20;
            }

            ListModel<TotalListResult> retval = new ListModel<TotalListResult>();

            // MENU 조건
            var menuList = db49_wowtv.NTB_MENU.AsNoTracking().AsQueryable();
            if (condition.SearchType == "MENU_NAME")
            {
                menuList = menuList.Where(a => string.IsNullOrEmpty(a.MENU_NAME) == false && a.MENU_NAME.IndexOf(condition.SearchText) > -1);
            }

            // ACTION LOG 조건
            var actionLogList = db49_wowtv.NTB_ACTION_LOG.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }
            if (condition.SearchType == "ADMIN_ID")
            {
                actionLogList = actionLogList.Where(a => string.IsNullOrEmpty(a.REG_ID) == false && a.REG_ID.IndexOf(condition.SearchText) > -1);
            }

            // ACCESS LOG 조건
            var accessLogList = db49_wowtv.NTB_ACCESS_LOG.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                accessLogList = accessLogList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                accessLogList = accessLogList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }
            if (condition.SearchType == "ADMIN_ID")
            {
                accessLogList = accessLogList.Where(a => string.IsNullOrEmpty(a.REG_ID) == false && a.REG_ID.IndexOf(condition.SearchText) > -1);
            }

            var unionList = (from log in actionLogList
                             join menu in menuList on
                             log.MENU_SEQ equals menu.MENU_SEQ
                             select new TotalListResult()
                             {
                                 MenuSeq = menu.MENU_SEQ,
                                 IsActionLog = true,
                                 MenuName = string.IsNullOrEmpty(menu.MENU_NAME) == false ? menu.MENU_NAME : "",
                                 TableKey = log.TABLE_KEY,
                                 ActionCode = string.IsNullOrEmpty(log.ACTION_CODE) == false ? log.ACTION_CODE : "",
                                 ActionDate = log.REG_DATE,
                                 AdminId = string.IsNullOrEmpty(log.REG_ID) == false ? log.REG_ID : "",
                                 IpAddress = log.IP
                             }).Union
                                (
                                    from log in accessLogList
                                    join menu in menuList on
                                    log.MENU_SEQ equals menu.MENU_SEQ
                                    select new TotalListResult()
                                    {
                                        MenuSeq = menu.MENU_SEQ,
                                        IsActionLog = false,
                                        MenuName = string.IsNullOrEmpty(menu.MENU_NAME) == false ? menu.MENU_NAME : "",
                                        TableKey = "",
                                        ActionCode = log.ACCESS_CODE,
                                        ActionDate = log.REG_DATE,
                                        AdminId = string.IsNullOrEmpty(log.REG_ID) == false ? log.REG_ID : "",
                                        IpAddress = log.IP
                                    }
                                );

            retval.TotalDataCount = unionList.Count();
            retval.ListData = unionList.OrderByDescending(a => a.ActionDate).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            return retval;
        }

        public ListModel<NewRegCfmResult> NewRegCfm(NewRegCfmCondition condition)
        {
            if (condition.PageSize <= 0)
            {
                condition.PageSize = 20;
            }

            ListModel<NewRegCfmResult> retval = new ListModel<NewRegCfmResult>();

            var adminList = db49_wowtv.TAB_CMS_ADMIN.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                adminList = adminList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                adminList = adminList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }

            var list = (from admin in adminList
                        join grp in db49_wowtv.NTB_GROUP.AsNoTracking() on admin.GROUP_SEQ equals grp.GROUP_SEQ into _grp
                        from grp in _grp.DefaultIfEmpty()
                        join pwdHistory in db49_wowtv.TAB_CMS_ADMIN_PwdHistory.AsNoTracking() on admin.ADMIN_ID equals pwdHistory.admin_id into _pwdHistory
                        from pwdHistory in _pwdHistory.DefaultIfEmpty()
                        select new NewRegCfmResult()
                        {
                            AdminId = admin.ADMIN_ID,
                            AdminName = admin.NAME,
                            GroupName = grp.GROUP_NAME,
                            PasswordChangedDate = pwdHistory.udtDt,
                            LatestConnectedDate = admin.LAST_LOGIN_DATE,
                            RegisteredDate = admin.REG_DATE
                        });

            retval.TotalDataCount = list.Count();
            retval.ListData = list.OrderByDescending(a => a.RegisteredDate).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            return retval;
        }

        public ListModel<AccAuthResult> AccAuth(AccAuthCondition condition)
        {
            ListModel<AccAuthResult> retval = new ListModel<AccAuthResult>();

            if (condition.PageSize <= 0)
            {
                condition.PageSize = 20;
            }

            var actionLogList = db49_wowtv.NTB_ACTION_LOG.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }
            actionLogList = actionLogList.Where(a => a.ADD_INFO_1 == "그룹생성" || a.ADD_INFO_1 == "그룹수정" || a.ADD_INFO_1 == "그룹삭제" || a.ADD_INFO_1 == "그룹복사" || a.ADD_INFO_1 == "그룹권한수정");

            retval.TotalDataCount = actionLogList.Count();

            List<AccAuthResult> listData = (from log in actionLogList
                              join grp in db49_wowtv.NTB_GROUP.AsNoTracking() on
                              log.TABLE_KEY equals grp.GROUP_SEQ.ToString()
                              select new AccAuthResult()
                              {
                                  TableKey = log.TABLE_KEY,
                                  ActionCode = log.ACTION_CODE,
                                  ActionType = log.ADD_INFO_1,
                                  ActionDate = log.REG_DATE,
                                  AdminId = log.REG_ID,
                                  IpAddress = log.IP,
                                  GroupName = grp.GROUP_NAME
                             }).OrderByDescending(a => a.ActionDate).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();


            foreach (AccAuthResult item in listData)
            {
                switch (item.ActionCode?.ToUpper())
                {
                    case "INSERT": item.ActionName = "등록"; break;
                    case "UPDATE": item.ActionName = "수정"; break;
                    case "DELETE": item.ActionName = "삭제"; break;
                }

                if (item.ActionType == "그룹생성" || item.ActionType == "그룹수정" || item.ActionType == "그룹삭제")
                {
                    item.ActionDescription = item.AdminId + "가 " + item.GroupName + "을(를) " + item.ActionName;
                }
                else if (item.ActionType == "그룹복사")
                {
                    item.ActionDescription = item.AdminId + "가 " + item.GroupName + "을(를) 복사";
                }
                else if (item.ActionType == "그룹권한수정")
                {
                    item.ActionDescription = item.AdminId + "가 " + item.GroupName + "의 권한을 " + item.ActionName;
                }
            }

            retval.ListData = listData;
            return retval;
        }

        public ListModel<AccLogResult> AccLog(AccLogCondition condition)
        {
            if (condition.PageSize <= 0)
            {
                condition.PageSize = 20;
            }

            ListModel<AccLogResult> retval = new ListModel<AccLogResult>();

            // MENU 조건
            var menuList = db49_wowtv.NTB_MENU.AsNoTracking().AsQueryable();
            if (condition.SearchType == "MENU_NAME")
            {
                menuList = menuList.Where(a => string.IsNullOrEmpty(a.MENU_NAME) == false && a.MENU_NAME.IndexOf(condition.SearchText) > -1);
            }

            // ACTION LOG 조건
            var actionLogList = db49_wowtv.NTB_ACTION_LOG.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }
            if (condition.SearchType == "ADMIN_ID")
            {
                actionLogList = actionLogList.Where(a => string.IsNullOrEmpty(a.REG_ID) == false && a.REG_ID.IndexOf(condition.SearchText) > -1);
            }

            var list = (from log in actionLogList
                             join menu in menuList on
                             log.MENU_SEQ equals menu.MENU_SEQ
                             select new AccLogResult()
                             {
                                 MenuSeq = menu.MENU_SEQ,
                                 MenuName = string.IsNullOrEmpty(menu.MENU_NAME) == false ? menu.MENU_NAME : "",
                                 TableKey = log.TABLE_KEY,
                                 ActionCode = string.IsNullOrEmpty(log.ACTION_CODE) == false ? log.ACTION_CODE : "",
                                 ActionDate = log.REG_DATE,
                                 AdminId = string.IsNullOrEmpty(log.REG_ID) == false ? log.REG_ID : "",
                                 IpAddress = log.IP
                             });

            retval.TotalDataCount = list.Count();
            retval.ListData = list.OrderByDescending(a => a.ActionDate).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            return retval;
        }

        public ListModel<WorkOutLogHistResult> WorkOutLogHist(WorkOutLogHistCondition condition)
        {
            if (condition.PageSize <= 0)
            {
                condition.PageSize = 20;
            }

            ListModel<WorkOutLogHistResult> retval = new ListModel<WorkOutLogHistResult>();

            // MENU 조건
            var menuList = db49_wowtv.NTB_MENU.AsNoTracking().AsQueryable();
            if (condition.SearchType == "MENU_NAME")
            {
                menuList = menuList.Where(a => string.IsNullOrEmpty(a.MENU_NAME) == false && a.MENU_NAME.IndexOf(condition.SearchText) > -1);
            }

            // ACTION LOG 조건
            var actionLogList = db49_wowtv.NTB_ACTION_LOG.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }
            if (condition.SearchType == "ADMIN_ID")
            {
                actionLogList = actionLogList.Where(a => string.IsNullOrEmpty(a.REG_ID) == false && a.REG_ID.IndexOf(condition.SearchText) > -1);
            }

            actionLogList = actionLogList.Where(a => a.REG_DATE.Value.Hour >= 2 && a.REG_DATE.Value.Hour <= 6);

            var list = (from log in actionLogList
                        join menu in menuList on
                        log.MENU_SEQ equals menu.MENU_SEQ
                        select new WorkOutLogHistResult()
                        {
                            MenuSeq = menu.MENU_SEQ,
                            MenuName = string.IsNullOrEmpty(menu.MENU_NAME) == false ? menu.MENU_NAME : "",
                            TableKey = log.TABLE_KEY,
                            ActionCode = string.IsNullOrEmpty(log.ACTION_CODE) == false ? log.ACTION_CODE : "",
                            ActionDate = log.REG_DATE,
                            AdminId = string.IsNullOrEmpty(log.REG_ID) == false ? log.REG_ID : "",
                            IpAddress = log.IP
                        });

            retval.TotalDataCount = list.Count();
            retval.ListData = list.OrderByDescending(a => a.ActionDate).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            return retval;
        }

        public ListModel<MemInfoOpenChkResult> MemInfoOpenChk(MemInfoOpenChkCondition condition)
        {
            if (condition.PageSize <= 0)
            {
                condition.PageSize = 20;
            }

            ListModel<MemInfoOpenChkResult> retval = new ListModel<MemInfoOpenChkResult>();

            var actionLogList = db49_wowtv.NTB_ACTION_LOG.AsNoTracking().AsQueryable();
            if (condition.RegistDateFrom.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE >= condition.RegistDateFrom.Value);
            }
            if (condition.RegistDateTo.HasValue == true)
            {
                actionLogList = actionLogList.Where(a => a.REG_DATE <= condition.RegistDateTo.Value);
            }
            actionLogList = actionLogList.Where(a => a.ADD_INFO_1 == "회원상세정보" && a.REG_DATE.HasValue == true);

            var totalList = actionLogList.Select(a => new { ACTION_SEQ = a.ACTION_SEQ, REG_ID = a.REG_ID, REG_DATE = a.REG_DATE, IP = a.IP,
                GROUP_DATE = SqlFunctions.DateName("year", a.REG_DATE) + SqlFunctions.DateName("month", a.REG_DATE) + SqlFunctions.DateName("day", a.REG_DATE) });

            var groupList = (from lst in totalList
                             group lst by new { lst.REG_ID, lst.GROUP_DATE } into z
                              select new MemInfoOpenChkResult {
                                  AdminId = z.Key.REG_ID,
                                  OpenDate = z.Key.GROUP_DATE,
                                  OpenCount = z.Count(),
                                  LatestActionLogSeq = z.Max(a => a.ACTION_SEQ),
                                  LatestOpenDate = z.Max(a => a.REG_DATE)
                              }
                            );

            List<MemInfoOpenChkResult> listData = groupList.Where(a => a.OpenCount >= 100).ToList();
            foreach (MemInfoOpenChkResult item in listData)
            {
                item.IpAddress = totalList.Where(a => a.ACTION_SEQ == item.LatestActionLogSeq).Select(a => a.IP).SingleOrDefault();
            }

            retval.TotalDataCount = listData.Count();
            retval.ListData = listData.OrderByDescending(a => a.LatestOpenDate).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            return retval;
        }
    }
}
