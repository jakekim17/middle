using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Menu;

namespace Wow.Tv.Middle.WcfService.Menu
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMenuService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IMenuService
    {
        [OperationContract]
        ListModel<NTB_MENU> SearchList(MenuCondition condition);


        [OperationContract]
        ListModel<MenuGroupItem> SearchListMenuGroup(int groupSeq, MenuCondition condition);



        [OperationContract]
        NTB_MENU GetAt(int menuSeq);

        [OperationContract]
        int Save(NTB_MENU model, LoginUser loginUser);

        [OperationContract]
        void Delete(int menuSeq, LoginUser loginUser);

        [OperationContract]
        void UpDown(int menuSeq, bool isUp, LoginUser loginUser);



        [OperationContract]
        NTB_MENU GetAtByUrl(string url);
        [OperationContract]
        NTB_MENU GetAtByName(string channelCode, string name);



        [OperationContract]
        void AuthListSave(int groupSeq, List<MenuItem> list, LoginUser loginUser);

        [OperationContract]
        void AuthSave(NTB_MENU_GROUP model, LoginUser loginUser);





        [OperationContract]
        Dictionary<string, int?> GetMenuList();

    }
}
