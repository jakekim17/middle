using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;
using Wow.Tv.Middle.Model.Db90.DNRS;

using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "ProgramTemplateService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 ProgramTemplateService.svc나 ProgramTemplateService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public partial class BroadService : IProgramTemplateService
    {

        NTB_PROGRAM_TEMPLATE IProgramTemplateService.GetAtProgramTemplate(string programCode)
        {
            return new ProgramTemplateBiz().GetAt(programCode);
        }


        public int SaveProgramTemplate(NTB_PROGRAM_TEMPLATE model, T_NEWS_PRG newsProgram, LoginUser loginUser)
        {
            return new ProgramTemplateBiz().Save(model, newsProgram, loginUser);
        }

        public void DeleteProgramTemplate(string programCode)
        {
            new ProgramTemplateBiz().Delete(programCode);
        }


        #region Parnter


        //public List<NTB_PROGRAM_TEMPLATE_PARTNER> GetProgramTemplatePartnerList(int programTemplateSeq)
        //{
        //    return new ProgramTemplateBiz().GetPartnerList(programTemplateSeq);
        //}

        //public void AddProgramTemplatePartner(int programTemplateSeq, int payNo, LoginUser loginUser)
        //{
        //    new ProgramTemplateBiz().AddPartner(programTemplateSeq, payNo, loginUser);
        //}

        //public void DeleteProgramTemplatePartner(int programTemplateSeq, int payNo)
        //{
        //    new ProgramTemplateBiz().DeletePartner(programTemplateSeq, payNo);
        //}

        //public void DeleteProgramTemplatePartnerList(int programTemplateSeq)
        //{
        //    new ProgramTemplateBiz().DeletePartnerList(programTemplateSeq);
        //}

        #endregion





        #region Group

        public List<NTB_PROGRAM_GROUP> GetProgramTemplateGroupList(int programTemplateSeq)
        {
            return new ProgramTemplateBiz().GetGroupList(programTemplateSeq);
        }

        public List<NTB_PROGRAM_TEMPLATE_GROUP> GetProgramTemplateGroupListByGroupSeq(int programGroupSeq)
        {
            return new ProgramTemplateBiz().GetGroupListByGroupSeq(programGroupSeq);
        }

        public void AddProgramTemplateGroup(int programTemplateSeq, int programGroupSeq, LoginUser loginUser)
        {
            new ProgramTemplateBiz().AddGroup(programTemplateSeq, programGroupSeq, loginUser);
        }

        public void DeleteProgramTemplateGroup(int programTemplateSeq, int programGroupSeq)
        {
            new ProgramTemplateBiz().DeleteGroup(programTemplateSeq, programGroupSeq);
        }

        public void DeleteProgramTemplateGroupList(int programTemplateSeq)
        {
            new ProgramTemplateBiz().DeleteGroupList(programTemplateSeq);
        }

        public void UpDownProgramTemplateGroupList(int programTemplateSeq, int programGroupSeq, bool isUp)
        {
            new ProgramTemplateBiz().UpDownGroup(programTemplateSeq, programGroupSeq, isUp);
        }

        #endregion


        public List<T_NEWS_PRG> GetFamilyList(string programCode)
        {
            return new ProgramTemplateBiz().GetFamilyList(programCode);
        }

        public List<T_NEWS_PRG> GetChildList(string programCode)
        {
            return new ProgramTemplateBiz().GetChildList(programCode);
        }

        public string GetProgramType(string programCode)
        {
            return new ProgramTemplateBiz().GetProgramType(programCode);
        }
    }
}
