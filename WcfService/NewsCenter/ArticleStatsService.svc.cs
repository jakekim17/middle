﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Stats;
using Wow.Tv.Middle.Model.Db89.wowbill;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    [ServiceContract]
    public interface IArticleStatsService
    {
        [OperationContract]
        ListModel<NUP_ARTICLE_SELECT_Result> GetArticleSearchList(StatsCondition condition);

        [OperationContract]
        List<NUP_ARTICLE_PROVIDE_SELECT_Result> GetProvideSearchList(StatsCondition condition);

        [OperationContract]
        List<NUP_FREELANCER_TRAFFIC_SELECT_Result> GetFreelancerTrafficList(StatsCondition condition);

        [OperationContract]
        List<NUP_FREELANCER_ARTICLE_RANK_SELECT_Result> GetFreelancerArticleRankList(StatsCondition condition);

        [OperationContract]
        PresentMember GetPresentMember(DateTime startDateTime, DateTime endDateTime);

        [OperationContract]
        void SaveTrafficRank(int rank, LoginUser loginUser);

        [OperationContract]
        string GetArtIdbyWriter(string compCode, string artId);

        [OperationContract]
        int ReportCount(DateTime startDateTime, DateTime endDateTime,string contentSeq);
    }
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "ArticleStatsService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 ArticleStatsService.svc나 ArticleStatsService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class ArticleStatsService : IArticleStatsService
    {
        public ListModel<NUP_ARTICLE_SELECT_Result> GetArticleSearchList(StatsCondition condition)
        {
            return new StatsBiz().GetArticleSearchList(condition);
        }

        public List<NUP_ARTICLE_PROVIDE_SELECT_Result> GetProvideSearchList(StatsCondition condition)
        {
            return new StatsBiz().GetProvideSearchList(condition);
        }

        public List<NUP_FREELANCER_TRAFFIC_SELECT_Result> GetFreelancerTrafficList(StatsCondition condition)
        {
            return new StatsBiz().GetFreelancerTrafficList(condition);
        }

        public List<NUP_FREELANCER_ARTICLE_RANK_SELECT_Result> GetFreelancerArticleRankList(StatsCondition condition)
        {
            return new StatsBiz().GetFreelancerArticleRankList(condition);
        }

        public PresentMember GetPresentMember(DateTime startDateTime, DateTime endDateTime)
        {
            return new StatsBiz().GetPresentMember(startDateTime, endDateTime);
        }

        public void SaveTrafficRank(int rank, LoginUser loginUser)
        {
            new StatsBiz().SaveTrafficRank(rank,loginUser);
        }

        public string GetArtIdbyWriter(string compCode, string artId)
        {
            return new StatsBiz().GetArtIdbyWriter(compCode, artId);

        }

        public int ReportCount(DateTime startDateTime, DateTime endDateTime,string menuSeq)
        {
            return new StatsBiz().ReportCount(startDateTime, endDateTime,menuSeq);
        }
    }
}
