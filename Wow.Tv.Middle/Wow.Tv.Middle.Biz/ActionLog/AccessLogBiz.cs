using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Biz.ActionLog
{
    public class AccessLogBiz : BaseBiz
    {
        public void Regist(string ipAddress, int menuSeq, string accessCode, string userId, string url)
        {
            NTB_ACCESS_LOG newLog = new NTB_ACCESS_LOG();
            newLog.IP = ipAddress;
            newLog.MENU_SEQ = menuSeq;
            newLog.ACCESS_CODE = accessCode;
            newLog.REG_ID = userId;
            newLog.URL = url;
            newLog.REG_DATE = DateTime.Now;
            db49_wowtv.NTB_ACCESS_LOG.Add(newLog);
            db49_wowtv.SaveChanges();
        }
    }
}
