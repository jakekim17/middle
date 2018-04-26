using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.AttachFile;

namespace Wow.Tv.Middle.Biz.AttachFile
{
    public class AttachFileBiz : BaseBiz
    {
        /// <summary>
        /// 첨부파일 조회
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns></returns>
        public ListModel<NTB_ATTACH_FILE> SearchList(AttachFileCondition condition)
        {
            ListModel<NTB_ATTACH_FILE> resultData = new ListModel<NTB_ATTACH_FILE>();

            var list = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.DEL_YN == "N");


            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.ATTACH_FILE_SEQ);


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


        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="attachFileSeq">첨부파일고유번호</param>
        /// <returns></returns>
        public NTB_ATTACH_FILE GetAt(int attachFileSeq)
        {
            return db49_wowtv.NTB_ATTACH_FILE.SingleOrDefault(a => a.ATTACH_FILE_SEQ == attachFileSeq);
        }
        public NTB_ATTACH_FILE GetAt(string tableCode, string tableKey)
        {
            return db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == tableCode && a.TABLE_KEY == tableKey).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
        }


        /// <summary>
        /// 첨부파일 생성
        /// </summary>
        /// <param name="model"></param>
        public void Create(NTB_ATTACH_FILE model)
        {
            string extension = "";

            if(String.IsNullOrEmpty(model.TABLE_CODE) == true)
            {
                throw new Exception("Table Code를 입력하여 주십시오.");
            }
            if (String.IsNullOrEmpty(model.TABLE_KEY) == true)
            {
                throw new Exception("Table Key를 입력하여 주십시오.");
            }
            if (String.IsNullOrEmpty(model.USER_UPLOAD_FILE_NAME) == true)
            {
                throw new Exception("사용자 업로드 파일명을 입력하여 주십시오.");
            }
            if (String.IsNullOrEmpty(model.REAL_FILE_PATH) == true)
            {
                throw new Exception("실제 업로드 파일경로를 입력하여 주십시오.");
            }
            if (String.IsNullOrEmpty(model.REAL_WEB_PATH) == true)
            {
                throw new Exception("실제 웹 경로를 입력하여 주십시오.");
            }
            if (model.FILE_SIZE <= 0)
            {
                throw new Exception("파일용량을 입력하여 주십시오.");
            }

            extension = System.IO.Path.GetExtension(model.USER_UPLOAD_FILE_NAME);
            model.EXTENSION = extension;
            model.REG_DATE = DateTime.Now;

            model.DEL_YN = "N";


            db49_wowtv.NTB_ATTACH_FILE.Add(model);
            db49_wowtv.SaveChanges();
        }


        /// <summary>
        /// 첨부파일 삭제(플래그처리 아님, 실제로 삭제함)
        /// </summary>
        /// <param name="attachFileSeq">첨부파일고유번호</param>
        public void Delete(int attachFileSeq)
        {
            var prev = GetAt(attachFileSeq);

            prev.DEL_YN = "Y";
            //db49_wowtv.NTB_ATTACH_FILE.Remove(prev);
            db49_wowtv.SaveChanges();
        }

        /// <summary>
        /// 공통 게시판에 게시물에 등록된 첨부파일 모두 삭제(플래그처리 아님, 실제로 삭제함)
        /// </summary>
        /// <param name="tableCode">TABLE 명</param>
        /// <param name="tableKey">tableCode에 Key</param>
        public void DeleteAll(string tableCode,string tableKey)
        {
            var files = GetFileList(tableCode, tableKey);
            files.ForEach(x=> x.DEL_YN = "Y");
            db49_wowtv.SaveChanges();
        }

        /// <summary>
        /// 게시물에 첨부 파일 정보를 가져온다.
        /// </summary>
        /// <param name="tableCode">TABLE 명</param>
        /// <param name="tableKey">tableCode에 Key</param>
        /// <returns>NTB_ATTACH_FILE List</returns>
        public List<NTB_ATTACH_FILE> GetFileList(string tableCode, string tableKey)
        {
            return db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == tableCode && a.TABLE_KEY == tableKey).ToList();
        }
    }
}
