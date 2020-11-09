using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.Admin;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.TradingStar;

namespace Wow.Tv.Middle.Biz.Admin
{
    /// <summary>
    /// <para>  수익률게시판 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-09-04</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-09-04</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-09-04 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class TradingStarBiz : BaseBiz
    {

        /// <summary>
        /// 검색 된 List
        /// </summary>
        /// <returns></returns>
        public List<tblTradingStarCategory> CategoryList()
        {
            var list = db49_wowtv.tblTradingStarCategories.AsQueryable().OrderBy(x => x.regdt).ToList();


            return list;
        }

        /// <summary>
        /// 단일 검색
        /// </summary>
        /// <param name="tradingCode"></param>
        /// <returns></returns>
        public tblTradingStarCategory GetCategory(string tradingCode)
        {
            return db49_wowtv.tblTradingStarCategories.AsQueryable().SingleOrDefault(x => x.tradingCode.Equals(tradingCode));
        }

        /// <summary>
        /// 수익률 카테고리 등록/수정
        /// </summary>
        /// <param name="tradingstarCategory"></param>
        public void CategorySave(tblTradingStarCategory model, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    tblTradingStarCategory category = GetCategory(model.tradingCode);
                    if (category == null)
                    {
                        model.regdt = DateTime.Now;
                        db49_wowtv.tblTradingStarCategories.Add(model);
                        db49_wowtv.SaveChanges();
                    }
                    else
                    {
                        category.codeName = model.codeName;
                        category.startDt = model.startDt;
                        category.endDt = model.endDt;
                        //category.regdt = DateTime.Now; AS-IS에서 수정을 안함
                        db49_wowtv.tblTradingStarCategories.AddOrUpdate(category);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 출연자/거래현황등록 검색 된 List
        /// </summary>
        /// <returns></returns>
        public ListModel<tblTradingStarUser> UserList(TradingStarCondition condition)
        {
            ListModel<tblTradingStarUser> resultData = new ListModel<tblTradingStarUser>();
            var list = db49_wowtv.tblTradingStarUser.AsQueryable();


            //기간 검색
            list = list.Where(x => x.tradingCode.Equals(condition.TradingCode));

            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }

                list = list.OrderByDescending(x=> x.weekname).ThenByDescending(x => x.name).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }


            resultData.ListData = list.ToList();

            if (resultData.ListData.Count <= 0) return resultData;

            //이미 listBox에서 불러진 데이터가 뿌려지는 중이므로, 지금 여기서 총누적수익률을 갱신해도 일괄갱신결과가 바로 적용되지 않고, 새로고침 1회후에 제대로 받아오게된다.
            //=============================2017-01-17 : 출연자 총 누적수익률 일괄 갱신 개발========================
            //[tblTradingStarTrade]의 수익률을 모두 합해서 [TBLTRADINGSTARUSER]의 해당 seq출연자의 [earningRateSum]컬럼을 UPDATE시켜준다.
            foreach (var item in resultData.ListData)
            {
                SPExecute_usp_tradingStarEarningRateRENEW(item.seq, item.tradingCode);
            }

            return resultData;
        }

        /// <summary>
        /// 출연자 단일 검색
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public tblTradingStarUser GetUser(int seq)
        {
            return db49_wowtv.tblTradingStarUser.AsQueryable().SingleOrDefault(x => x.seq.Equals(seq));
        }

        /// <summary>
        /// SP : 수익률게시판 출연자 총 누적수익률 갱신
        /// Description : TV어드민 > 수익률게시판 > 출연자리스트 접근시에 수행됨.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="tradingCode"></param>
        private void SPExecute_usp_tradingStarEarningRateRENEW(int seq, string tradingCode)
        {
            db49_wowtv.usp_tradingStarEarningRateRENEW(seq, tradingCode);
        }

        /// <summary>
        /// 출연자 삭제
        /// </summary>
        /// <param name="seq">seq</param>
        /// <param name="loginUser"></param>
        public void UserDelete(int seq)
        {
            tblTradingStarUser userSingle = GetUser(seq);

            if (userSingle == null) return;

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {

                    db49_wowtv.tblTradingStarUser.Remove(userSingle);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// 출연자 등록/수정
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void UserSave(tblTradingStarUser model, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    tblTradingStarUser user = GetUser(model.seq);
                    if (user == null)
                    {
                        model.totalScore = 0;
                        //model.introduce = "0";
                        if (model.earningRateSum == null)
                            model.earningRateSum = 0;

                        model.regDt = DateTime.Now;
                        db49_wowtv.tblTradingStarUser.Add(model);
                        db49_wowtv.SaveChanges();
                    }
                    else
                    {
                        user.name = model.name;
                        user.pro_id = model.pro_id;
                        user.replyid = model.replyid;
                        user.weekname = model.weekname;
                        user.stockchar = model.stockchar;
                        user.state = model.state;
                        user.thumbnail = model.thumbnail;
                        user.introduce = model.introduce;
                        user.memo = model.memo;
                        //user.regDt = DateTime.Now; AS-IS에서 수정을 안함
                        db49_wowtv.tblTradingStarUser.AddOrUpdate(user);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 거래현황 검색 된 List
        /// </summary>
        /// <returns></returns>
        public List<tblTradingStarTrade> TradeList(int regSeq)
        {
            var list = db49_wowtv.tblTradingStarTrade.AsQueryable();
            list = list.Where(x => x.regseq == regSeq).OrderByDescending(x=> x.seq);


            return list.ToList();
        }

        /// <summary>
        /// 단일 종목 검색
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public tblTradingStarTrade GetTrade(int seq)
        {
            return db49_wowtv.tblTradingStarTrade.AsQueryable().SingleOrDefault(x => x.seq.Equals(seq));
        }
        
        /// <summary>
        /// 종목 삭제
        /// </summary>
        /// <param name="seq">seq</param>
        /// <param name="loginUser"></param>
        public void TradeDelete(int seq)
        {
            tblTradingStarTrade tradeSingle = GetTrade(seq);

            if (tradeSingle == null) return;

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    db49_wowtv.tblTradingStarTrade.Remove(tradeSingle);

                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// 종목 등록/수정
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void TradeSave(tblTradingStarTrade model, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    tblTradingStarTrade trade = GetTrade(model.seq);
                    if (trade == null)
                    {
                        model.starMark = 0;
                        model.topperEarningRate = 0;
                        model.regdt = DateTime.Now;
                        db49_wowtv.tblTradingStarTrade.Add(model);
                        db49_wowtv.SaveChanges();
                    }
                    else
                    {
                        trade.cname = model.cname;
                        trade.code = model.code;
                        trade.broaddt = model.broaddt;
                        trade.buydt = model.buydt;
                        trade.buycost = model.buycost;
                        trade.buystate = model.buystate;
                        trade.selldt = model.selldt;
                        trade.sellcost = model.sellcost;
                        trade.topperEarningRate = model.topperEarningRate;
                        trade.goalEarningRate = model.goalEarningRate;
                        trade.regseq = model.regseq;
                        trade.strategybuy = model.strategybuy;
                        trade.strategytarget = model.strategytarget;
                        trade.strategylosscut = model.strategylosscut;
                        trade.targetCost = model.targetCost;
                        trade.losscutCost = model.losscutCost;
                        db49_wowtv.tblTradingStarTrade.AddOrUpdate(trade);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 현재가 조회
        /// </summary>
        /// <param name="korname"></param>
        /// <returns></returns>
        public List<tblOnlineSise> CurrentStockPriceList(string korname)
        {
            var stockPriceList = (from stock in db22_stock.tblStockBatch
                join sise in db22_stock.tblOnlineSise on stock.StockCode equals sise.StockCode
                where stock.GroupID == "ST" && stock.korName == korname
                select sise).ToList();
            //ST는 AS-IS 고정
            //var stockPriceList = db22_stock.tblStockBatch.Join(db22_stock.tblOnlineSise, batch => batch.StockCode, sise => sise.StockCode, (batch, sise) => new {batch, sise})
            //    .Where(t => t.batch.GroupID == "ST" && t.batch.korName == korname)
            //    .Select(t => sise
            //    {
            //        korname = t.batch.korName,
            //        TradePrice = t.sise.TradePrice
            //    }).ToList();

            if (stockPriceList.Count != 0) return stockPriceList;
            //위에 정보가 없으면 SP 호출해서 값을 가져온다.
            var spStockPrice = db22_stock.usp_GetStockPrice(korname);
            stockPriceList = spStockPrice.Select(t => new tblOnlineSise
            {
                korname = t.stock_wanname,
                TradePrice = t.curr_price
            }).ToList();

            return stockPriceList;
        }

        /// <summary>
        /// 출연자 수익률 업데이트 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="earningRateSum"></param>
        public void UpdateEarningRate(int seq, double earningRateSum)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    tblTradingStarUser user = GetUser(seq);
                    if (user != null)
                    {
                        user.earningRateSum = earningRateSum;
                        db49_wowtv.tblTradingStarUser.AddOrUpdate(user);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 출연자 평균수익 업데이트
        /// </summary>
        /// <param name="regSeq"></param>
        /// <param name="totalHavaRateText"></param>
        public void UpdateTotalHavaRateText(int regSeq, double totalHavaRateText)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    tblTradingStarUser user = GetUser(regSeq);
                    if (user != null)
                    {
                        user.totalHavaRateText = totalHavaRateText;
                        db49_wowtv.tblTradingStarUser.AddOrUpdate(user);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 종목 검색
        /// </summary>
        /// <param name="condition">종목 이름</param>
        /// <returns></returns>
        public ListModel<tblStockBatch> GetStockList(StockBatchCondition condition)
        {
            ListModel<tblStockBatch> resultData = new ListModel<tblStockBatch>();
            var list = db22_stock.tblStockBatch.AsQueryable();
            list = list.OrderByDescending(x => x.korName);


            //종목 검색
            list = list.Where(x => x.korName.Contains(condition.SearchText));

            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }

                list = list.OrderByDescending(x => x.korName).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }


            resultData.ListData = list.ToList();

            return resultData;
        }

        /// <summary>
        /// 별점 수정
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="starMark"></param>
        public void UpdateStarMark(int seq, float starMark)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    tblTradingStarTrade trade = GetTrade(seq);
                    if (trade != null)
                    {
                        trade.starMark = starMark;
                        db49_wowtv.tblTradingStarTrade.AddOrUpdate(trade);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

    }
}