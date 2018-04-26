using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;

namespace Wow.Tv.Middle.Biz.Board
{
    public class BoardCommentBiz : BaseBiz
    {


        public NTB_BOARD_COMMENT GetAt(int commentSeq)
        {
            return db49_wowtv.NTB_BOARD_COMMENT.SingleOrDefault(a => a.COMMENT_SEQ == commentSeq);
        }


        public void Save(NTB_BOARD_COMMENT model, LoginUser loginUser)
        {
            NTB_BOARD_COMMENT data = GetAt(model.COMMENT_SEQ);
            if (data == null)
            {
                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_BOARD_COMMENT.Add(model);
            }
            else
            {
                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();
        }


        public void Delete(int commentSeq, LoginUser loginUser)
        {
            NTB_BOARD_COMMENT data = GetAt(commentSeq);
            if (data != null)
            {
                data.DEL_YN = "Y";
                db49_wowtv.SaveChanges();
            }
        }



    }
}
