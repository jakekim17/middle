using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Menu;
using Wow.Tv.Middle.Biz.Menu;

namespace Wow.Tv.Middle.WcfService.Menu
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MenuService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MenuService.svc나 MenuService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MenuService : IMenuService
    {
        public ListModel<NTB_MENU> SearchList(MenuCondition condition)
        {
            return new MenuBiz().SearchList(condition);
        }


        public ListModel<MenuGroupItem> SearchListMenuGroup(int groupSeq, MenuCondition condition)
        {
            return new MenuBiz().SearchListMenuGroup(groupSeq, condition);
        }


        public NTB_MENU GetAt(int menuSeq)
        {
            return new MenuBiz().GetAt(menuSeq);
        }

        public int Save(NTB_MENU model, LoginUser loginUser)
        {
            return new MenuBiz().Save(model, loginUser);
        }

        public void Delete(int menuSeq, LoginUser loginUser)
        {
            new MenuBiz().Delete(menuSeq, loginUser);
        }

        public void UpDown(int menuSeq, bool isUp, LoginUser loginUser)
        {
            new MenuBiz().UpDown(menuSeq, isUp, loginUser);
        }


        public NTB_MENU GetAtByUrl(string url)
        {
            return new MenuBiz().GetAtByUrl(url);
        }
        public NTB_MENU GetAtByName(string channelCode, string name)
        {
            return new MenuBiz().GetAtByName(channelCode, name);
        }


        public void AuthListSave(int groupSeq, List<MenuItem> list, LoginUser loginUser)
        {
            new MenuGroupBiz().SaveList(groupSeq, list, loginUser);
        }

        public void AuthSave(NTB_MENU_GROUP model, LoginUser loginUser)
        {
            new MenuGroupBiz().Save(model, loginUser);
        }




        public Dictionary<string, int?> GetMenuList()
        {
            return new MenuBiz().GetMenuList();
        }

    }
}
