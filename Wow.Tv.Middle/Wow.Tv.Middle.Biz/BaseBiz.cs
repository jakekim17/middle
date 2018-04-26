using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.wowcafe;
using Wow.Tv.Middle.Model.Db51.ARSsms;
using Wow.Tv.Middle.Model.Db51.contents;
using Wow.Tv.Middle.Model.Db51.WOWMMS;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB;
using Wow.Tv.Middle.Model.Db90.DNRS;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db51.ARSConsult;
using Wow.Tv.Middle.Model.Db35.chinaguide;
using Wow.Tv.Middle.Model.Db123.WOW4989;
using Wow.Tv.Middle.Model.Db16.wowfa;
using Wow.Tv.Middle.Model.Db51.ems50;
using Wow.Tv.Middle.Model.Db89.Sleep_wowbill;


namespace Wow.Tv.Middle.Biz
{
    public class BaseBiz
    {
        //protected wowtvEntities wowTvDb;
        private Db49_wowtv Db49_wowtv_Entities;
        private Db49_wownet Db49_wownet_Entities;
        private Db49_Article Db49_Article_Entities;
        private Db49_broadcast Db49_broadcast_Entities;
        private Db49_editVOD Db49_editVOD_Entities;
        private Db49_wowcafe Db49_wowcafe_Entities;
        private Db51_ARSsms Db51_ARSsms_Entities;
        private Db51_contents Db51_contents_Entities;
        private Db51_WOWMMS Db51_WOWMMS_Entities;
        private D22_stock Db22_stock_Entiies;
        private Db89_wowbill Db89_wowbill_Entiies;
        private Db89_Sleep_wowbill Db89_Sleep_wowbill_Entiies;
        private Db89_WOWTV_BILL_DB Db89_WOWTV_BILL_DB_Entiies;
        private Db90_DNRS Db90_DNRS_Entiies;
        private Db51_ARSConsult Db51_ARSConsult_Entiies;
        private Db35_chinaguide Db35_chinaguide_Entiies;
        private Db123_WOw4989 Db123_WOW4989_Entiies;
        private Db16_wowfa Db16_wowfa_Entiies;
        private Db51_ems50 Db51_ems50_Entiies;

        public BaseBiz()
        {
            //wowTvDb = new wowtvEntities();
        }


