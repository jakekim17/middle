using System;
using System.Linq;
using Wow.Fx;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    /// <summary>
    /// <para>  광고관리 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 오규환</para>
    /// <para>- 최초작성일 : 2017-10-11</para>
    /// <para>- 최종수정자 : ABC솔루션 오규환</para>
    /// <para>- 최종수정일 : 2017-10-11</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-10-11 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class NewsAdBiz : BaseBiz
    {

        #region 하드 코딩 광고
        /// <summary>
        /// 하드 코딩 광고 리스트
        /// </summary>
        /// <returns>ListModel<NTB_HARD_CODING_AD></returns>
        public ListModel<NTB_HARD_CODING_AD> GetHardCodingAdList()
        {
            ListModel<NTB_HARD_CODING_AD> resultData = new ListModel<NTB_HARD_CODING_AD>();
            resultData.ListData = db49_Article.NTB_HARD_CODING_AD.Where(p => p.DEL_YN.Equals("N")).ToList();

            return resultData;
        }

        /// <summary>
        /// 하드 코딩 광고 정보
        /// </summary>
        /// <param name="SEQ">일렬번호</param>
        /// <returns>하드 코딩 광고 내용</returns>
        public NTB_HARD_CODING_AD GetHardCodingAdInfo(int SEQ)
        {
            NTB_HARD_CODING_AD SingleRow = db49_Article.NTB_HARD_CODING_AD.SingleOrDefault(p => p.SEQ.Equals(SEQ));

            return SingleRow;
        }

        /// <summary>
        /// 하드 코딩 광고 등록,수정
        /// </summary>
        /// <param name="hardCodingAdInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        public bool SetHardCodingAd(NTB_HARD_CODING_AD hardCodingAdInfo, LoginUser loginUser)
        {
            bool isSuccess = false;
            try
            {
                var hardCodingAd = db49_Article.NTB_HARD_CODING_AD.SingleOrDefault(p => p.SEQ.Equals(hardCodingAdInfo.SEQ));

                //Insert
                if (hardCodingAd == null)
                {
                    hardCodingAdInfo.REG_ID   = loginUser.LoginId;
                    hardCodingAdInfo.REG_DATE = DateTime.Now;
                    hardCodingAdInfo.MOD_ID   = loginUser.LoginId;
                    hardCodingAdInfo.MOD_DATE = DateTime.Now;
                    hardCodingAdInfo.DEL_YN   = "N";

                    db49_Article.NTB_HARD_CODING_AD.Add(hardCodingAdInfo);
                }
                //Upate
                else
                {
                    hardCodingAd.AD_GUBUN   = hardCodingAdInfo.AD_GUBUN;
                    hardCodingAd.AD_TITLE   = hardCodingAdInfo.AD_TITLE;
                    hardCodingAd.AD_CONTENT = hardCodingAdInfo.AD_CONTENT;
                    hardCodingAd.MOD_ID     = loginUser.LoginId;
                    hardCodingAd.MOD_DATE   = DateTime.Now;
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                WowLog.Write(e.Message);

            }

            return isSuccess;
        }

        #endregion




    }
}
