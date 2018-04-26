using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Wow.Fx
{
    public class CdnUploadHandler
    {
        public class CdnResultModel<T>
        {
            public T Response { get; set; }
        }

        public class AuthResultModel
        {
            public string result { get; set; }
            public string key { get; set; }
            public string error { get; set; }
        }
        public class FileUploadResultModel
        {
            public FileList list { get; set; }
            public Cnt cnt { get; set; }
        }

        public class FileList
        {
            public string[] success { get; set; }
            public string[] fail { get; set; }
        }
        public class Cnt
        {
            public int success { get; set; }
            public int fail { get; set; }
        }



        public AuthResultModel GetAuth()
        {
            AuthResultModel authResultModel = new AuthResultModel();

            string user = System.Configuration.ConfigurationManager.AppSettings["Cdn_UserId"];// "wowtv@kinxcdn.com";
            string pwd = System.Configuration.ConfigurationManager.AppSettings["Cdn_Pwd"]; // "wowtv6676";
            string stoid = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stoid"]; //"wowtvupload";
            string stopwd = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stopwd"]; //"Uploadwowtv123@!";

            //string str = "{\"Response\":{\"result\":\"OK\",\"key\":\"3A403782393E337C397E393E34823F363A35403236393837\",\"error\":\"\"}}";

            String callUrl = String.Format("https://stats.kinxcdn.com/api/auth?user={0}&pwd={1}&stoid={2}&stopwd={3}", user, pwd, stoid, stopwd);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(callUrl);
            myReq.Method = "GET";
            //HttpWebResponse 객체 받아옴
            HttpWebResponse wRes = (HttpWebResponse)myReq.GetResponse();
            // Response의 결과를 스트림을 생성합니다.
            Stream respGetStream = wRes.GetResponseStream();
            StreamReader readerGet = new StreamReader(respGetStream, Encoding.UTF8);
            // 생성한 스트림으로부터 string으로 변환합니다.
            string resultGet = readerGet.ReadToEnd();

            CdnResultModel<AuthResultModel> pObj = JsonConvert.DeserializeObject<CdnResultModel<AuthResultModel>>(resultGet);

            return pObj.Response;
        }


        public FileUploadResultModel FileUpload(string cdnUpLoadPath, string fileName, Stream stream)
        {
            // ==================================
            // ----------------------------------
            string strFTP_URL = @"ftp://wownetup.xst.kinxcdn.com/";

            cdnUpLoadPath = cdnUpLoadPath.Replace("\\", "/");

            //MakeDirectory(strFTP_URL);
            string prevDirectory = strFTP_URL;
            string tempDirectory = strFTP_URL;
            string[] directoryList = cdnUpLoadPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in directoryList)
            {
                tempDirectory += item + "/";
                if (IsDirectoryExist(prevDirectory, item) == false)
                {
                    MakeDirectory(tempDirectory);
                }
                prevDirectory = tempDirectory;
            }

            // ----------------------------------
            // ==================================


            FileUploadResultModel fileUploadResultModel = null;

            cdnUpLoadPath = cdnUpLoadPath.Replace("\\", "/");

            AuthResultModel authResultModel = GetAuth();
            string auth = authResultModel.key;
            string stoid = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stoid"]; //"wowtvupload";
            //string path = "/qqq/";

            string url = "https://stats.kinxcdn.com/api/upload";
            //string file = @"F:\project\한경TV\aaa.zip";
            string paramName = "file";
            string contentType = "application/octet-stream";
            System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("key", auth);
            nvc.Add("stoid", stoid);
            nvc.Add("path", cdnUpLoadPath);

            Random random = new Random();
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x") + random.Next(1000, 9999).ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.AllowWriteStreamBuffering = false;
            wr.SendChunked = true;
            Stream rs = wr.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, fileName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
            //FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            //fileStream.Close();
            //stream.Close();
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();
            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                string resultJsonString = reader2.ReadToEnd();

                CdnResultModel<FileUploadResultModel> pObj = JsonConvert.DeserializeObject<CdnResultModel<FileUploadResultModel>>(resultJsonString);
                fileUploadResultModel = pObj.Response;

                if(fileUploadResultModel.list == null)
                {
                    throw new Exception("CDN 파일업로드 실패");
                }
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }

                throw new Exception("CDN 파일업로드 실패");
            }
            finally
            {
                wr = null;
            }



            return fileUploadResultModel;
        }


        public void FtpUpLoad(string cdnUpLoadPath, string fileName, Stream stream)
        {
            string strFTP_URL = @"ftp://wownetup.xst.kinxcdn.com/"; 
            string strFTP_ID = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stoid"]; //"wowtvupload";
            string strFTP_PW = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stopwd"]; //"Uploadwowtv123@!";

            cdnUpLoadPath = cdnUpLoadPath.Replace("\\", "/");

            //MakeDirectory(strFTP_URL);
            string prevDirectory = strFTP_URL;
            string tempDirectory = strFTP_URL;
            string[] directoryList = cdnUpLoadPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var item in directoryList)
            {
                tempDirectory += item + "/";
                if(IsDirectoryExist(prevDirectory, item) == false)
                {
                    MakeDirectory(tempDirectory);
                }
                prevDirectory = tempDirectory;
            }

            strFTP_URL = strFTP_URL + cdnUpLoadPath + fileName;

            // 선택파일의 이진 데이터 생성
            byte[] fileData = new byte[stream.Length];

            // 파일정보를 바이너리에 저장
            BinaryReader br = new BinaryReader(stream);
            br.Read(fileData, 0, fileData.Length);
            br.Close();

            WebClient request = new WebClient();

            // FTP 로 접속
            request.Credentials = new NetworkCredential(strFTP_ID, strFTP_PW);

            // FTP 로 데이터 업로드
            byte[] newFileData = request.UploadData(strFTP_URL, fileData);
        }


        private bool IsDirectoryExist(string path, string directoryName)
        {
            bool isExist = false;

            string strFTP_URL = path;
            string strFTP_ID = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stoid"]; //"wowtvupload";
            string strFTP_PW = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stopwd"]; //"Uploadwowtv123@!";

            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFTP_URL));
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(strFTP_ID, strFTP_PW);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();

            StreamReader streamReader = new StreamReader(ftpStream, Encoding.UTF8);
            string data = streamReader.ReadToEnd();

            ftpStream.Close();
            response.Close();


            string[] directorys = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int count = directorys.Where(a => a.ToUpper() == directoryName.ToUpper()).Count();
            if (count > 0)
            {
                isExist = true;
            }

            return isExist;
        }


        private void MakeDirectory(string path)
        {
            string strFTP_URL = path;
            string strFTP_ID = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stoid"]; //"wowtvupload";
            string strFTP_PW = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stopwd"]; //"Uploadwowtv123@!";

            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFTP_URL));
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(strFTP_ID, strFTP_PW);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();

            ftpStream.Close();
            response.Close();
        }


        

        private string GetParentDirectory(string currentDirectory)
        {
            string[] directorys = currentDirectory.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string parentDirectory = string.Empty;
            for (int i = 0; i < directorys.Length - 1; i++)
            {
                parentDirectory += "/" + directorys[i];
            }

            return parentDirectory;
        }



        public MemoryStream FtpDownLoad(string cdnUpLoadPath)
        {
            var returnStream = new MemoryStream();
            string strFTP_URL = @"ftp://wownetup.xst.kinxcdn.com";
            string strFTP_ID = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stoid"]; //"wowtvupload";
            string strFTP_PW = System.Configuration.ConfigurationManager.AppSettings["Cdn_Stopwd"]; //"Uploadwowtv123@!";

            cdnUpLoadPath = cdnUpLoadPath.Replace("\\", "/");

            // WebClient 객체 생성
            using (WebClient cli = new WebClient())
            {
                // FTP 사용자 설정
                cli.Credentials = new NetworkCredential(strFTP_ID, strFTP_PW);

                // FTP 다운로드 실행
                //cli.DownloadFile(strFTP_URL + cdnUpLoadPath, fileName);
                byte[] newFileData = cli.DownloadData(strFTP_URL + cdnUpLoadPath);
                returnStream.Write(newFileData, 0, newFileData.Length);
                returnStream.Position = 0;
            }

            return returnStream;
        }
    }
}
