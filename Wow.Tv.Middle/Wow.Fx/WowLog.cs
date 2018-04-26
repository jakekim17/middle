using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wow.Fx
{
    public class WowLog
    {
        public static void Write(string logMessage, Exception ex)
        {
            string exceptionMessage = "\r\n[Exception]\r\nMessage: " + ex.Message + "\r\nStackTrace: " + ex.StackTrace + "\r\nInnerExceptionMessage: " + ex.InnerException?.Message;
            if (logMessage == null) logMessage = "";

            Write(logMessage + exceptionMessage);
        }

        public static void Write(string logMessage, HttpRequestBase Request, Exception ex)
        {
            string exceptionMessage = "\r\n[Exception]\r\nMessage: " + ex.Message + "\r\nStackTrace: " + ex.StackTrace + "\r\nInnerExceptionMessage: " + ex.InnerException?.Message;
            if (logMessage == null) logMessage = "";

            string requestUrl = "\r\n[RequestUrl]\r\n" + Request.Url?.AbsoluteUri;

            string requestFormContents = "";
            foreach (string key in Request.Form?.Keys)
            {
                requestFormContents += ", " + key + ":" + Request.Form[key];
            }
            if (requestFormContents.Length > 2)
            {
                requestFormContents = "\r\n[RequestForm]\r\n" + requestFormContents.Substring(2);
            }

            Write(logMessage + exceptionMessage + requestUrl + requestFormContents);
        }

        private static object lockObject = new object();
        public static void Write(string logMessage)
        {

            FileStream oFS = null;
            StreamWriter oSW = null;
            try
            {
                logMessage = string.Format("{0}[{2}]{3}{0}{1}{0}"
                                           , Environment.NewLine
                                           , logMessage
                                           , DateTime.Now.ToString()
                                           ,
                                           "----------------------------------------------------------------------------------------------------");

                string FILE_LOG_ROOT_PATH = "C:\\WowLog\\";
                if (!Directory.Exists(FILE_LOG_ROOT_PATH))
                {
                    Directory.CreateDirectory(FILE_LOG_ROOT_PATH);
                }

                string preName1 = System.Web.HttpContext.Current.Request.Url.Host;
                int preName2 = System.Web.HttpContext.Current.Request.Url.Port;

                lock (lockObject)
                {
                    //파일스트림 객체 초기화
                    using (oFS = new FileStream(
                                     //파일 이름 지정
                                     FILE_LOG_ROOT_PATH + "\\" + preName1 + "-" + preName2 + "-Log-" + DateTime.Now.ToString("yyyyMMdd") + ".txt",
                                     //파일이 있으면 열고, 없으면 만든다
                                     FileMode.OpenOrCreate,
                                     //파일을 읽기,쓰기 모드로 연다
                                     FileAccess.ReadWrite))
                    {
                        //스트림라이터 객체 초기화
                        using (oSW = new StreamWriter(oFS))
                        {
                            //마지막 부분을 찾는다.
                            oSW.BaseStream.Seek(0, SeekOrigin.End);

                            oSW.Write(logMessage);

                            //반드시 flush를 해야, 메모리에 있는 내용을 파일에 기록한다.
                            //flush하지 않으면 파일을 잠그기 때문에 다른 프로세스가 이 파일에 접근할 수 없다
                            oSW.Flush();
                            oSW.Close();
                        }
                    }
                }
            }
            finally
            {
            }

            //string filePath = "C:\\WowLog\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //System.IO.File.AppendAllText(filePath, DateTime.Now.ToDate() + ":" + value + "\r\n");
        }


        public static void TempWrite(string msg)
        {
            System.IO.File.AppendAllText("D:\\TempLog\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", DateTime.Now.ToString("HH:mm:ss") + " => " + msg + "\r\n");

        }
    }
}
