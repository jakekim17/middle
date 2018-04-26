using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Menu;

namespace Wow.Tv.Middle.Biz.Menu
{
    public class MenuGroupBiz : BaseBiz
    {
        public NTB_MENU_GROUP GetAt(int menuSeq, int groupSeq)
        {
            return db49_wowtv.NTB_MENU_GROUP.SingleOrDefault(a => a.MENU_SEQ == menuSeq && a.GROUP_SEQ == groupSeq);
        }


        public void SaveList(int groupSeq, List<MenuItem> list, LoginUser loginUser)
        {
            foreach(var item in list)
            {
                NTB_MENU_GROUP model = new NTB_MENU_GROUP();
                model.GROUP_SEQ = groupSeq;
                model.MENU_SEQ = item.MenuSeq;
                model.READ_YN = (item.ReadYn == true ? "Y" : "N") ;
                model.WRITE_YN = (item.WriteYn == true ? "Y" : "N");
                model.DEL_YN = (item.DeleteYn == true ? "Y" : "N");

                Save(model, loginUser);
            }

        }

        public void Save(NTB_MENU_GROUP model, LoginUser loginUser)
        {
            NTB_MENU_GROUP data = GetAt(model.MENU_SEQ, model.GROUP_SEQ);
            if (data == null)
            {
                db49_wowtv.NTB_MENU_GROUP.Add(model);
            }
            else
            {
                data.READ_YN = model.READ_YN;
                data.WRITE_YN = model.WRITE_YN;
                data.DEL_YN = model.DEL_YN;
            }
            db49_wowtv.SaveChanges();
        }
    }
}
