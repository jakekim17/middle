using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db49.wowtv.Kvina;

namespace Wow.Tv.Middle.Biz.Admin
{
	public class KvinaBiz : BaseBiz
	{
		//KVINA 공지사항 리스트
		public ListModel<KvinaNoticeList> KvinaNoticeList(IntegratedBoardCondition condition)
		{
			string sqlQuery = $@"
					SELECT 	SEQ,BCODE,TITLE,USER_ID,USER_NAME,REF,REF_STEP,REF_LEVEL,READ_CNT,MEMO_CNT,RECOMMEND_CNT,REVERSE_CNT,ASTROMANCY_CNT,VIEW_FLAG,EMOTICON, REG_DATE,GUBUN,SOURCE,EMAIL,PUBLICITY_CNT, (select count(*) from TAB_NOTICE with (nolock) where Bcode = 'T07020000') as ROWCNT

					FROM TAB_NOTICE	 with (nolock)
					WHERE
						BCODE = 'T07020000'
					ORDER BY SEQ DESC	";
			


			//검색
			if (!string.IsNullOrEmpty(condition.SearchText))
			{
				if (condition.SearchType.Equals("TITLE"))
				{
					string likeTitle = "'%"+ condition.SearchText + "%'";
					sqlQuery = $@"
							SELECT 	SEQ,BCODE,TITLE,USER_ID,USER_NAME,REF,REF_STEP,REF_LEVEL,READ_CNT,MEMO_CNT,RECOMMEND_CNT,REVERSE_CNT,ASTROMANCY_CNT,VIEW_FLAG,EMOTICON, REG_DATE,GUBUN,SOURCE,EMAIL,PUBLICITY_CNT, (select count(*) from TAB_NOTICE with (nolock) where Bcode = 'T07020000' and TITLE like {likeTitle} 	) as ROWCNT

							FROM TAB_NOTICE	 with (nolock)
							WHERE
								BCODE = 'T07020000'
								and TITLE like {likeTitle} ORDER BY SEQ DESC	";

				}
				else if (condition.SearchType.Equals("CONTENT"))
				{
					string likeContent = "'%"+ condition.SearchText + "%'";
					sqlQuery = $@"
								SELECT 	SEQ,BCODE,TITLE,USER_ID,USER_NAME,REF,REF_STEP,REF_LEVEL,READ_CNT,MEMO_CNT,RECOMMEND_CNT,REVERSE_CNT,ASTROMANCY_CNT,VIEW_FLAG,EMOTICON, REG_DATE,GUBUN,SOURCE,EMAIL,PUBLICITY_CNT, (select count(*) from TAB_NOTICE with (nolock) where Bcode = 'T07020000' and CONTENT like {likeContent} ) as ROWCNT

								FROM TAB_NOTICE	 with (nolock)
								WHERE
									BCODE = 'T07020000'
									and CONTENT like {likeContent} ORDER BY SEQ DESC	";
				}

			}



			var resultData = new ListModel<KvinaNoticeList>();
			resultData.ListData = db49_wowtv.Database.SqlQuery<KvinaNoticeList>(sqlQuery).ToList();

			return resultData;
		}

		//KVINA 공지사항 리스트 linq
		public ListModel<TAB_NOTICE> KvinaNoticeListLinq(IntegratedBoardCondition condition)
		{

			ListModel<TAB_NOTICE> resultData = new ListModel<TAB_NOTICE>();


			var list = db49_wowtv.TAB_NOTICE.AsQueryable();

			list = list.Where(a => a.BCODE.Equals("T07020000"));
			//list = list.OrderByDescending(a => a.SEQ);

			//검색
			if (!string.IsNullOrEmpty(condition.SearchText))
			{
				if (condition.SearchType.Equals("TITLE"))
				{
					list = list.Where(a => a.TITLE.Contains(condition.SearchText));
				}
				else if (condition.SearchType.Equals("CONTENT"))
				{
					list = list.Where(a => a.CONTENT.Contains(condition.SearchText));
				}
				else if (condition.SearchType.Equals("WRITE"))
				{
					list = list.Where(a => a.USER_NAME.Contains(condition.SearchText));
				}
				else
				{
					list = list.Where(a => a.TITLE.Contains(condition.SearchText) || a.CONTENT.Contains(condition.SearchText) || (a.USER_NAME != null && a.USER_NAME.Contains(condition.SearchText)));
				}

			}

			//전체 갯수 가져오기.
			resultData.TotalDataCount = list.Count();

			/*---- 페이징 ----*/
			if (condition.PageSize > -1)
			{
				if (condition.PageSize == 0)
				{
					condition.PageSize = 10;
				}

				list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
			}
			/*---- 페이징 ----*/


			resultData.ListData = list.ToList();

			return resultData;
		}

