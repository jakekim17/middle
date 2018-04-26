using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Recruit;

namespace Wow.Tv.Middle.Biz.Recruit
{
    public class RecruitBiz : BaseBiz
    {
        /// <summary>
        /// 지원자 정보 조회리스트
        /// </summary>
        /// <param name="condition"></param>
        public List<NUP_RECRUIT_SELECT_Result> SearchList(RecruitCondition condition)
        {
            condition.Page = condition.CurrentIndex == 0 ? 1 : (condition.CurrentIndex / condition.PageSize) + 1;
            var result = db89_wowbill.NUP_RECRUIT_SELECT(condition.SearchName, condition.SearchSsno, condition.SearchSeq, condition.PageSize, condition.Page, condition.SearchPassword).ToList();

            if (result.Count > 0)
            {
                foreach(var item in result)
                {
                    result[result.IndexOf(item)].NAME = item.NAME.Substring(0, 1) + "**";
                }
            }
            return result;
        }

        /// <summary>
        /// 지원자 정보 상세 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public NUP_RECRUIT_SELECT_Result GetApplicantInfo(RecruitCondition condition)
        {
            condition.PageSize = 1;
            var result = db89_wowbill.NUP_RECRUIT_SELECT(condition.SearchName, condition.SearchSsno, condition.SearchSeq, condition.PageSize, condition.Page, condition.SearchPassword).FirstOrDefault();
            if(result != null && condition.SearchSsno == null)
            {
                result.NAME = result.NAME.Substring(0, 1) + "**";
            }
            return result;
        }

        /// <summary>
        /// 조회수 증가
        /// </summary>
        public void IncreaseViewCnt(int seq)
        {
            var data = db89_wowbill.tblRecruit.SingleOrDefault(a => a.Seq.Equals(seq));
            if(data != null)
            {
                data.ViewCnt = data.ViewCnt + 1;
                db89_wowbill.SaveChanges();
            }
        }

        /// <summary>
        /// 지원 저장
        /// </summary>
        /// <param name="model"></param>
        public void SaveRecruit(tblRecruit model)
        {
            db89_wowbill.NUP_RECRUIT_INSERT(model.Type, model.Name, model.Ssno, model.Img, model.JobApplication, model.Password, model.Agreement, null);
        }
    }
}