        protected Db49_wowtv db49_wowtv
        {
            get
            {
                if (Db49_wowtv_Entities == null)
                {
                    Db49_wowtv_Entities = new Db49_wowtv();
                    Db49_wowtv_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db49_wowtv_Entities;
            }
        }

        protected Db49_Article db49_Article
        {
            get
            {
                if (Db49_Article_Entities == null)
                {
                    Db49_Article_Entities = new Db49_Article();
                    Db49_Article_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db49_Article_Entities;
            }
        }

        protected Db49_wownet db49_wownet
        {
            get
            {
                if (Db49_wownet_Entities == null)
                {
                    Db49_wownet_Entities = new Db49_wownet();
                    Db49_wownet_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db49_wownet_Entities;
            }
        }

        protected Db49_broadcast db49_broadcast
        {
            get
            {
                if (Db49_broadcast_Entities == null)
                {
                    Db49_broadcast_Entities = new Db49_broadcast();
                    Db49_broadcast_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db49_broadcast_Entities;
            }
        }

        protected Db49_editVOD db49_editVOD
        {
            get
            {
                if (Db49_editVOD_Entities == null)
                {
                    Db49_editVOD_Entities = new Db49_editVOD();
                    Db49_editVOD_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db49_editVOD_Entities;
            }
        }

        protected Db49_wowcafe db49_wowcafe
        {
            get
            {
                if (Db49_wowcafe_Entities == null)
                {
                    Db49_wowcafe_Entities = new Db49_wowcafe();
                    Db49_wowcafe_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db49_wowcafe_Entities;
            }
        }

        protected Db51_ARSsms db51_ARSsms
        {
            get
            {
                if (Db51_ARSsms_Entities == null)
                {
                    Db51_ARSsms_Entities = new Db51_ARSsms();
                    Db51_ARSsms_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db51_ARSsms_Entities;
            }
        }

        protected Db51_contents Db51_contents
        {
            get
            {
                if (Db51_contents_Entities == null)
                {
                    Db51_contents_Entities = new Db51_contents();
                    Db51_contents_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db51_contents_Entities;
            }
        }

        protected Db51_ARSConsult db51_ARSConsult
        {
            get
            {
                if (Db51_ARSConsult_Entiies == null)
                {
                    Db51_ARSConsult_Entiies = new Db51_ARSConsult();
                    Db51_ARSConsult_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db51_ARSConsult_Entiies;
            }
        }

        protected Db51_WOWMMS Db51_WOWMMS
        {
            get
            {
                if (Db51_WOWMMS_Entities == null)
                {
                    Db51_WOWMMS_Entities = new Db51_WOWMMS();
                    Db51_WOWMMS_Entities.Configuration.ProxyCreationEnabled = false;
                }

                return Db51_WOWMMS_Entities;
            }
        }

        protected D22_stock db22_stock
        {
            get
            {
                if (Db22_stock_Entiies == null)
                {
                    Db22_stock_Entiies = new D22_stock();
                    Db22_stock_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db22_stock_Entiies;
            }
        }

        protected Db89_wowbill db89_wowbill
        {
            get
            {
                if (Db89_wowbill_Entiies == null)
                {
                    Db89_wowbill_Entiies = new Db89_wowbill();
                    Db89_wowbill_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db89_wowbill_Entiies;
            }
        }

        protected Db89_Sleep_wowbill db89_Sleep_wowbill
        {
            get
            {
                if (Db89_Sleep_wowbill_Entiies == null)
                {
                    Db89_Sleep_wowbill_Entiies = new Db89_Sleep_wowbill();
                    Db89_Sleep_wowbill_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db89_Sleep_wowbill_Entiies;
            }
        }

        protected Db89_WOWTV_BILL_DB db89_WOWTV_BILL_DB
        {
            get
            {
                if (Db89_WOWTV_BILL_DB_Entiies == null)
                {
                    Db89_WOWTV_BILL_DB_Entiies = new Db89_WOWTV_BILL_DB();
                    Db89_WOWTV_BILL_DB_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db89_WOWTV_BILL_DB_Entiies;
            }
        }

        protected Db90_DNRS db90_DNRS
        {
            get
            {
                if (Db90_DNRS_Entiies == null)
                {
                    Db90_DNRS_Entiies = new Db90_DNRS();
                    Db90_DNRS_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db90_DNRS_Entiies;
            }
        }


        protected Db35_chinaguide db35_chinaguide
        {
            get
            {
                if (Db35_chinaguide_Entiies == null)
                {
                    Db35_chinaguide_Entiies = new Db35_chinaguide();
                    Db35_chinaguide_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db35_chinaguide_Entiies;
            }
        }



        protected Db123_WOw4989 db123_WOw4989
        {
            get
            {
                if (Db123_WOW4989_Entiies == null)
                {
                    Db123_WOW4989_Entiies = new Db123_WOw4989();
                    Db123_WOW4989_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db123_WOW4989_Entiies;
            }
        }

        protected Db16_wowfa db16_wowfa
        {
            get
            {
                if (Db16_wowfa_Entiies == null)
                {
                    Db16_wowfa_Entiies = new Db16_wowfa();
                    Db16_wowfa_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db16_wowfa_Entiies;
            }
        }

        protected Db51_ems50 db51_ems50
        {
            get
            {
                if (Db51_ems50_Entiies == null)
                {
                    Db51_ems50_Entiies = new Db51_ems50();
                    Db51_ems50_Entiies.Configuration.ProxyCreationEnabled = false;
                }

                return Db51_ems50_Entiies;
            }
        }
    }
}
