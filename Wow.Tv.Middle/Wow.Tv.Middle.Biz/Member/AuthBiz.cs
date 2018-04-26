using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.Biz.Member
{
    public class AuthBiz: BaseBiz
    {
        public IpinInitResult IpinInit(IpinInitRequest request)
        {
            string requestData = "";
            string returnMessage = "";
            int returnCode;

            IPIN2CLIENTLib.KisinfoClass ipinInfo = new IPIN2CLIENTLib.KisinfoClass();
            ipinInfo.fnRequestSEQ(request.IDPCode);
            returnCode = ipinInfo.fnRequest(request.IDPCode, request.IDPPassword, request.CPREQuestNum, request.ReturnUrl);

            if (returnCode == 0)
            {
                requestData = ipinInfo.bstrRequestCipherData; // 클라이언트 요청정보 암호화
            }
            else if (returnCode == -1)
            {
                returnMessage = "암호화 시스템 오류";
                requestData = "";
            }
            else if (returnCode == -2)
            {
                returnMessage = "암호화 처리 오류";
                requestData = "";
            }
            else if (returnCode == -3)
            {
                returnMessage = "암호화 데이터 오류";
                requestData = "";
            }
            else if (returnCode == -9)
            {
                returnMessage = "입력값 오류";
                requestData = "";
            }

            IpinInitResult result = new IpinInitResult();
            result.ReturnCode = returnCode;
            result.ReturnMessage = returnMessage;
            result.RequestData = requestData;

            return result;
        }

        /// <summary>
        /// 신규가입/아이디찾기/비번찾기 시도 로그
        /// </summary>
        /// <param name="request"></param>
        public IpinCheckResult IpinCheck(IpinCheckRequest request)
        {
            IpinCheckResult response = new IpinCheckResult();

            IPIN2CLIENTLib.KisinfoClass ipinInfo = new IPIN2CLIENTLib.KisinfoClass();
            response.ReturnCode = ipinInfo.fnResponseExt(request.IDPCode, request.IDPPassword, request.encryptedData, request.CPREQuestNum);

            if (response.ReturnCode == 1) // 인증성공
            {
                response.VirtualNumber = ipinInfo.bstrVNumber;
                response.Name = ipinInfo.bstrName;
                response.DupInfo = ipinInfo.bstrDupInfo;
                response.AgeCode = ipinInfo.bstrAgeCode;
                if (ipinInfo.bstrGenderCode == "0")
                {
                    response.GenderCode = "2";
                }
                else
                {
                    response.GenderCode = "1";
                }
                response.BirthDate = ipinInfo.bstrBirthDate;
                response.NationalInfo = ipinInfo.bstrNationalInfo;
                response.AuthInfo = ipinInfo.bstrAuthInfo;
                response.CoInfo1 = ipinInfo.bstrCoInfo1;
                response.CIUpdate = ipinInfo.bstrCIUpdate;
            }
            else if (response.ReturnCode == -1)
            {
                response.ReturnMessage = "복호화 시스템 오류";
            }
            else if (response.ReturnCode == -4)
            {
                response.ReturnMessage = "복호화 처리 오류";
            }
            else if (response.ReturnCode == -5)
            {
                response.ReturnMessage = "복호화 위변조 검증 실패";
            }
            else if (response.ReturnCode == -9)
            {
                response.ReturnMessage = "입력값 오류";
            }

            return response;
        }

        public SmsInitResult SmsInit(SmsInitRequest request)
        {
            // 요청번호
            // 성공/실패후에 같은 값으로 되돌려주게 되므로 업체에 적절하게 변경하여 쓰건, 아래와 같이 생성한다.
            string requestNo = "REQ0000000001";

            string encryptedData = "";
            string returnMessage = "";
            int returnCode;

            CPCLIENTLib.NiceIDClass smsInfo = new CPCLIENTLib.NiceIDClass();
            int requestNoreturn = smsInfo.fnRequestNO(request.SiteCode);
            if (requestNoreturn == 0)
            {
                requestNo = smsInfo.bstrRandomRequestNO;
            }

            string authType = request.isMobile == true ? "M" : ""; // 없으면 기본 선택화면, X: 공인인증서, M: 핸드폰, C: 카드
            string plainData =
                "7:REQ_SEQ" + Encoding.Default.GetByteCount(requestNo) + ":" + requestNo +
                "8:SITECODE" + Encoding.Default.GetByteCount(request.SiteCode) + ":" + request.SiteCode +
                "9:AUTH_TYPE" + Encoding.Default.GetByteCount(authType) + ":" + authType +
                "7:RTN_URL" + Encoding.Default.GetByteCount(request.ReturnUrl) + ":" + request.ReturnUrl +
                "7:ERR_URL" + Encoding.Default.GetByteCount(request.ErrorUrl) + ":" + request.ErrorUrl;

            returnCode = smsInfo.fnEncode(request.SiteCode, request.SitePassword, plainData);
            if (returnCode == 0)
            {
                encryptedData = smsInfo.bstrCipherData;
            }
            else
            {
                // -1 : 암호화 시스템 오류
                // -2 : 암호화 처리 오류
                // -3 : 암호화 데이터 오류
                // -4 : 입력 데이터 오류
                returnMessage = "요청정보_암호화_오류:" + returnCode;
            }

            SmsInitResult result = new SmsInitResult();
            result.RequestNo = requestNo;
            result.ReturnCode = returnCode;
            result.ReturnMessage = returnMessage;
            result.EncryptedData = encryptedData;

            return result;
        }

        public SmsCheckResult SmsCheck(SmsCheckRequest request)
        {
            SmsCheckResult result = new SmsCheckResult();
            string elementRequestSeq = "";

            CPCLIENTLib.NiceIDClass smsInfo = new CPCLIENTLib.NiceIDClass();
            result.ReturnCode = smsInfo.fnDecode(request.SiteCode, request.SitePassword, request.EncryptedData);

            if (result.ReturnCode == 0)
            {
                string plainData = smsInfo.bstrPlainData;
                string cipherTime = smsInfo.bstrCipherDateTime;

                result.ReturnMessage = "인증결과_복호화_성공_원문 [" + plainData + "]";

                Hashtable element = new Hashtable();
                SmsSplit split = new SmsSplit();
                element = split.parse(plainData);
                elementRequestSeq = element["REQ_SEQ"] as string;

                if (element == null || element.Count == 0)
                {
                    result.ReturnMessage = "응답값이 유효하지 않습니다.";
                    result.ReturnCode = -100;
                }
                else if (request.RequestNo != elementRequestSeq)
                {
                    result.ReturnMessage = "세션값 오류입니다.";
                    result.ReturnCode = -200;
                }
                else
                {
                    result.CipherTime = cipherTime;
                    result.RequestSeq = element["REQ_SEQ"] as string;
                    result.ResponseSeq = element["RES_SEQ"] as string;
                    result.AuthType = element["AUTH_TYPE"] as string;
                    result.Name = element["NAME"] as string;
                    result.BirthDate = element["BIRTHDATE"] as string;
                    if (element["GENDER"] as string == "0")
                    {
                        result.Gender = "2";
                    }
                    else
                    {
                        result.Gender = "1";
                    }

                    result.NationalInfo = element["NATIONALINFO"] as string;
                    result.DI = element["DI"] as string;
                    result.CI = element["CI"] as string;
                    result.MobileNo = element["MOBILE_NO"] as string;
                    result.MobileCompany = element["MOBILE_CO"] as string;

                    if (string.IsNullOrEmpty(result.DI) == true)
                    {
                        result.ReturnMessage = "입력값이 잘못되었습니다.";
                        result.ReturnCode = -300;
                    }
                }
            }
            else
            {
                switch (result.ReturnCode)
                {
                    case -1: result.ReturnMessage = "암호화 시스템 에러입니다."; break;
                    case -4: result.ReturnMessage = "입력 데이터 오류입니다."; break;
                    case -5: result.ReturnMessage = "복호화 해쉬 오류입니다."; break;
                    case -6: result.ReturnMessage = "복호화 데이터 오류입니다."; break;
                    case -9: result.ReturnMessage = "입력 데이터 오류입니다."; break;
                    case -12: result.ReturnMessage = "사이트 패스워드 오류입니다."; break;
                }
            }

            //MemberIdSearchLog("", result.Name, result.DI, request.RequestNo, elementRequestSeq, "MOBILE_REG");

            return result;
        }

        public void MemberIdSearchLog(string vNumber, string name, string dupInfo, string reqSeqSession, string reqSeqResult, string type)
        {
            TblMemberIdSearchLog log = new TblMemberIdSearchLog();
            log.vNumber = vNumber;
            log.Name = name;
            log.DupInfo = dupInfo;
            log.REQ_SEQ_session = reqSeqSession;
            log.REQ_SEQ_sResult = reqSeqResult;
            log.Type = type;
            log.RegDt = DateTime.Now;
            db89_wowbill.TblMemberIdSearchLog.Add(log);
            db89_wowbill.SaveChanges();
        }

        public EmailAuthResult EmailAuth(int userNumber)
        {
            EmailAuthResult result = new EmailAuthResult();

            try
            {
                db89_wowbill.Database.ExecuteSqlCommand("exec usp_MemberEmailAuth " + userNumber + ", 'EXP'");

                tblUser user = (from tbl in db89_wowbill.tblUser.AsNoTracking() where tbl.userNumber == userNumber select tbl).SingleOrDefault();

                tblMemberEmailAuth emailAuthData = (from tbl in db89_wowbill.tblMemberEmailAuth where tbl.userNumber == userNumber select tbl).SingleOrDefault();
                result.ExpireDate = emailAuthData.expireDt.Value;

                result.Success = true;
                result.ReturnMessage = "";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnMessage = ex.Message;
            }

            return result;
        }
    }
}
