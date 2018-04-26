using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Family;

using ClosedXML.Excel;

namespace Wow.Tv.Middle.Biz.Family
{
    public class FamilyBiz : BaseBiz
    {

        public ListModel<NTB_FAMILY> SearchList(FamilyCondition condition)
        {
            ListModel<NTB_FAMILY> resultData = new ListModel<NTB_FAMILY>();

            var list = db49_wowtv.NTB_FAMILY.AsQueryable();
            
            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                switch(condition.SearchType)
                {
                    case "All":
                        list = list.Where(a => a.SITE_NAME.Contains(condition.SearchText) == true || a.URL.Contains(condition.SearchText) == true);
                        break;
                    case "SiteName":
                        list = list.Where(a => a.SITE_NAME.Contains(condition.SearchText) == true);
                        break;
                    case "Url":
                        list = list.Where(a => a.URL.Contains(condition.SearchText) == true);
                        break;
                }
            }

            if(String.IsNullOrEmpty(condition.Active_YN) == false)
            {
                list = list.Where(a => a.ACTIVE_YN == condition.Active_YN);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.SORT_ORDER);


            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();


            foreach (var item in resultData.ListData)
            {
                var groupCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.UP_COMMON_CODE == CommonCodeStatic.FAMILY_SITE_CODE && a.CODE_VALUE1 == item.GROUP_CODE);
                if (groupCode != null)
                {
                    item.GroupCodeName = groupCode.CODE_NAME;
                }

            }


            return resultData;
        }



        public NTB_FAMILY GetAt(int familySeq)
        {
            return db49_wowtv.NTB_FAMILY.SingleOrDefault(a => a.FAMILY_SEQ == familySeq);
        }


        public int Save(NTB_FAMILY model, LoginUser loginUser)
        {
            NTB_FAMILY data = GetAt(model.FAMILY_SEQ);
            if (data == null)
            {
                int sortOrder = 1;
                var maxOrder = db49_wowtv.NTB_FAMILY.OrderByDescending(a => a.SORT_ORDER).FirstOrDefault();
                if (maxOrder != null)
                {
                    sortOrder = maxOrder.SORT_ORDER + 1;
                }
                model.SORT_ORDER = sortOrder;
                
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_FAMILY.Add(model);
            }
            else
            {
                data.GROUP_CODE = model.GROUP_CODE;
                data.SITE_NAME = model.SITE_NAME;
                data.URL = model.URL;
                data.ACTIVE_YN = model.ACTIVE_YN;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            return model.FAMILY_SEQ;
        }


        public void Delete(int familySeq, LoginUser loginUser)
        {
            NTB_FAMILY data = GetAt(familySeq);
            if (data != null)
            {
                db49_wowtv.NTB_FAMILY.Remove(data);
                db49_wowtv.SaveChanges();
            }
        }



        public void DeleteList(List<int> seqList, LoginUser loginUser)
        {
            foreach (int item in seqList)
            {
                Delete(item, loginUser);
            }
        }



        public void UpDown(int familySeq, bool isUp, LoginUser loginUser)
        {
            NTB_FAMILY upDown = null;

            var prev = GetAt(familySeq);

            if (isUp == true)
            {
                // 바로위 메뉴를 조회
                upDown = db49_wowtv.NTB_FAMILY.Where(a => a.SORT_ORDER < prev.SORT_ORDER).OrderByDescending(a => a.SORT_ORDER).FirstOrDefault();
            }
            else
            {
                // 바로아래 메뉴를 조회
                upDown = db49_wowtv.NTB_FAMILY.Where(a => a.SORT_ORDER > prev.SORT_ORDER).OrderBy(a => a.SORT_ORDER).FirstOrDefault();
            }

            if (upDown != null)
            {
                int upDownOrder = upDown.SORT_ORDER;
                upDown.SORT_ORDER = prev.SORT_ORDER;
                prev.SORT_ORDER = upDownOrder;


                upDown.MOD_ID = loginUser.LoginId;
                upDown.MOD_DATE = DateTime.Now;
                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;

                db49_wowtv.SaveChanges();
            }
        }




        public System.IO.MemoryStream ExcelExport(FamilyCondition condition)
        {
            var stream = new System.IO.MemoryStream();
            int rowIndex = 2;
            int columnIndex = 1;
            condition.PageSize = -1;
            var list = SearchList(condition);

            XLWorkbook workBook = new XLWorkbook();
            IXLWorksheet sheet = workBook.AddWorksheet("패밀리사이트 목록");

            sheet.Cell(1, 1).Value = "패밀리사이트 목록";
            sheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 15;


            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "번호";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "그룹";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "사이트명";
            sheet.Column(columnIndex).Width = 35;
            sheet.Cell(rowIndex, columnIndex++).Value = "URL";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "활성";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "작성일";

            columnIndex = 1;
            foreach (var item in list.ListData)
            {
                columnIndex = 1;
                rowIndex++;

                sheet.Cell(rowIndex, columnIndex++).SetValue<int>(item.FAMILY_SEQ);
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.GroupCodeName) == true ? "" : item.GroupCodeName));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.SITE_NAME) == true ? "" : item.SITE_NAME));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.URL) == true ? "" : item.URL));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.ACTIVE_YN) == true ? "" : item.ACTIVE_YN));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>(item.REG_DATE.ToDateTime() + "\r\n" + item.MOD_DATE.ToDateTime());
            }

            if (columnIndex > 1)
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



    }
}
