
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.IRClub;

namespace Wow.Tv.Middle.Biz.IRCenter
{
    public class IRClubBiz : BaseBiz
    {
        public ListModel<TAB_IR_CLUB_2010> GetList(IRClubCondition condition)
        {
            var resultData = new ListModel<TAB_IR_CLUB_2010>();
            var list = db49_wownet.TAB_IR_CLUB_2010.OrderBy(a => a.COMPANY_NAME).AsQueryable();

            if (!String.IsNullOrEmpty(condition.industryVolume))
            {
                list = list.Where(a => a.INDUSTRY_VOLUME.Equals(condition.industryVolume));
            }

            if (!String.IsNullOrEmpty(condition.regType))
            {
                list = list.Where(a => a.REG_TYPE.Equals(condition.regType));
            }

            if (!String.IsNullOrEmpty(condition.viewFlag))
            {
                list = list.Where(a => a.VIEW_FLAG.Equals(condition.viewFlag));
            }

            if (!String.IsNullOrEmpty(condition.searchText))
            {
                switch (condition.searchType)
                {
                    case "all":
                        list = list.Where(a => a.COMPANY_NAME.Contains(condition.searchText) || a.HOMEPAGE.Contains(condition.searchText));
                        break;
                    case "companyName":
                        list = list.Where(a => a.COMPANY_NAME.Contains(condition.searchText));
                        break;
                    case "homepage":
                        list = list.Where(a => a.HOMEPAGE.Contains(condition.searchText));
                        break;
                }
            }

            resultData.TotalDataCount = list.Count();

            if(condition.PageSize > -1)
            {
                if(condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            return resultData;
        }

        public int Save(TAB_IR_CLUB_2010 model)
        {
            model.SCODE = Regex.Replace(model.SCODE, @"\D", "");
            var data = GetData(model.SEQ);
            if(data != null)
            {
                data.COMPANY_NAME = model.COMPANY_NAME;
                data.COMPANY_PHONE = model.COMPANY_PHONE;
                data.HOMEPAGE = model.HOMEPAGE;
                data.START_DATE = model.START_DATE;
                data.END_DATE = model.END_DATE;
                data.APPROVAL_FLAG = model.APPROVAL_FLAG;
                data.INDUSTRY_NAME = model.INDUSTRY_NAME;
                data.INDUSTRY_VOLUME = model.INDUSTRY_VOLUME;
                data.SCODE = model.SCODE;
                data.REG_TYPE = model.REG_TYPE;
                data.VIEW_FLAG = model.VIEW_FLAG;
                data.MOD_DATE = DateTime.Now;
                if(model.COMPANY_LOGO != null)
                {
                    data.COMPANY_LOGO = model.COMPANY_LOGO;
                }
            }
            else
            {
                model.REG_DATE = DateTime.Now;
                db49_wownet.TAB_IR_CLUB_2010.Add(model);
            }
            db49_wownet.SaveChanges();

            return model.SEQ;
        }

        public void Delete(int[] deleteList)
        {
            if(deleteList != null)
            {
                var data = new TAB_IR_CLUB_2010();
                foreach(var seq in deleteList)
                {
                    data = GetData(seq);
                    if(data != null)
                    {
                        db49_wownet.TAB_IR_CLUB_2010.Remove(data);
                    }
                }
                db49_wownet.SaveChanges();
            }
        }

        public TAB_IR_CLUB_2010 GetData(int seq)
        {
            return db49_wownet.TAB_IR_CLUB_2010.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

        public List<view_AllStockCode> GetStockCode(string searchText)
        {
            var list = db22_stock.view_AllStockCode.OrderBy(a => a.SName).AsQueryable();

            if (!String.IsNullOrEmpty(searchText))
            {
                switch (searchText)
                {
                    case "가":
                        list = list.Where(a => a.SName.CompareTo("ㄱ") >= 0 && a.SName.CompareTo("ㄴ") < 0);
                        break;
                    case "나":
                        list = list.Where(a => a.SName.CompareTo("ㄴ") >= 0 && a.SName.CompareTo("ㄷ") < 0);
                        break;
                    case "다":
                        list = list.Where(a => a.SName.CompareTo("ㄷ") >= 0 && a.SName.CompareTo("ㄹ") < 0);
                        break;
                    case "라":
                        list = list.Where(a => a.SName.CompareTo("ㄹ") >= 0 && a.SName.CompareTo("ㅁ") < 0);
                        break;
                    case "마":
                        list = list.Where(a => a.SName.CompareTo("ㅁ") >= 0 && a.SName.CompareTo("ㅂ") < 0);
                        break;
                    case "바":
                        list = list.Where(a => a.SName.CompareTo("ㅂ") >= 0 && a.SName.CompareTo("ㅅ") < 0);
                        break;
                    case "사":
                        list = list.Where(a => a.SName.CompareTo("ㅅ") >= 0 && a.SName.CompareTo("ㅇ") < 0);
                        break;
                    case "아":
                        list = list.Where(a => a.SName.CompareTo("ㅇ") >= 0 && a.SName.CompareTo("ㅈ") < 0);
                        break;
                    case "자":
                        list = list.Where(a => a.SName.CompareTo("ㅈ") >= 0 && a.SName.CompareTo("ㅊ") < 0);
                        break;
                    case "차":
                        list = list.Where(a => a.SName.CompareTo("ㅊ") >= 0 && a.SName.CompareTo("ㅋ") < 0);
                        break;
                    case "카":
                        list = list.Where(a => a.SName.CompareTo("ㅋ") >= 0 && a.SName.CompareTo("ㅌ") < 0);
                        break;
                    case "타":
                        list = list.Where(a => a.SName.CompareTo("ㅌ") >= 0 && a.SName.CompareTo("ㅍ") < 0);
                        break;
                    case "파":
                        list = list.Where(a => a.SName.CompareTo("ㅍ") >= 0 && a.SName.CompareTo("ㅎ") < 0);
                        break;
                    case "하":
                        list = list.Where(a => a.SName.CompareTo("ㅎ") >= 0 );
                        break;
                    case "A~Z":
                        list = list.Where(a => a.SName.CompareTo("A") >= 0 && a.SName.CompareTo("Z") < 0);
                        break;
                    default:
                        list = list.Where(a => a.SName.Contains(searchText));
                        break;
                }
            }

            return list.ToList();
        }
    }
}
