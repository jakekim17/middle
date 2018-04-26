using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Biz.Member
{
    public class SmsSplit
    {
        public Hashtable parse(string arg)
        {
            System.Text.Encoding krEncoding = System.Text.Encoding.GetEncoding("euc-kr");

            Hashtable result = new Hashtable();
            byte[] bInput = krEncoding.GetBytes(arg);
            int nLength = bInput.Length;

            int nStart = 0;
            int nEnd = 0;
            int nCutLength = 0;

            string sName = "";
            string sValue = "";

            while (nStart < nLength)
            {
                // ---- 파라미터명 추출하기 Start
                nEnd = getIndex(bInput, ':', nStart);

                // 항상 쌍이 맞아야 함으로 : 가 없다면 오류
                if (nEnd < 0)
                {
                    Console.WriteLine("[ERROR] missing #1:");
                    return null;
                }

                // byte 수 추출
                nCutLength = Convert.ToInt32(cutHanString(arg, nStart, nEnd));

                // 마지막 길이를 넘어서면 오류
                if (nEnd >= nLength)
                {
                    Console.WriteLine("[ERROR] length error #1");
                    return null;
                }

                // 데이타 추출
                sName = cutHanString(arg, nEnd + 1, nEnd + nCutLength + 1);
                // ---- 파라미터명 추출하기 End


                // ---- 파라미터에 대한 데이타 추출하기 Start
                nStart = nEnd + nCutLength + 1;
                nEnd = getIndex(bInput, ':', nStart);

                // 항상 쌍이 맞아야 함으로 : 가 없다면 오류
                if (nEnd < 0)
                {
                    Console.WriteLine("[ERROR] missing #2:");
                    return null;
                }

                // byte 수 추출
                nCutLength = Convert.ToInt32(cutHanString(arg, nStart, nEnd));

                // 마지막 길이를 넘어서면 오류
                if (nEnd >= nLength)
                {
                    Console.WriteLine("[ERROR] length error #2");
                    return null;
                }

                // 데이타 추출
                sValue = cutHanString(arg, nEnd + 1, nEnd + nCutLength + 1);
                // ---- 파라미터에 대한 데이타 추출하기 End

                nStart = nEnd + nCutLength + 1;

                // Hashtable 에 정보입력
                result.Add(sName, sValue);
            }

            return result;
        }

        private int getIndex(byte[] bInput, char ch, int start)
        {
            int nReturn = -1;
            int nLength = bInput.Length;
            int idx = start;

            while (idx < nLength)
            {
                if ((byte)bInput[idx] == ch)
                {
                    nReturn = idx;
                    break;
                }

                idx++;
            }

            return nReturn;
        }

        private String cutHanString(String sInput, int nStart, int nEnd)
        {
            System.Text.Encoding krEncoding = System.Text.Encoding.GetEncoding("euc-kr");

            int iLength = nEnd - nStart;
            byte[] bInput = krEncoding.GetBytes(sInput);

            return krEncoding.GetString(bInput, nStart, iLength);
        }
    }
}
