using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;

using ClosedXML.Excel;

namespace Wow.Tv.Middle.Biz.Board
{
    public class BoardBiz : BaseBiz
    {
        public ListModel<NTB_BOARD> SearchList(BoardCondition condition)
        {
            ListModel<NTB_BOARD> resultData = new ListModel<NTB_BOARD>();

            var list = db49_wowtv.NTB_BOARD.Where(a => a.DEL_YN == "N");

            if (String.IsNullOrEmpty(condition.ActiveYn) == false)
            {
                list = list.Where(a => a.ACTIVE_YN == condition.ActiveYn);
            }

            if (String.IsNullOrEmpty(condition.BoardTypeCode) == false)
            {
                list = list.Where(a => a.BOARD_TYPE_CODE == condition.BoardTypeCode);
            }

            if (String.IsNullOrEmpty(condition.BoardName) == false)
            {
                list = list.Where(a => a.BOARD_NAME.Contains(condition.BoardName) == true);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.BOARD_SEQ);

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
                var boardTypeCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.UP_COMMON_CODE == CommonCodeStatic.BOARD_TYPE_CODE && a.CODE_VALUE1 == item.BOARD_TYPE_CODE);
                if (boardTypeCode != null)
                {
                    item.BoardTypeCodeName = boardTypeCode.CODE_NAME;
                }

            }

            resultData.AddInfoInt1 = db49_wowtv.NTB_BOARD_CONTENT.Where(a => a.DEL_YN == "N").Count();

            return resultData;
        }



        public NTB_BOARD GetAt(int boardSeq)
        {
            return db49_wowtv.NTB_BOARD.SingleOrDefault(a => a.BOARD_SEQ == boardSeq);
        }




        public int Save(NTB_BOARD model, LoginUser loginUser)
        {
            NTB_BOARD data = GetAt(model.BOARD_SEQ);
            if (data == null)
            {
                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_BOARD.Add(model);
            }
            else
            {
                data.BOARD_TYPE_CODE = model.BOARD_TYPE_CODE;
                data.BOARD_NAME = model.BOARD_NAME;
                data.TOP_NOTICE_YN = model.TOP_NOTICE_YN;
                data.COMMENT_YN = model.COMMENT_YN;
                data.REPLY_YN = model.REPLY_YN;
                data.ATTACH_FILE_YN = model.ATTACH_FILE_YN;
                data.FILE_COUNT = model.FILE_COUNT;
                data.SCRAP_YN = model.SCRAP_YN;
                data.ACTIVE_YN = model.ACTIVE_YN;
                data.BLIND_YN = model.BLIND_YN;
                data.KEYWORD_YN = model.KEYWORD_YN;
                data.PASSWORD_YN = model.PASSWORD_YN;
                data.TOP_CONTENT = model.TOP_CONTENT;
                data.BOTTOM_CONTENT = model.BOTTOM_CONTENT;
                data.EMAIL_YN = model.EMAIL_YN;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            return model.BOARD_SEQ;
        }


        public void Delete(int boardSeq, LoginUser loginUser)
        {
            NTB_BOARD data = GetAt(boardSeq);
            if (data != null)
            {
                //db49_wowtv.NTB_BOARD.Remove(data);
                data.DEL_YN = "Y";
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



        public System.IO.MemoryStream ExcelExport(BoardCondition condition)
        {
            var stream = new System.IO.MemoryStream();
            int rowIndex = 2;
            int columnIndex = 1;
            condition.PageSize = -1;
            var list = SearchList(condition);

            XLWorkbook workBook = new XLWorkbook();
            IXLWorksheet sheet = workBook.AddWorksheet("통합게시판 목록");

            sheet.Cell(1, 1).Value = "통합게시판 목록";
            sheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 15;


            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "번호";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "유형";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "게시판명";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "파일첨부";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "활성";
            sheet.Column(columnIndex).Width = 25;
            sheet.Cell(rowIndex, columnIndex++).Value = "작성일";

            columnIndex = 1;
            foreach (var item in list.ListData)
            {
                columnIndex = 1;
                rowIndex++;

                sheet.Cell(rowIndex, columnIndex++).SetValue<int>(item.BOARD_SEQ);
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.BoardTypeCodeName) == true ? "" : item.BoardTypeCodeName));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.BOARD_NAME) == true ? "" : item.BOARD_NAME));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.ATTACH_FILE_YN) == true ? "" : item.ATTACH_FILE_YN));
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