		////KVINA 공지사항 뷰
		//public TAB_NOTICE KvinaNoticeView(int seq)
		//{
		//	return db49_wowtv.TAB_NOTICE.SingleOrDefault(a => a.SEQ == seq);
		//}

		//KVINA 공지사항 뷰
		public KvinaNoticeView KvinaNoticeView(int seq)
		{

			var resultData = new KvinaNoticeView();
			
			if (!seq.Equals("") )
			{
				string sqlQuery = $@"
					SELECT 	SEQ,BCODE,TITLE,USER_ID,USER_NAME,CONTENT, REF,REF_STEP,REF_LEVEL,READ_CNT,MEMO_CNT,RECOMMEND_CNT,REVERSE_CNT,ASTROMANCY_CNT,VIEW_FLAG,EMOTICON, REG_DATE,GUBUN,SOURCE,EMAIL,PUBLICITY_CNT, (select count(*) from TAB_NOTICE with (nolock) where Bcode = 'T07020000') as ROWCNT

					FROM TAB_NOTICE	 with (nolock)
					WHERE
						BCODE = 'T07020000'
						and SEQ = {seq}		";

				resultData = db49_wowtv.Database.SqlQuery<KvinaNoticeView>(sqlQuery).FirstOrDefault();


			}
			return resultData;
		}

		//KVINA 공지사항 뷰 linq
		public TAB_NOTICE KvinaNoticeViewLinq(int seq)
		{

			var resultData = new TAB_NOTICE();

			var dataSeq = db49_wowtv.TAB_NOTICE.AsNoTracking().Where(a => a.SEQ == seq).SingleOrDefault();
			if (dataSeq != null)
			{

				resultData =  db49_wowtv.TAB_NOTICE.SingleOrDefault(a => a.SEQ == seq);
				
			}
			return resultData;
		}


		//KVINA 신규등록
		public void KvinaNoticeWriteProc(string title, string content, string user_name)
		{
				string executeSql = " insert into TAB_NOTICE( BCODE, TITLE, CONTENT, USER_ID, USER_NAME, REF, REF_STEP, REF_LEVEL, READ_CNT, MEMO_CNT, RECOMMEND_CNT, REVERSE_CNT, ASTROMANCY_CNT, ASTROMANCY_NUM, VIEW_FLAG, EMOTICON, REG_DATE, GUBUN, SOURCE, EMAIL, PUBLICITY_CNT, TOTAL_NUM)  values ('T07020000',  {0}, {1}, '', {2}, 0, 0, 0, 0, 0, 0, 0, 0, 0, 'Y', '', getdate(), '', '', '', 0, 0) ";
				db49_wowtv.Database.ExecuteSqlCommand(executeSql, title, content, user_name);
		}

		//KVINA 신규등록/수정 linq
		public void KvinaNoticeWriteProcLinq(TAB_NOTICE model)
		{

			TAB_NOTICE kvinaSingle = db49_wowtv.TAB_NOTICE.SingleOrDefault(a => a.SEQ == model.SEQ);

			if (kvinaSingle == null) //insert
			{
				model.REG_DATE = DateTime.Now;
				model.BCODE = "T07020000";
				model.READ_CNT = 0;
				model.VIEW_FLAG = "Y";
				db49_wowtv.TAB_NOTICE.Add(model);
			}
			else //update
			{
				kvinaSingle.TITLE = model.TITLE;
				kvinaSingle.CONTENT = model.CONTENT;
				kvinaSingle.USER_NAME = model.USER_NAME;
			}
			//db49_wowtv.Database.Log = s => Debug.WriteLine(s);

			db49_wowtv.SaveChanges();
			
		}


		//KVINA 수정
		public void KvinaNoticeWriteEdit(int seq, string title, string content, string user_name)
		{
			if (!seq.Equals("") )
			{
				string executeSql = " update tab_notice set title = {1}, content= {2}, user_name={3} where seq = {0} ";
				db49_wowtv.Database.ExecuteSqlCommand(executeSql, seq, title, content, user_name);
			}
		}


