using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.editVOD;

using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "ProgramCmsAuthService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 ProgramCmsAuthService.svc나 ProgramCmsAuthService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public partial class BroadService : IProgramCmsAuthService
    {
        public void CmsMainFileMigration()
        {
            new ProgramCmsAuthBiz().MainFileMigration();
        }

        //public TAB_PGM_CMS_AUTH GetAtCmsAuth(string programCode)
        //{
        //    return new ProgramCmsAuthBiz().GetAt(programCode);
        //}

        //public void SaveCmsAuth(TAB_PGM_CMS_AUTH model)
        //{
        //    new ProgramCmsAuthBiz().Save(model);
        //}
    }
}
