using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db51.contents;
using Wow.Tv.Middle.Model.Db51.contents.Balance;

namespace Wow.Tv.Middle.Biz.IRCenter
{
    public class BalanceBiz : BaseBiz
    {
        public BalanceModel<NTB_FINANCE_STATUS> GetList(BalanceCondition condition)
        {
            var resultData = new BalanceModel<NTB_FINANCE_STATUS>();

            var list = Db51_contents.NTB_FINANCE_STATUS.Where(a => a.DEL_YN.Equals("N")).AsQueryable();//항목리스트

            if (!String.IsNullOrEmpty(condition.Year))
            {
                list = list.Where(a => a.YEAR.Equals(condition.Year));
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a => a.NO).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }


            resultData.ListData = list.ToList();
            resultData.YearList = Db51_contents.NTB_FINANCE_STATUS.Select(a => new BalanceYear { Year = a.YEAR }).Distinct().OrderByDescending(a => a.Year).ToList();//년도리스트(DB)
            //resultData.MonthList = Db51_contents.NTB_FINANCE_STATUS.Select(a => new BalanceYear { Month = a.month }).Distinct().ToList();//분기리스트(DB)

            return resultData;
        }


        public void Delete(int[] deleteList)
        {
            foreach (var index in deleteList)
            {
                var data = Db51_contents.NTB_FINANCE_STATUS.SingleOrDefault((a => a.NO.Equals(index)));

                if (data != null)
                {
                    data.DEL_YN = "Y";    
                }
                Db51_contents.SaveChanges();

            }
        }

        public int Save(NTB_FINANCE_STATUS model, LoginUser loginUser)
        {
            var data = GetData(model.NO);

            if (data != null)//데이터 존재한다면 업데이트 
            {
               
                data.YEAR = model.YEAR;
                data.CURNT_ASSET = model.CURNT_ASSET;
                data.NON_CURNT_ASSET = model.NON_CURNT_ASSET;
                data.TOTAL_ASSET = model.TOTAL_ASSET;
                data.CURNT_LIABILITES = model.CURNT_LIABILITES;
                data.NON_CURNT_LIABILITES = model.NON_CURNT_LIABILITES;
                data.TOTAL_LIABILITES = model.TOTAL_LIABILITES;
                data.CAPITAL_STOCK = model.CAPITAL_STOCK;
                data.TOTAL_CAPITAL = model.TOTAL_CAPITAL;
                data.OPERATING_PROFIT = model.OPERATING_PROFIT;
                data.NET_INCOME = model.NET_INCOME;
                data.REVENUE = model.REVENUE;
                data.FLAG = model.FLAG;
                data.MOD_DATE = DateTime.Now;//수정일
            }
            else
            {//인서트
                if(Db51_contents.NTB_FINANCE_STATUS.FirstOrDefault() == null)
                {
                    var cnt = 1;
                    model.NO = cnt;
                }
                else
                {
                    model.NO = Db51_contents.NTB_FINANCE_STATUS.OrderByDescending(a => a.NO).First().NO + 1;
                }
                model.DEL_YN = "N";
                model.REG_ID = loginUser.UserName;
                model.REG_DATE = DateTime.Now;
                Db51_contents.NTB_FINANCE_STATUS.Add(model);

            }
            Db51_contents.SaveChanges();

            return model.NO;
        }


        public NTB_FINANCE_STATUS GetData(int no)
        {
            return Db51_contents.NTB_FINANCE_STATUS.SingleOrDefault(a => a.NO.Equals(no));
        }

        public BalanceModel<NTB_FINANCE_STATUS> GetFrontData(String year)
        {
            var yearVal = Int32.Parse(year);

            var resultData = new BalanceModel<NTB_FINANCE_STATUS>();

            if (year.Equals("0"))
            {
                var defaultData = Db51_contents.NTB_FINANCE_STATUS.OrderByDescending(a => a.YEAR).FirstOrDefault();

                if (defaultData != null)
                {
                    yearVal = Int32.Parse(defaultData.YEAR);
                }
            }
            

            var data1 = Db51_contents.NTB_FINANCE_STATUS.Where(a => a.DEL_YN.Equals("N") && a.FLAG.Equals("Y")).SingleOrDefault(a => a.YEAR.Equals(yearVal.ToString())); ;
            var data2 = Db51_contents.NTB_FINANCE_STATUS.Where(a => a.DEL_YN.Equals("N") && a.FLAG.Equals("Y")).SingleOrDefault(a => a.YEAR.Equals((yearVal - 1).ToString()));
            var data3 = Db51_contents.NTB_FINANCE_STATUS.Where(a => a.DEL_YN.Equals("N") && a.FLAG.Equals("Y")).SingleOrDefault(a => a.YEAR.Equals((yearVal - 2).ToString()));

            resultData.YearList = Db51_contents.NTB_FINANCE_STATUS.Select(a => new BalanceYear { Year = a.YEAR }).Distinct().OrderByDescending(a => a.Year).ToList();//년도리스트(DB)
            resultData.AccountData1 = data1 == null ? new NTB_FINANCE_STATUS() : data1;
            resultData.AccountData2 = data2 == null ? new NTB_FINANCE_STATUS() : data2;
            resultData.AccountData3 = data3 == null ? new NTB_FINANCE_STATUS() : data3;

            
            
            
            return resultData;
        }
    }
}
