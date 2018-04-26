using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Wow.Tv.Middle.Model.Db89.Sleep_wowbill
{
    public partial class Db89_Sleep_wowbill : DbContext
    {
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string msg = ex.Message;

                if (ex.EntityValidationErrors.Count() > 0)
                {
                    foreach (var item in ex.EntityValidationErrors.ElementAt(0).ValidationErrors)
                    {
                        msg += "\r\n" + item.ErrorMessage;
                    }

                }
                throw new Exception(msg);
            }
        }
    }
}
