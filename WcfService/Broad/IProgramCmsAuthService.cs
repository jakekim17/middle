using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.editVOD;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IProgramCmsAuthService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IProgramCmsAuthService
    {
        [OperationContract]
        void CmsMainFileMigration();
        
        //[OperationContract]
        //TAB_PGM_CMS_AUTH GetAtCmsAuth(string programCode);

        //[OperationContract]
        //void SaveCmsAuth(TAB_PGM_CMS_AUTH model);
    }
}
