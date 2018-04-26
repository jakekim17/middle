using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.ActionLog;

using ClosedXML.Excel;

namespace Wow.Tv.Middle.Biz.ActionLog
{
    public class ActionLogBiz : BaseBiz
    {
        public enum ActionCode { Insert, Update, Delete, Select, Excel };
        

        public ListModel<NTB_ACTION_LOG> SearchList(ActionLogCondition condition)
        {
            ListModel<NTB_ACTION_LOG> resultData = new ListModel<NTB_ACTION_LOG>();

            var list = db49_wowtv.NTB_ACTION_LOG.AsQueryable();

            if(String.IsNullOrEmpty(condition.ActionCode) == false)
            {
                list = list.Where(a => a.ACTION_CODE == condition.ActionCode);
            }

            if(condition.StartDate != null)
            {
                list = list.Where(a => a.REG_DATE >= condition.StartDate);
            }
            if (condition.EndDate != null)
            {
                DateTime dtEnd = condition.EndDate.Value.ToDateFinishTime();
                list = list.Where(a => a.REG_DATE <= dtEnd);
            }

            if(String.IsNullOrEmpty(condition.SearchText) == false)
            {
                switch(condition.SearchType)
                {
                    case "MenuName":
                        list = list.Where(a => a.MENU_NAME.Contains(condition.SearchText) == true);
                        break;
                    case "Url":
                        list = list.Where(a => a.URL.Contains(condition.SearchText) == true);
                        break;
                    case "AdminId":
                        list = list.Where(a => a.REG_ID.Contains(condition.SearchText) == true);
                        break;
                    case "AdminName":
                        list = list.Where(a => a.REG_NAME.Contains(condition.SearchText) == true);
                        break;
                }
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.REG_DATE);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }


        public System.IO.MemoryStream ExcelExport(ActionLogCondition condition)
        {
            var stream = new System.IO.MemoryStream();
            int rowIndex = 2;
            int columnIndex = 1;
            condition.PageSize = -1;
            var list = SearchList(condition);

            XLWorkbook workBook = new XLWorkbook();
            IXLWorksheet sheet = workBook.AddWorksheet("운영자 로그 목록");

            sheet.Cell(1, 1).Value = "운영자 로그 목록";
            sheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 15;


            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "메뉴명";
            sheet.Column(columnIndex).Width = 30;
            sheet.Cell(rowIndex, columnIndex++).Value = "URL";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "액션타입";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "설명";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "접속IP";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "운영자";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "작성일";

            columnIndex = 1;
            foreach (var item in list.ListData)
            {
                columnIndex = 1;
                rowIndex++;
                
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.MENU_NAME) == true ? "" : item.MENU_NAME));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.URL) == true ? "" : item.URL));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.ACTION_CODE) == true ? "" : item.ACTION_CODE));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.ADD_INFO_1) == true ? "" : item.ADD_INFO_1));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.IP) == true ? "" : item.IP));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.REG_NAME) == true ? "" : item.REG_NAME));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>(item.REG_DATE.ToDateTime());
            }

            if(columnIndex > 1)
            {
                columnIndex = columnIndex - 1;
            }

            sheet.Range(1, 1, 1, columnIndex).Merge();
            sheet.Range(2, 1, 2, columnIndex).Style.Fill.BackgroundColor = XLColor.FromHtml("#D8D8D8");
            sheet.Range(2, 1, 2, columnIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            sheet.Range(2, 1, rowIndex, columnIndex).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            sheet.Range(2, 1, rowIndex, columnIndex).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            workBook.SaveAs(stream);

            return stream;
        }


        /// <summary>
        /// 로그 생성
        /// </summary>
        /// <param name="menuSeq"></param>
        /// <param name="tableKey"></param>
        /// <param name="actionCode"></param>
        /// <param name="addInfo1"></param>
        /// <param name="addInfo2"></param>
        /// <param name="loginUser"></param>
        public void Create(string menuSeq, string tableKey, ActionCode actionCode, string addInfo1, string addInfo2, LoginUser loginUser)
        {
            NTB_ACTION_LOG model = new NTB_ACTION_LOG();
            model.IP = loginUser.Ip;
            if(String.IsNullOrEmpty(menuSeq) == true)
            {
                model.MENU_SEQ = 0;
            }
            else
            {
                model.MENU_SEQ = int.Parse(menuSeq);
            }
            model.TABLE_KEY = tableKey;
            model.ACTION_CODE = actionCode.ToString();
            model.ADD_INFO_1 = addInfo1;
            model.ADD_INFO_2 = addInfo2;
            model.REG_ID = loginUser.LoginId;
            model.REG_DATE = DateTime.Now;

            if (String.IsNullOrEmpty(model.REG_ID) == false)
            {
                var reg = db49_wowtv.TAB_CMS_ADMIN.SingleOrDefault(a => a.ADMIN_ID == model.REG_ID);
                if (reg != null)
                {
                    model.REG_NAME = reg.NAME;
                }
            }

            if (model.MENU_SEQ > 0)
            {
                var menu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.MENU_SEQ);
                if (menu != null)
                {
                    model.MENU_NAME = menu.MENU_NAME;
                    model.URL = menu.LINK_URL;
                }
            }

            db49_wowtv.NTB_ACTION_LOG.Add(model);
            db49_wowtv.SaveChanges();
        }


        public void Create(string menuSeq, string tableKey, string addInfo1, string addInfo2, LoginUser loginUser)
        {
            ActionCode actionCode = ActionCode.Update;
            if(String.IsNullOrEmpty(tableKey) == true || tableKey == "0")
            {
                actionCode = ActionCode.Insert;
            }

            Create(menuSeq, tableKey, actionCode, addInfo1, addInfo2, loginUser);
        }



    }
}