		//KVINA 수정 linq => KvinaNoticeWriteProcLinq() 로 수정/신규등록을 같이 씀.


		//삭제
		public void KvinaNoticeDelete(int seq)
		{
			if (!seq.Equals("") )
			{
				string executeSql = " delete from TAB_NOTICE where seq = {0} ";
				db49_wowtv.Database.ExecuteSqlCommand(executeSql, seq);
			}
		}

		//삭제 linq
		public void KvinaNoticeDeleteLinq(int seq)
		{
			//var dataSeq = db49_wowtv.TAB_NOTICE.AsNoTracking().Where(a => a.SEQ == seq).SingleOrDefault();
			var dataSeq = db49_wowtv.TAB_NOTICE.Where(a => a.SEQ == seq).SingleOrDefault();
			if (dataSeq != null) //seq가 null이 아니면 삭제함.
			{

				db49_wowtv.TAB_NOTICE.Remove(dataSeq);
				db49_wowtv.SaveChanges();
			}
		}


		////KVINA 페이레터 API TEST
		//public string KvinaPaymentAPI_test2(string strUserID, string strPassword)
		//{
		//	string result_value = string.Empty;
		//	try
		//	{
		//		dbConn = new SqlConnection(strConnectionString);


		//		SqlCommand cmd = new SqlCommand();
		//		cmd.Connection = dbConn;
		//		cmd.CommandType = CommandType.StoredProcedure;
		//		cmd.CommandText = "UP_KVINA_PURCHASE_AR_LST";

		//		cmd.Parameters.Add("@pi_strUserName", SqlDbType.VarChar, 50);
		//		cmd.Parameters.Add("@pi_strMobileNum", SqlDbType.VarChar, 10);

		//		cmd.Parameters.Add("@O_WORK_DATE", SqlDbType.VarChar, 8);
		//		cmd.Parameters.Add("@O_USER_NM", SqlDbType.VarChar, 10);
		//		cmd.Parameters.Add("@O_USER_INFO", SqlDbType.VarChar, 20);
		//		cmd.Parameters.Add("@O_MSG", SqlDbType.VarChar, 255);

		//		cmd.Parameters["@pi_strUserName"].Value = strUserID;
		//		cmd.Parameters["@pi_strMobileNum"].Value = strPassword;

		//		cmd.Parameters["@O_WORK_DATE"].Direction = ParameterDirection.Output;
		//		cmd.Parameters["@O_USER_NM"].Direction = ParameterDirection.Output;
		//		cmd.Parameters["@O_USER_INFO"].Direction = ParameterDirection.Output;
		//		cmd.Parameters["@O_MSG"].Direction = ParameterDirection.Output;

		//		dbConn.Open();
		//		cmd.ExecuteNonQuery();


		//		result_value = Convert.ToString(cmd.Parameters["@O_USER_NM"].Value);


		//	}
		//	catch (Exception e)
		//	{
		//		return e.Message.ToString();
		//	}
		//	finally
		//	{
		//		dbConn.Close();

		//	}
		//	return result_value;
		//}

		//KVINA 페이레터 API
		public int KvinaPaymentAPI_ori()
		{

			ObjectParameter po_DblTotalPurAmt = new ObjectParameter("po_DblTotalPurAmt", typeof(int));
			ObjectParameter po_DblTotalCnlAmt = new ObjectParameter("po_DblTotalCnlAmt", typeof(int));
			ObjectParameter po_DblTotalPurCnt = new ObjectParameter("po_DblTotalPurCnt", typeof(int));
			ObjectParameter po_DblTotalCnlCnt = new ObjectParameter("po_DblTotalCnlCnt", typeof(int));
			ObjectParameter po_intRecordCnt = new ObjectParameter("po_intRecordCnt", typeof(int));
			ObjectParameter po_intRatval = new ObjectParameter("po_intRatval", typeof(int));

			int resultData = db89_WOWTV_BILL_DB.UP_KVINA_PURCHASE_AR_LST("","","",null, null, "20171001","20181231",1000,1,po_DblTotalPurAmt, po_DblTotalCnlAmt, po_DblTotalPurCnt, po_DblTotalCnlCnt, po_intRecordCnt, po_intRatval);



			//형변환 오류남.
			//int DblTotalPurAmt = int.Parse(po_DblTotalPurAmt.Value.ToString());
			//int DblTotalCnlAmt = int.Parse(po_DblTotalCnlAmt.Value.ToString());
			//int DblTotalPurCnt = int.Parse(po_DblTotalPurCnt.Value.ToString());
			//int DblTotalCnlCnt = int.Parse(po_DblTotalCnlCnt.Value.ToString());
			//int intRecordCnt = int.Parse(po_intRecordCnt.Value.ToString());
			//int intRatval = int.Parse(po_intRatval.Value.ToString());

			//형변환 오류안나는 방법
			int DblTotalPurAmt = Convert.ToInt32(po_DblTotalPurAmt.Value);
			int DblTotalCnlAmt = Convert.ToInt32(po_DblTotalCnlAmt.Value);
			int DblTotalPurCnt = Convert.ToInt32(po_DblTotalPurCnt.Value);
			int DblTotalCnlCnt = Convert.ToInt32(po_DblTotalCnlCnt.Value);
			int intRecordCnt = Convert.ToInt32(po_intRecordCnt.Value);
			//int intRatval = Convert.ToInt32(po_intRatval.Value);
			int intRatval = 0;




			return resultData;



			//return DblTotalPurAmt;
			//return DblTotalCnlAmt;
			//return DblTotalPurCnt;
			//return DblTotalCnlCnt;
			//return intRecordCnt;
			//return intRatval;


		}

