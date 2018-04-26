using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Admin;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db49.wowtv.Kvina;

namespace Wow.Tv.Middle.WcfService.Kvina
{
	// 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "KvinaService"을 변경할 수 있습니다.
	// 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 KvinaService.svc나 KvinaService.svc.cs를 선택하고 디버깅을 시작하십시오.
	public class KvinaService : IKvinaService
	{




		// Kvina 공지사항 리스트
		public ListModel<KvinaNoticeList> KvinaNoticeList(IntegratedBoardCondition condition)
		{
			return new KvinaBiz().KvinaNoticeList(condition);
		}

		// Kvina 공지사항 리스트 linq
		public ListModel<TAB_NOTICE> KvinaNoticeListLinq(IntegratedBoardCondition condition)
		{
			return new KvinaBiz().KvinaNoticeListLinq(condition);
		}

		//// Kvina 공지사항 뷰
		//public TAB_NOTICE KvinaNoticeView(int seq)
		//{
		//	return new KvinaBiz().KvinaNoticeView(seq);
		//}

		// Kvina 공지사항 뷰
		public KvinaNoticeView KvinaNoticeView(int seq)
		{
			return new KvinaBiz().KvinaNoticeView(seq);
		}

		// Kvina 공지사항 뷰 linq
		public TAB_NOTICE KvinaNoticeViewLinq(int seq)
		{
			return new KvinaBiz().KvinaNoticeViewLinq(seq);
		}

		//KVINA 신규등록
		public void KvinaNoticeWriteProc(string title, string content, string user_name)
		{
			new KvinaBiz().KvinaNoticeWriteProc(title, content, user_name);
		}

		//KVINA 신규등록/수정 linq
		public void KvinaNoticeWriteProcLinq(TAB_NOTICE model)
		{
			new KvinaBiz().KvinaNoticeWriteProcLinq(model);
		}

		//KVINA 수정
		public void KvinaNoticeWriteEdit(int seq, string title, string content, string user_name)
		{
			new KvinaBiz().KvinaNoticeWriteEdit(seq, title, content, user_name);
		}

		//삭제
		public void KvinaNoticeDelete(int seq)
		{
			new KvinaBiz().KvinaNoticeDelete(seq);
		}

		//삭제 linq
		public void KvinaNoticeDeleteLinq(int seq)
		{
			new KvinaBiz().KvinaNoticeDeleteLinq(seq);
		}

		//KVINA 페이레터 API
		public int[] KvinaPaymentAPI()
		{
			return new KvinaBiz().KvinaPaymentAPI();
		}





	}
}
