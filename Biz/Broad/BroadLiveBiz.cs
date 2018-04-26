using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;

namespace Wow.Tv.Middle.Biz.Broad
{
    public class BroadLiveBiz : BaseBiz
    {
        public NTB_BROAD_LIVE GetAt(int broadLiveID)
        {
            return db49_wowtv.NTB_BROAD_LIVE.SingleOrDefault(a => a.BROAD_LIVE_ID == broadLiveID);
        }

        public ListModel<NTB_BROAD_LIVE> SearchList(BroadLiveCondition condition)
        {
            ListModel<NTB_BROAD_LIVE> resultData = new ListModel<NTB_BROAD_LIVE>();

            var list = db49_wowtv.NTB_BROAD_LIVE.Where(a => a.DEL_YN == "N");

            if(condition.BroadStartDate != null)
            {
                list = list.Where(a => a.BROAD_DATE >= condition.BroadStartDate.Value);
            }

            if (condition.BroadEndDate != null)
            {
                list = list.Where(a => a.BROAD_DATE <= condition.BroadEndDate.Value);
            }

            if(String.IsNullOrEmpty(condition.PublishYn) == false)
            {
                list = list.Where(a => a.PUBLISH_YN == condition.PublishYn);
            }

            if (String.IsNullOrEmpty(condition.ProgramName) == false)
            {
                list = list.Where(a => a.PROGRAM_NAME.Contains(condition.ProgramName) == true);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.BROAD_LIVE_ID);

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
        
        public int Save(NTB_BROAD_LIVE model, LoginUser loginUser)
        {
            var prev = GetAt(model.BROAD_LIVE_ID);

            if(prev == null)
            {
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                model.DEL_YN = "N";

                db49_wowtv.NTB_BROAD_LIVE.Add(model);
            }
            else
            {
                prev.PROGRAM_NAME = model.PROGRAM_NAME;
                prev.BROAD_DATE = model.BROAD_DATE;
                prev.START_HOUR = model.START_HOUR;
                prev.START_MINUT = model.START_MINUT;
                prev.END_HOUR = model.END_HOUR;
                prev.END_MINUT = model.END_MINUT;
                prev.YOUTUBE_URL = model.YOUTUBE_URL;
                prev.YOUTUBE_STA_URL = model.YOUTUBE_STA_URL;
                prev.YOUTUBE_COUNT = model.YOUTUBE_COUNT;
                prev.YOUTUBE_PAGE_COUNT = model.YOUTUBE_PAGE_COUNT;
                prev.AFREECA_URL = model.AFREECA_URL;
                prev.AFREECA_STA_URL = model.AFREECA_STA_URL;
                prev.AFREECA_COUNT = model.AFREECA_COUNT;
                prev.AFREECA_PAGE_COUNT = model.AFREECA_PAGE_COUNT;
                prev.KAKAO_URL = model.KAKAO_URL;
                prev.KAKAO_STA_URL = model.KAKAO_STA_URL;
                prev.KAKAO_COUNT = model.KAKAO_COUNT;
                prev.KAKAO_PAGE_COUNT = model.KAKAO_PAGE_COUNT;
                prev.FACEBOOK_URL = model.FACEBOOK_URL;
                prev.FACEBOOK_STA_URL = model.FACEBOOK_STA_URL;
                prev.FACEBOOK_COUNT = model.FACEBOOK_COUNT;
                prev.FACEBOOK_PAGE_COUNT = model.FACEBOOK_PAGE_COUNT;
                prev.PUBLISH_YN = model.PUBLISH_YN;
                prev.REMARK = model.REMARK;
                prev.MAIN_VOD_URL = model.MAIN_VOD_URL;

                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            return model.BROAD_LIVE_ID;
        }

        public void Delete(int broadLiveID)
        {
            var prev = GetAt(broadLiveID);

            prev.DEL_YN = "Y";
            //db49_wowtv.NTB_BROAD_LIVE.Remove(prev);

            db49_wowtv.SaveChanges();
        }


        public void DeleteLiveList(List<int> seqList)
        {
            foreach (int item in seqList)
            {
                Delete(item);
            }
        }
    }
}
