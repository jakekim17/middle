﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Wow.Tv.Middle.WcfService.ActionLog
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IAccessLogService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IAccessLogService
    {
        [OperationContract]
        void Regist(string ipAddress, int menuSeq, string accessCode, string userId, string url);
    }
}
