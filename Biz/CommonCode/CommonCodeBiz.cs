using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.CommonCode;

namespace Wow.Tv.Middle.Biz.CommonCode
{
    public class CommonCodeBiz : BaseBiz
    {
        /// <summary>
        /// 공통코드 조회
        /// </summary>
        /// <param name="condition">조회조건</param>
        /// <returns></returns>
        public ListModel<NTB_COMMON_CODE> SearchList(CommonCodeCondition condition)
        {
            ListModel<NTB_COMMON_CODE> resultData = new ListModel<NTB_COMMON_CODE>();

            var list = db49_wowtv.NTB_COMMON_CODE.Where(a => a.DEL_YN == "N");

            if (String.IsNullOrEmpty(condition.UpCommonCode) == true)
            {
                list = list.Where(a => a.UP_COMMON_CODE == null);
            }
            else
            {
                list = list.Where(a => a.UP_COMMON_CODE == condition.UpCommonCode);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.SORT_ORDER);

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
        /// 공통코드 가져오기
        /// </summary>
        /// <param name="commonCode">코드</param>
        /// <returns></returns>
        public NTB_COMMON_CODE GetAt(string commonCode)
        {
            return db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.COMMON_CODE == commonCode);
        }


        public NTB_COMMON_CODE GetAt(string upCommonCode, string codeValue1)
        {
            return db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE == upCommonCode && a.CODE_VALUE1 == codeValue1).OrderBy(a => a.COMMON_CODE).FirstOrDefault();
        }


        /// <summary>
        /// 공통코드 저장
        /// </summary>
        /// <param name="model">저장할 객체</param>
        /// <param name="loginUser">작업자정보</param>
        public string Save(NTB_COMMON_CODE model, LoginUser loginUser)
        {
            NTB_COMMON_CODE data = GetAt(model.COMMON_CODE);
            if (data == null)
            {
                StringInt stringInit = MakeNewCode(model.UP_COMMON_CODE);
                model.COMMON_CODE = stringInit.StringValue;
                model.SORT_ORDER = stringInit.IntValue;

                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_COMMON_CODE.Add(model);
            }
            else
            {
                data.UP_COMMON_CODE = model.UP_COMMON_CODE;
                data.CODE_NAME = model.CODE_NAME;
                data.SORT_ORDER = model.SORT_ORDER;
                data.CODE_VALUE1 = model.CODE_VALUE1;
                data.CODE_VALUE2 = model.CODE_VALUE2;
                data.CODE_VALUE3 = model.CODE_VALUE3;
                data.CODE_VALUE4 = model.CODE_VALUE4;
                data.CODE_VALUE5 = model.CODE_VALUE5;

                data.ACTIVE_YN = model.ACTIVE_YN;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            return model.COMMON_CODE;
        }


        /// <summary>
        /// 공통코드 삭제(실제는 삭제하지 않고 플래그 처리)
        /// </summary>
        /// <param name="commonCode">코드</param>
        /// <param name="loginUser">작업자정보</param>
        public void Delete(string commonCode, LoginUser loginUser)
        {
            NTB_COMMON_CODE data = GetAt(commonCode);
            if (data != null)
            {
                data.DEL_YN = "Y";
                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;

                db49_wowtv.SaveChanges();
            }
        }



        private StringInt MakeNewCode(string upCode)
        {
            StringInt result = new StringInt();
            string newCode = "";
            int newOrder = 1;

            // =======================================
            // 1. 코드는 무조건 9자리가는 전제로....
            // 2. 뎁스는 3뎁스까지만 Fix
            // =======================================


            // 상위코드가 없으면 1 뎁스로 생성
            if (String.IsNullOrEmpty(upCode) == true)
            {
                var lastCode = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE == null).OrderByDescending(a => a.COMMON_CODE).FirstOrDefault();
                int temp = int.Parse(lastCode.COMMON_CODE.Substring(0, 3));
                temp = temp + 1;
                newCode = temp.ToString("000") + "000000";

                newOrder = lastCode.SORT_ORDER + 1;
            }
            else
            {
                string part1 = upCode.Substring(0, 3);
                string part2 = upCode.Substring(3, 3);
                string part3 = upCode.Substring(6, 3);

                // 상위의 가운데 부분이 000 이면 2뎁스로 생성
                if(part2 == "000")
                {
                    int temp = 0;
                    int tempOrder = 0;
                    var lastCode = db49_wowtv.NTB_COMMON_CODE.Where(a => a.COMMON_CODE.Substring(0, 3) == part1 && a.COMMON_CODE.Substring(3, 3) != "000").OrderByDescending(a => a.COMMON_CODE).FirstOrDefault();
                    if (lastCode != null)
                    {
                        temp = int.Parse(lastCode.COMMON_CODE.Substring(3, 3));
                        tempOrder = lastCode.SORT_ORDER;
                    }
                    temp = temp + 1;
                    newCode = part1 + temp.ToString("000") + "000";

                    newOrder = tempOrder + 1;
                }
                // 상위의 끝 부분이 000 이면 3뎁스로 생성
                else if (part3 == "000")
                {
                    int temp = 0;
                    int tempOrder = 0;
                    var lastCode = db49_wowtv.NTB_COMMON_CODE.Where(a => a.COMMON_CODE.Substring(0, 6) == part1 + part2 && a.COMMON_CODE.Substring(6, 3) != "000").OrderByDescending(a => a.COMMON_CODE).FirstOrDefault();
                    if (lastCode != null)
                    {
                        temp = int.Parse(lastCode.COMMON_CODE.Substring(6, 3));
                        tempOrder = lastCode.SORT_ORDER;
                    }
                    temp = temp + 1;
                    newCode = part1 + part2 + temp.ToString("000");

                    newOrder = tempOrder + 1;
                }
            }

            result.StringValue = newCode;
            result.IntValue = newOrder;

            return result;
        }
    }
}
