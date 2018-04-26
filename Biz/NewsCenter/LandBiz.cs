using System;
using System.Linq;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class LandBiz : BaseBiz
    {
        public NTB_POST GetMapAddress(string Sido, string Gugun)
        {
            NTB_POST data = new NTB_POST();

            data.Sido = db49_Article.NTB_POST.Select(a => a.sido).Distinct().OrderBy(a=>a).ToList(); // default 8도 가져오기

            if (!String.IsNullOrEmpty(Sido) && !Sido.Trim().Equals("세종"))
            {
                data.Gugun = db49_Article.NTB_POST.Where(a => a.sido.Equals(Sido)).Select(a => a.gugun).Distinct().OrderBy(a => a).ToList(); //8도에 맞는 구/군 가져오기

                if (!String.IsNullOrEmpty(Gugun)) //gugun 선택했다면
                {
                    data.Dong = db49_Article.NTB_POST.Where(a => a.sido.Equals(Sido) && a.gugun.Equals(Gugun)).OrderBy(a => a.dong).Select(a => a.dong).Distinct().OrderBy(a => a).ToList(); // 동 가져오기
                }
            }
            else if (!String.IsNullOrEmpty(Sido) && Sido.Trim().Equals("세종"))
            {
                data.Dong = db49_Article.NTB_POST.Where(a => a.sido.Equals(Sido)).OrderBy(a => a.dong).Select(a => a.dong).Distinct().OrderBy(a => a).ToList(); // 동 가져오기
            }

            return data;
        }
    }
}