		//KVINA 페이레터 API
		public int[] KvinaPaymentAPI()
		{
			 
			ObjectParameter po_DblTotalPurAmt = new ObjectParameter("po_DblTotalPurAmt", typeof(int));
			ObjectParameter po_DblTotalCnlAmt = new ObjectParameter("po_DblTotalCnlAmt", typeof(int));
			ObjectParameter po_DblTotalPurCnt = new ObjectParameter("po_DblTotalPurCnt", typeof(int));
			ObjectParameter po_DblTotalCnlCnt = new ObjectParameter("po_DblTotalCnlCnt", typeof(int));
			ObjectParameter po_intRecordCnt = new ObjectParameter("po_intRecordCnt", typeof(int));
			ObjectParameter po_intRatval = new ObjectParameter("po_intRatval", typeof(int));

			db89_WOWTV_BILL_DB.UP_KVINA_PURCHASE_AR_LST("", "", "", null, null, "20171001", DateTime.Now.ToString("yyyyMMdd"), 1000, 1, po_DblTotalPurAmt, po_DblTotalCnlAmt, po_DblTotalPurCnt, po_DblTotalCnlCnt, po_intRecordCnt, po_intRatval);



			//형변환 오류남.
			//int DblTotalPurAmt = int.Parse(po_DblTotalPurAmt.Value.ToString());
			//int DblTotalCnlAmt = int.Parse(po_DblTotalCnlAmt.Value.ToString());
			//int DblTotalPurCnt = int.Parse(po_DblTotalPurCnt.Value.ToString());
			//int DblTotalCnlCnt = int.Parse(po_DblTotalCnlCnt.Value.ToString());
			//int intRecordCnt = int.Parse(po_intRecordCnt.Value.ToString());
			//int intRatval = int.Parse(po_intRatval.Value.ToString());

			//형변환 오류안나는 방법
			int DblTotalPurAmt = Convert.ToInt32(po_DblTotalPurAmt.Value);
			int DblTotalCnlAmt = Convert.ToInt32(po_DblTotalCnlAmt.Value);
			int DblTotalPurCnt = Convert.ToInt32(po_DblTotalPurCnt.Value);
			int DblTotalCnlCnt = Convert.ToInt32(po_DblTotalCnlCnt.Value);
			int intRecordCnt = Convert.ToInt32(po_intRecordCnt.Value);
			//int intRatval = Convert.ToInt32(po_intRatval.Value);
			int intRatval = 0;


			int[] resultData = new int[] { DblTotalPurAmt, DblTotalCnlAmt, DblTotalPurCnt, DblTotalCnlCnt, intRecordCnt, intRatval };

			return resultData;



			//return DblTotalPurAmt;
			//return DblTotalCnlAmt;
			//return DblTotalPurCnt;
			//return DblTotalCnlCnt;
			//return intRecordCnt;
			//return intRatval;


		}

