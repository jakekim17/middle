using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db51.ARSsms;
using Wow.Tv.Middle.Model.Db51.WOWMMS;

namespace Wow.Tv.Middle.Biz.Component
{
    public class SmsBiz : BaseBiz
    {
        public void SendSms(string receiveNo, string callbackNo, string message, string transportId, string etc1)
        {
            _SendSms(receiveNo, callbackNo, message, transportId, etc1, null, null, null, null, null, true);
        }
        public void SendSms(string receiveNo, string callbackNo, string message, string transportId, string etc1, bool transMms)
        {
            _SendSms(receiveNo, callbackNo, message, transportId, etc1, null, null, null, null, null, transMms);
        }

        /// <summary>
        /// SMS 전송
        /// </summary>
        /// <param name="phone">받는 번호(대쉬-는 알아서 처리됨)</param>
        /// <param name="callBack">보낸는 번호(대쉬-는 알아서 처리됨)</param>
        /// <param name="msg">메세지</param>
        /// <param name="transMms">88 Byte 초과 시 MMS 전환여부</param>
        /// <param name="transportId">처리 아이디</param>
        /// <param name="etc1">추가정보1</param>
        /// <param name="etc2">추가정보2</param>
        /// <param name="etc3">추가정보3</param>
        /// <param name="etc4">추가정보4</param>
        /// <param name="etc5">추가정보5</param>
        /// <param name="etc6">추가정보6</param>
        private void _SendSms(string receiveNo, string callbackNo, string message, string transportId, string etc1, string etc2, string etc3, string etc4, string etc5, string etc6, bool transMms)
        {
            bool sendSms = true;
            System.Text.Encoding encoder = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            int byteCount = encoder.GetByteCount(message);
            string sendMessage = message;

            if (transMms == true)
            {
                if (byteCount > 88)
                {
                    sendSms = false;
                    SendMms(receiveNo, callbackNo, null, message, etc1, etc2, etc3);
                }
            }
            else
            {
                if (byteCount > 88)
                {
                    byte[] buf = encoder.GetBytes(message);
                    sendMessage = encoder.GetString(buf, 0, 88);
                }
            }

            if (sendSms == true)
            {
                SC_TRAN scTran = new SC_TRAN();

                scTran.TR_SENDDATE = DateTime.Now;
                scTran.TR_SENDSTAT = "0";
                scTran.TR_RSLTSTAT = "00";
                scTran.TR_PHONE = receiveNo.Replace("-", "");
                scTran.TR_CALLBACK = callbackNo.Replace("-", "");
                scTran.TR_MSG = sendMessage;
                scTran.TR_ID = transportId;

                scTran.TR_ETC1 = etc1;
                scTran.TR_ETC2 = etc2;
                scTran.TR_ETC3 = etc3;
                scTran.TR_ETC4 = etc4;
                scTran.TR_ETC5 = etc5;
                scTran.TR_ETC6 = etc6;

                db51_ARSsms.SC_TRAN.Add(scTran);
                db51_ARSsms.SaveChanges();
            }
        }

        public void SendMms(string receiveNo, string callbackNo, string subject, string message, string etc1 = null, string etc2 = null, string etc3 = null)
        {
            System.Text.Encoding encoder = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            int byteCount = encoder.GetByteCount(message);

            mms_msg msg = new mms_msg();
            msg.REQDATE = DateTime.Now;
            msg.PHONE = receiveNo.Replace("-", "");
            msg.CALLBACK = callbackNo.Replace("-", "");
            msg.SUBJECT = subject;
            msg.MSG = message;
            msg.FILE_CNT = 0;
            msg.ETC1 = etc1;
            msg.ETC2 = etc2;
            msg.ETC3 = etc3;

            msg.REQDATE = DateTime.Now;
            msg.FILE_CNT_REAL = 0;
            msg.STATUS = "2";
            msg.EXPIRETIME = "43200";
            msg.REPCNT = 0;
            msg.TYPE = "0";

            Db51_WOWMMS.mms_msg.Add(msg);
            Db51_WOWMMS.SaveChanges();
        }
    }
}
