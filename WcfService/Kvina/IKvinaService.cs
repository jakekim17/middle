using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db49.wowtv.Kvina;

namespace Wow.Tv.Middle.WcfService.Kvina
{
	// 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IKvinaService"을 변경할 수 있습니다.
	[ServiceContract]
	public interface IKvinaService
	{
		/*
		[OperationContract]
		void DoWork();
		*/

		// Kvina 공지사항 리스트
		[OperationContract]
		ListModel<KvinaNoticeList> KvinaNoticeList(IntegratedBoardCondition condition);

		// Kvina 공지사항 리스트 linq
		[OperationContract]
		ListModel<TAB_NOTICE> KvinaNoticeListLinq(IntegratedBoardCondition condition);

		//// Kvina 공지사항 뷰
		//[OperationContract]
		//TAB_NOTICE KvinaNoticeView(int seq);

		// Kvina 공지사항 뷰
		[OperationContract]
		KvinaNoticeView KvinaNoticeView(int seq);
		
		// Kvina 공지사항 뷰 linq
		[OperationContract]
		TAB_NOTICE KvinaNoticeViewLinq(int seq);
		
		//KVINA 신규등록
		[OperationContract]
		void KvinaNoticeWriteProc(string title, string content, string user_name);

		//KVINA 신규등록/수정 linq
		[OperationContract]
		void KvinaNoticeWriteProcLinq(TAB_NOTICE model);

		//KVINA 수정
		[OperationContract]
		void KvinaNoticeWriteEdit(int seq, string title, string content, string user_name);

		//삭제
		[OperationContract]
		void KvinaNoticeDelete(int seq);

		//삭제 linq
		[OperationContract]
		void KvinaNoticeDeleteLinq(int seq);

		//KVINA 페이레터 API
		[OperationContract]
		int[] KvinaPaymentAPI();
	}






}
