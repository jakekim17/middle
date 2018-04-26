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

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IProgramTemplateService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IProgramTemplateService
    {
        [OperationContract]
        NTB_PROGRAM_TEMPLATE GetAtProgramTemplate(string programCode);


        [OperationContract]
        int SaveProgramTemplate(NTB_PROGRAM_TEMPLATE model, T_NEWS_PRG newsProgram, LoginUser loginUser);


        [OperationContract]
        void DeleteProgramTemplate(string programCode);




        #region Partner


        //[OperationContract]
        //List<NTB_PROGRAM_TEMPLATE_PARTNER> GetProgramTemplatePartnerList(int programTemplateSeq);



        //[OperationContract]
        //void AddProgramTemplatePartner(int programTemplateSeq, int payNo, LoginUser loginUser);


        //[OperationContract]
        //void DeleteProgramTemplatePartner(int programTemplateSeq, int payNo);

        //[OperationContract]
        //void DeleteProgramTemplatePartnerList(int programTemplateSeq);

        #endregion



        #region Group

        [OperationContract]
        List<NTB_PROGRAM_GROUP> GetProgramTemplateGroupList(int programTemplateSeq);

        [OperationContract]
        List<NTB_PROGRAM_TEMPLATE_GROUP> GetProgramTemplateGroupListByGroupSeq(int programGroupSeq);


        [OperationContract]
        void AddProgramTemplateGroup(int programTemplateSeq, int programGroupSeq, LoginUser loginUser);


        [OperationContract]
        void DeleteProgramTemplateGroup(int programTemplateSeq, int programGroupSeq);

        [OperationContract]
        void DeleteProgramTemplateGroupList(int programTemplateSeq);


        [OperationContract]
        void UpDownProgramTemplateGroupList(int programTemplateSeq, int programGroupSeq, bool isUp);


        #endregion



        [OperationContract]
        List<T_NEWS_PRG> GetFamilyList(string programCode);
        
        [OperationContract]
        List<T_NEWS_PRG> GetChildList(string programCode);

        [OperationContract]
        string GetProgramType(string programCode);

    }
}