		public int KvinaPaymentAPI_test()
		{
			SqlConnection dbConn = new SqlConnection("server=192.168.128.103/BILLSQL, 5089;uid=abcdev;pwd=Devabc123@!;initial catalog=WOWTV_BILL_DB");
			//SqlConnection dbConn = new SqlConnection("server=192.168.64.89;uid=webprimiddle;pwd=dbdkagh89!;initial catalog=WOWTV_BILL_DB");
			dbConn.Open();

			try
			{
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = dbConn;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "dbo.UP_KVINA_PURCHASE_AR_LST";

				//input
				cmd.Parameters.Add("@pi_strUserName", SqlDbType.VarChar, 50);
				cmd.Parameters.Add("@pi_strMobileNum", SqlDbType.VarChar, 13);
				cmd.Parameters.Add("@pi_strProdName", SqlDbType.VarChar, 20);
				cmd.Parameters.Add("@pi_intStateCode", SqlDbType.TinyInt, 10);
				cmd.Parameters.Add("@pi_intChargeNo", SqlDbType.Int, 10);
				cmd.Parameters.Add("@pi_strStartYMD", SqlDbType.Char, 8);
				cmd.Parameters.Add("@pi_strEndYMD", SqlDbType.Char, 8);
				cmd.Parameters.Add("@pi_intPageSize", SqlDbType.Int, 10);
				cmd.Parameters.Add("@pi_intPageNo", SqlDbType.Int, 10);

				//input값 입력
				cmd.Parameters["@pi_strUserName"].Value = "";
				cmd.Parameters["@pi_strMobileNum"].Value = "";
				cmd.Parameters["@pi_strProdName"].Value = "";
				cmd.Parameters["@pi_intStateCode"].Value = null;
				cmd.Parameters["@pi_intChargeNo"].Value = null;
				cmd.Parameters["@pi_strStartYMD"].Value = "20171001";
				cmd.Parameters["@pi_strEndYMD"].Value = "20180309";
				cmd.Parameters["@pi_intPageSize"].Value = 1000;
				cmd.Parameters["@pi_intPageNo"].Value = 1;
				
				//output
				SqlParameter @po_DblTotalPurAmt = cmd.Parameters.Add("@po_DblTotalPurAmt", SqlDbType.Int);
				@po_DblTotalPurAmt.Direction = ParameterDirection.Output;
				SqlParameter @po_DblTotalCnlAmt = cmd.Parameters.Add("@po_DblTotalCnlAmt", SqlDbType.Int);
				@po_DblTotalCnlAmt.Direction = ParameterDirection.Output;
				SqlParameter @po_DblTotalPurCnt = cmd.Parameters.Add("@po_DblTotalPurCnt", SqlDbType.Int);
				@po_DblTotalPurCnt.Direction = ParameterDirection.Output;
				SqlParameter @po_DblTotalCnlCnt = cmd.Parameters.Add("@po_DblTotalCnlCnt", SqlDbType.Int);
				@po_DblTotalCnlCnt.Direction = ParameterDirection.Output;
				SqlParameter @po_intRecordCnt = cmd.Parameters.Add("@po_intRecordCnt", SqlDbType.Int);
				@po_intRecordCnt.Direction = ParameterDirection.Output;
				SqlParameter @po_intRatval = cmd.Parameters.Add("@po_intRatval", SqlDbType.Int);
				@po_intRatval.Direction = ParameterDirection.Output;

				cmd.ExecuteNonQuery();

				//output값 가져오기
				int DblTotalPurAmt = 0;
				int DblTotalCnlAmt = 0;
				int DblTotalPurCnt = 0;
				int DblTotalCnlCnt = 0;
				int intRecordCnt = 0;
				int intRatval = 0;
				DblTotalPurAmt = Convert.ToInt32(cmd.Parameters["@po_DblTotalPurAmt"].Value);
				DblTotalCnlAmt = Convert.ToInt32(cmd.Parameters["@DblTotalCnlAmt"].Value);
				DblTotalPurCnt = Convert.ToInt32(cmd.Parameters["@DblTotalPurCnt"].Value);
				DblTotalCnlCnt = Convert.ToInt32(cmd.Parameters["@DblTotalCnlCnt"].Value);
				intRecordCnt = Convert.ToInt32(cmd.Parameters["@intRecordCnt"].Value);
				intRatval = Convert.ToInt32(cmd.Parameters["@intRatval"].Value);

				return DblTotalPurAmt;
				return DblTotalCnlAmt;
				return DblTotalPurCnt;
				return DblTotalCnlCnt;
				return intRecordCnt;
				return intRatval;
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.Message);
				return 4;
			}
			finally
			{
				dbConn.Close();
			}
		}





	}



}
