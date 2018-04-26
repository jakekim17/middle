using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Wow
{
    public static class WowExtensionMethod
    {
        /// <summary>
        /// 기사 By line
        /// </summary>
        /// <param name="reporterName"></param>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public static string NewsByline(string reporterName)
        {
            string byline = string.Empty;

            switch (reporterName)
            {
                case "디지털뉴스부" : byline = "디지털뉴스부 "; break;
                case "증권콘텐츠팀" : byline = "증권콘텐츠팀 "; break;
                case "인터넷뉴스팀" : byline = "인터넷뉴스팀 "; break;
                case "온라인뉴스팀" : byline = "온라인뉴스팀 "; break;
                case "와우스타뉴스팀" : byline = "와우스타뉴스팀 "; break;
                case "김상인" : 
                case "김미영" : 
                case "정소연" : 
                case "편집국" : 
                case "황동식" : byline = "데일리뉴스팀 "; break;
                case "김민재" : 
                case "와우스포츠" : byline = "스포츠팀 "; break;
                case "권태건" :
                case "김범태" :
                case "김시완" :
                case "김진영" : 
                case "박승호" : 
                case "이기호" : 
                case "이원정" : 
                case "정영훈" : 
                case "최상섭" : byline = string.Format("데일리뉴스팀 {0} ", reporterName); break;
                case "김진태" :
                case "나성민" :
                case "심재철" :
                case "정진호" : byline = string.Format("스포츠팀 {0} ", reporterName); break;
                case "임해니" :
                case "정한영" :
                case "이정현" :
                case "노상은" :
                case "송민종" : byline = ""; break;

                default:
                    byline = string.Format("{0} ", reporterName);
                    break;
            }

            return byline;
        }
        

        /// <summary>
        /// 뉴스 단독,공시,시황 아이콘
        /// </summary>
        /// <param name="gubun">단독,공시,시황</param>
        /// <returns>단독,공시,시황 색상&Html</returns>
        public static string NewsGugunIcon(string gubun)
        {
            return NewsGugunIcon(gubun, "S");
        }

        /// <summary>
        /// 뉴스 단독,공시,시황 아이콘
        /// </summary>
        /// <param name="gubun">단독,공시,시황</param>
        /// <param name="iconSize">아이콘 사이즈(B,S)</param>
        /// <returns>단독,공시,시황 색상&Html</returns>
        public static string NewsGugunIcon(string gubun, string iconSize)
        {
            string gubunHtml = string.Empty;

            if(!string.IsNullOrEmpty(gubun))
            {
                if (iconSize.Equals("B"))
                {
                    switch (gubun.Trim())
                    {
                        case "단독":
                            gubunHtml = "<span class=\"box-icon01\">단독</span>";
                            break;
                        case "공시":
                            gubunHtml = "<span class=\"box-icon01 type2\">공시</span>";
                            break;
                        case "시황":
                            gubunHtml = "<span class=\"box-icon01 type2\">시황</span>";
                            break;
                        default:
                            gubunHtml = "";
                            break;
                    }
                }
                else
                {
                    switch (gubun.Trim())
                    {
                        case "단독":
                            gubunHtml = "<span class=\"box-icon\">단독</span>";
                            break;
                        case "공시":
                            gubunHtml = "<span class=\"box-icon type02\">공시</span>";
                            break;
                        case "시황":
                            gubunHtml = "<span class=\"box-icon type06\">시황</span>";
                            break;
                        default:
                            gubunHtml = "";
                            break;
                    }
                }
            }
            
            return gubunHtml;
        }

        /// <summary>
        /// 뉴스 단독,공시,시황 아이콘
        /// </summary>
        /// <param name="gubun">단독,공시,시황</param>
        /// <returns>단독,공시,시황 색상&Html</returns>
        public static string NewsMobileGubunIcon(string gubun)
        {
            return NewsMobileGubunIcon(gubun, "S");
        }

        /// <summary>
        /// 뉴스 단독,공시,시황 아이콘
        /// </summary>
        /// <param name="gubun">단독,공시,시황</param>
        /// <param name="iconSize">아이콘 사이즈(B,S)</param>
        /// <returns>단독,공시,시황 색상&Html</returns>
        public static string NewsMobileGubunIcon(string gubun, string iconSize)
        {
            string gubunHtml = string.Empty;

            if (!string.IsNullOrEmpty(gubun))
            {
                if (iconSize.Equals("B"))
                {
                    switch (gubun.Trim())
                    {
                        case "단독":
                            gubunHtml = "<span class=\"text-icon\">단독</span>";
                            break;
                        case "공시":
                            gubunHtml = "<span class=\"text-icon type2\">공시</span>";
                            break;
                        case "시황":
                            gubunHtml = "<span class=\"text-icon type2\">시황</span>";
                            break;
                        default:
                            gubunHtml = "";
                            break;
                    }
                }
                else
                {
                    switch (gubun.Trim())
                    {
                        case "단독":
                            gubunHtml = "<span class=\"text-icon\">단독</span>";
                            break;
                        case "공시":
                            gubunHtml = "<span class=\"text-icon type02\">공시</span>";
                            break;
                        case "시황":
                            gubunHtml = "<span class=\"text-icon type14\">시황</span>";
                            break;
                        default:
                            gubunHtml = "";
                            break;
                    }
                }
            }

            return gubunHtml;
        }

        /// <summary>
        /// 텍스트 강조
        /// </summary>
        /// <param name="value">강조할 텍스트</param>
        /// <returns></returns>
        public static string ToStrong(this string value, string boldYn)
        {
            if(boldYn == "Y")
            {
                value = string.Format("<strong>{0}</strong>", value);
            }

            return value;
        }


        /// <summary>
        /// profile no image
        /// </summary>
        /// <param name="imageType">이미지 타입</param>
        /// <returns></returns>
        public static string NoImageProfile(string imageType)
        {
            string wowTvStyle = System.Configuration.ConfigurationManager.AppSettings["WowTvStyle"];

            string imageUrl = string.Format(@"{0}/images/common/no_image_profile_144.gif", wowTvStyle);

            if (String.IsNullOrEmpty(imageType))
            {
                imageUrl = string.Format(@"{0}/images/common/no_image_profile_{1}.gif", wowTvStyle, imageType.ToLower());
            }

            return imageUrl;
        }


        #region 뉴스 썸네일 이미지

        /// <summary>
        /// 썸네일 타입 --> 이미지 경로의 IMG TYPE
        /// </summary>
        /// <param name="thumbNailType"></param>
        /// <returns></returns>
        public static string ThumbNailTypeToImgType(this string thumbnailType)
        {
            string imgType = string.Empty;

            switch (thumbnailType.ToUpper())
            {
                case "16B": imgType = "IMG01"; break;
                case "16M": imgType = "IMG02"; break;
                case "16S": imgType = "IMG03"; break;
                case "11B": imgType = "IMG04"; break;
                case "11M": imgType = "IMG05"; break;
                case "11S": imgType = "IMG06"; break;
                case "34M": imgType = "IMG07"; break;
                case "34S": imgType = "IMG08"; break;
                default: imgType = "IMG03"; break;
            }

            return imgType;
        }

        /// <summary>
        /// 이미지 로딩 에러시 대체 이미지
        /// </summary>
        /// <param name="thumbnailType"></param>
        /// <returns></returns>
        public static string NewsThumbnailOnError(string thumbnailType)
        {
            string wowTvStyle = System.Configuration.ConfigurationManager.AppSettings["WowTvStyle"];

            Random rndNum = new Random();
            int imgNum = rndNum.Next(1, 3);
            //Wow.Fx.WowLog.Write(" NewsThumbnailOnError imgNum :  " + imgNum.ToString());
            return string.Format(@"{0}/images/common/no_image_{1}_{2}.jpg", wowTvStyle, thumbnailType.ToLower(), imgNum);
        }

        /// <summary>
        /// 뉴스 스탠드 썸네일 이미지
        /// </summary>
        /// <param name="newsStandImageFile">관리자 설정</param>
        /// <param name="thumbNailType">썸네일 사이트 타입</param>
        /// <param name="thumbNailFile">썸네일 파일경로</param>
        /// <param name="vodNum">동영상 파일번호</param>
        /// <param name="imageDir">이미지 폴더</param>
        /// <param name="imagFile">이미지 파일명</param>
        /// <param name="artDate">등록일</param>
        /// <returns></returns>
        public static string NewsStandThumbnailPath(string newsStandImageFile, string thumbnailType, string thumbnailFile, int? vodNum, string imageDir, string imagFile, DateTime artDate)
        {
            string thumbnailFilePath = string.Empty;

            if (!String.IsNullOrEmpty(newsStandImageFile))
            {
                if (newsStandImageFile.IndexOf(@"\upload_view/") > -1 && newsStandImageFile.IndexOf(@"\upload_view/") == 0)
                {
                    newsStandImageFile = newsStandImageFile.Replace(@"\upload_view/", "http://image.wowtv.co.kr/wowtv_main/");
                    newsStandImageFile = newsStandImageFile.Replace(@"\", @"/");

                    thumbnailFilePath = newsStandImageFile;
                }
                else
                {
                    string uploadWebPathRoot = System.Configuration.ConfigurationManager.AppSettings["UploadWebPathRoot"];

                    newsStandImageFile = newsStandImageFile.Replace(@"/Admin/News/", uploadWebPathRoot + "/Admin/News/");
                    newsStandImageFile = newsStandImageFile.Replace(@"\upload_view/", uploadWebPathRoot + "/Admin/News/upload_view/");
                    newsStandImageFile = newsStandImageFile.Replace(@"\", @"/");

                    thumbnailFilePath = newsStandImageFile;
                }
            }
            else
            {
                thumbnailFilePath = NewsThumbnailPath(thumbnailType, thumbnailFile, vodNum, imageDir, imagFile, artDate);
            }

            return thumbnailFilePath;
        }


        /// <summary>
        /// 뉴스 썸네일 이미지
        /// </summary>
        /// <param name="thumbNailType">썸네일 사이트 타입</param>
        /// <param name="thumbNailFile">썸네일 파일경로</param>
        /// <param name="vodNum">동영상 파일번호</param>
        /// <param name="imageDir">이미지 폴더</param>
        /// <param name="imagFile">이미지 파일명</param>
        /// <param name="artDate">등록일</param>
        /// <returns></returns>
        public static string NewsThumbnailPath(string thumbnailType, string thumbnailFile, int? vodNum, string imageDir, string imagFile, DateTime artDate)
        {
            string thumbnailFilePath = string.Empty;

            if(vodNum == null)
            {
                vodNum = 0;
            }

            if (!String.IsNullOrEmpty(thumbnailType) && !String.IsNullOrEmpty(thumbnailFile))
            {
                string IMGTYPE = thumbnailType.ThumbNailTypeToImgType();

                thumbnailFile = thumbnailFile.Replace("IMG03", IMGTYPE);

                thumbnailFilePath = string.Format(@"http://img.wowtv.co.kr{0}", thumbnailFile);
            }
            else
            {
                if (vodNum >= 0 && !String.IsNullOrEmpty(imagFile) )
                {
                    thumbnailFilePath = thumbnailFilePath.NewsThumbNailOld(vodNum, imageDir, imagFile, artDate);
                }                   
            }

            if (String.IsNullOrEmpty(thumbnailFilePath))
            {
                string wowTvStyle = System.Configuration.ConfigurationManager.AppSettings["WowTvStyle"];

                Random rndNum = new Random();
                int imgNum = rndNum.Next(1, 3);
                //Wow.Fx.WowLog.Write(" imgNum :  " + imgNum.ToString());
                thumbnailFilePath = string.Format(@"{0}/images/common/no_image_{1}_{2}.jpg", wowTvStyle, thumbnailType.ToLower(), imgNum);
            }

            return thumbnailFilePath;
        }

        /// <summary>
        /// 뉴스 썸네일 이미지
        /// </summary>
        /// <param name="vodNum">동영상 NUM</param>
        /// <param name="imageDir">이미지 폴더</param>
        /// <param name="imagFile">이미지 파일명</param>
        /// <param name="artDate">등록일</param>
        /// <returns></returns>
        public static string NewsThumbNailOld(this string thumbNailFilePath,  int? vodNum, string imageDir, string imagFile, DateTime artDate)
        {
            string imageUrl = string.Empty;

            //영상 뉴스
            if (vodNum > 0)
            {
                if (!string.IsNullOrEmpty(imagFile) && !string.IsNullOrEmpty(imageDir) && imageDir.Length > 8) 
                {
                    imageDir = imageDir.Substring(0, 9);

                    //'신규 동영상 이미지 URL - VOD HD 전환이후부터 이미지 URL 변경됨.
                    if (artDate > DateTime.Parse("2016-10-11"))
                    {
                        imageUrl = string.Format("http://vodimg.wowtv.co.kr/{0}/{1}", imageDir, imagFile);
                    }
                    else
                    {
                        imageUrl = string.Format("http://vod1.wowtv.co.kr:8080/img/{0}/{1}/{2}", imageDir, imagFile.Substring(0, 13), imagFile);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(imagFile))
                {
                    if (imagFile.Substring(0, 7).ToLower().Equals("http://"))
                    {
                        imageUrl = imagFile;
                    }
                    else
                    {
                        imageUrl = string.Format("http://image.wownet.co.kr/static/news/{0}/{1}", imageDir, imagFile);
                    }
                }
            }

            return imageUrl;
        }
        #endregion
        

        /// <summary>
        /// 카드 뉴스 리스트 이미지
        /// </summary>
        /// <param name="imageDir">이미지 폴더</param>
        /// <param name="imagFile">이미지 파일명</param>
        /// <param name="artDate">등록일</param>
        /// <returns></returns>
        public static string NewsImage(string imageDir, string imagFile, DateTime artDate)
        {
            string imageUrl = string.Empty;
            if (!string.IsNullOrEmpty(imagFile))
            {
                if (imagFile.Substring(0, 7).ToLower().Equals("http://"))
                {
                    imageUrl = imagFile;
                }
                else
                {
                    imageUrl = string.Format("http://image.wownet.co.kr/static/news/{0}/{1}", imageDir, imagFile);
                }
            }
            return imageUrl;
        }

        /// <summary>
        /// 뉴스 리스트 이미지
        /// </summary>
        /// <param name="vodNum">동영상 NUM</param>
        /// <param name="imageDir">이미지 폴더</param>
        /// <param name="imagFile">이미지 파일명</param>
        /// <param name="artDate">등록일</param>
        /// <returns></returns>
        public static string NewsListImage(int? vodNum, string imageDir, string imagFile, DateTime artDate)
        {
            string imageUrl = string.Empty;

            //영상 뉴스
            if(vodNum > 0)
            {
                if (!string.IsNullOrEmpty(imagFile) && !string.IsNullOrEmpty(imageDir) && imageDir.Length>  8 )
                {
                    imageDir = imageDir.Substring(0, 9);

                    //'신규 동영상 이미지 URL - VOD HD 전환이후부터 이미지 URL 변경됨.
                    if (artDate > DateTime.Parse("2016-10-11"))
                    {
                        imageUrl = string.Format("http://vodimg.wowtv.co.kr/{0}/{1}", imageDir, imagFile);
                    }
                    else
                    {
                        imageUrl = string.Format("http://vod1.wowtv.co.kr:8080/img/{0}/{1}/{2}", imageDir, imagFile.Substring(0, 13), imagFile);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(imagFile))
                {
                    if (imagFile.Substring(0, 7).ToLower().Equals("http://"))
                    {
                        imageUrl = imagFile;
                    }
                    else
                    {
                        imageUrl = string.Format("http://image.wownet.co.kr/static/news/{0}/{1}", imageDir, imagFile);
                    }
                }
            }
 
            return imageUrl;
        }

        /// <summary>
        /// 뉴스 해시 태그 생성(1개 노출)
        /// </summary>
        /// <param name="tag">,로 구분된 태그</param>
        /// <returns></returns>
        public static string NewsHashTag(string sourceTag)
        {
            return NewsHashTag(sourceTag, 1, "");
        }


        /// <summary>
        /// 뉴스 해시 태그 생성(1개 노출 영역지정)
        /// </summary>
        /// <param name="sourceTag">,로 구분된 태그</param>
        /// <param name="sourceGubun">표현 영역 구분(READ[뉴스 상세])</param>
        /// <returns></returns>
        public static string NewsHashTag(string sourceTag, string sourceGubun)
        {
            return NewsHashTag(sourceTag, 1, sourceGubun);
        }

        /// <summary>
        /// 뉴스 해시 태그 생성(N개 노출)
        /// </summary>
        /// <param name="sourceTag">,로 구분된 태그</param>
        /// <param name="sourceGubun">표현 영역 구분(READ[뉴스 상세])</param>
        /// <returns></returns>
        public static string NewsHashTag(string sourceTag, int retCount, string sourceGubun)
        {
            ///정재민 추가
            if (string.IsNullOrWhiteSpace(sourceTag))
            {
                return string.Empty;
            }

            string[] words = sourceTag.Split(new char[] {','});
            StringBuilder sb = new StringBuilder();

            //파트너 구분 ex)$P506 [ $ + 파트너 아이디 ]
            if (!sourceTag.Substring(0, 1).Equals("$"))
            {
                int i = 0;

                if (retCount <= 0)
                {
                    retCount = 1;
                }

                if (retCount > words.Count())
                {
                    retCount = words.Count();
                }

                foreach (string hashTags in words)
                {
                    if (i < retCount)
                    {
                        string HashTagLink = string.Empty;

                        if (sourceGubun.Equals("READ"))
                        {
                            HashTagLink = string.Format("<span><a href=\"javascript:HashTagLink('{0}')\" class='hash-tag02'>{0}</a></span>", hashTags);
                        }
                        else
                        {
                            HashTagLink = string.Format("<a href=\"javascript:HashTagLink('{0}')\" class='hash-tag02'>{0}</a>", hashTags);
                        }                   

                        sb.AppendLine(HashTagLink);
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 뉴스 해시 태그 생성(N개 노출) 모바일
        /// </summary>
        /// <param name="sourceTag">,로 구분된 태그</param>
        /// <param name="sourceGubun">표현 영역 구분(READ[뉴스 상세])</param>
        /// <returns></returns>
        public static string NewsHashTagMobile(string sourceTag, int retCount, string sourceGubun)
        {
            ///정재민 추가
            if (string.IsNullOrWhiteSpace(sourceTag))
            {
                return string.Empty;
            }

            string[] words = sourceTag.Split(new char[] { ',' });
            StringBuilder sb = new StringBuilder();

            //파트너 구분 ex)$P506 [ $ + 파트너 아이디 ]
            if (!sourceTag.Substring(0, 1).Equals("$"))
            {
                int i = 0;

                if (retCount <= 0)
                {
                    retCount = 1;
                }

                if (retCount > words.Count())
                {
                    retCount = words.Count();
                }

                foreach (string hashTags in words)
                {
                    if (i < retCount)
                    {
                        string HashTagLink = string.Empty;

                        if (sourceGubun.Equals("READ"))
                        {
                            HashTagLink = string.Format("<span><a href=\"javascript:HashTagLink('{0}')\" class='txt-tag'>{0}</a></span>", hashTags);
                        }
                        else
                        {
                            HashTagLink = string.Format("<a href=\"javascript:HashTagLink('{0}')\" class='txt-tag'>{0}</a>", hashTags);
                        }

                        sb.AppendLine(HashTagLink);
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
            }
            return sb.ToString();
        }



        /// <summary>
        /// 연예 스포츠 태그
        /// </summary>
        /// <param name="sourceTag"></param>
        /// <returns></returns>
        public static string EntSpoHashTag(string sourceTag)
        {
            string str = "";
            if (sourceTag != null && sourceTag != "")
            {
                string[] words = sourceTag.Split(new char[] { ',' });
                //str = string.Format("<span class='hash-tag'>#{0}</span>", words[0]);
                str = string.Format("<span class=\"hash-tag\"><a href=\"javascript:HashTagLink('{0}')\" class='hash-tag02'>{0}</a></span>", words[0]);
            }

            return str;
        }

        public static string EntSpoMobileHashTag(string sourceTag)
        {
            string str = "";
            if (sourceTag != null && sourceTag != "")
            {
                string[] words = sourceTag.Split(new char[] { ',' });
                str = string.Format("<span class=\"text-icon type11\"><a href=\"javascript:NewsCommon.NewsHashTag('{0}')\">{0}</a></span>", words[0]);
            }

            return str;
        }

        /// <summary>
        /// 뉴스 CONTENT REPLACE
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string NewsContentReplace(string content, string compcode)
        {
            //이미지 Replace(한국경제TV 기사의 경우 이미지경로 변경 (2012.02.14))
            if (compcode != null )
            {
                if (compcode.Equals("WO"))
                {
                    content = content.Replace("http://newsimg.wowtv.co.kr/", "http://image.wowtv.co.kr/wowtv_news/");
                }
            }

            content = content.Replace("<div id=\"relnews_list\">", ""); // <- 특징주 내용의 하단에 이 태그가 들어가 있음 닫는 태그없이 있어. 레이아웃 깨짐발생
            content = content.Replace("<저작권자(c) 연합뉴스, 무단 전재-재배포 금지>", "&lt;저작권자(c) 연합뉴스, 무단 전재-재배포 금지&gt;"); // <--  Syntax error, unrecognized expression: 저작권자(c):has(span.icon)
            content = content.Replace("\r\n", "<br>");

            List<String> imageTagList = HtmlSourceToImgSrclist(content);

            foreach (var item in imageTagList)
            {
                int index = content.IndexOf(item);

                string imgUrl = content.Substring(index, (item.Length + 1));

                content = content.Replace(imgUrl, string.Format("{0} onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' style='display:block; margin:0 auto'", imgUrl));
            }

            /*
            content = content.Replace("src=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("Src=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("SrC=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("SRc=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("SRC=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("sRc=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("sRC=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            content = content.Replace("srC=", " onload='javascript:imgcheck(this,true)' onerror='javascript:imgcheck(this,false)' src=");
            */

            return content;
        }

        /// <summary>
        /// 뉴스 CONTENT REPLACE
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string NewsContentMobileReplace(string content)
        {
            content = content.Replace("<div id=\"relnews_list\">", ""); // <- 특징주 내용의 하단에 이 태그가 들어가 있음 닫는 태그없이 있어. 레이아웃 깨짐발생
            content = content.Replace("<저작권자(c) 연합뉴스, 무단 전재-재배포 금지>", "&lt;저작권자(c) 연합뉴스, 무단 전재-재배포 금지&gt;"); // <--  Syntax error, unrecognized expression: 저작권자(c):has(span.icon)
            content = content.Replace("\r\n", "<br>");

            List<String> imageTagList = HtmlSourceToImgSrclist(content);

            foreach (var item in imageTagList)
            {
                int index = content.IndexOf(item);

                string imgUrl = content.Substring(index, (item.Length + 1));

                content = content.Replace(imgUrl, string.Format("{0} onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' style='display:block; margin:0 auto'", imgUrl));
            }
            /*
            content = content.Replace("src=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("Src=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("SrC=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("SRc=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("SRC=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("sRc=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("sRC=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");
            content = content.Replace("srC=", " onload='javascript:imgMobilecheck(this,true)' onerror='javascript:imgMobilecheck(this,false)' src=");

            content = content.Replace("src=", " style='display:block; margin:0 auto' src=");
            */
            return content;
        }

        /// <summary>
        /// HTML 소스에서 img src 추출
        /// </summary>
        /// <param name="htmlSource">Html Source</param>
        /// <returns>List<string></returns>
        public static List<string> HtmlSourceToImgSrclist(string htmlSource)
        {
            List<string> listOfImgSrc = new List<string>();

            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";

            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                listOfImgSrc.Add(href);
            }

            return listOfImgSrc;
        }


        public static string ToDate(this DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                return dt.Value.ToDate();
            }
        }

        public static string ToDate(this DateTime dt)
        {
            if (dt.Year == 1)
            {
                return "";
            }
            else
            {
                return dt.ToString("yyyy-MM-dd");
            }
        }

        public static string ToDateTime(this DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                return dt.Value.ToDateTime();
            }
        }

        public static string ToDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 해당일자의 마지막 시간으로 셋팅한 값
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDateFinishTime(this DateTime dt)
        {
            DateTime dtFinishTimeDate = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return dtFinishTimeDate;
        }

        /// <summary>
        /// Generic Type Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToClone"></param>
        /// <returns></returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// html 태그 제거
        /// </summary>
        public static String RemoveTag(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"[<][a-z|A-Z|/](.|)*?[>]", "");
        }


        /// <summary>
        /// html 태그 제거 & Replace
        /// </summary>
        public static String RemoveTagAndReplace(string text, int textCutLen, bool isEllipsis)
        {
            text = text.Replace("&middot;", " ");
            text = text.Replace("&middot;", " ");
            text = text.Replace("&rsquo;", " ");
            text = text.Replace("&lsquo;", " ");
            text = text.Replace("&ldquo;", "\"");
            text = text.Replace("&rdquo;", "\"");
            text = text.Replace("&#39", " ");
            text = text.Replace("&#39;", " ");
            text = text.Replace("&quot;", " ");
            text = text.Replace("\r\n", " ");
            text = text.Replace("&middot;", "·");
            text = text.Replace(";", "");
            text = text.Replace("(c)", "");
            text = text.Replace("&amp", " ");
            text = text.Replace("&nbsp", " ");

            if (textCutLen > 0)
            {
                text = RemoveTag(text);

                text = SubStringWidthPad(text, textCutLen, isEllipsis);
            }

            return text;
        }

        /// <summary>
        /// 문자열을 특정 길이로 변환하고 남는 부분은 공백문자로 채운다
        /// </summary>
        /// <param name="str">처리할 문자열</param>
        /// <param name="len">최대 표현할 문자수</param>
        /// <returns></returns>
        public static string SubStringWidthPad(string str, int len)
        {
            return SubStringWidthPad(str, len, false);
        }

        /// <summary>
        /// 문자열을 특정 길이로 변환하고 남는 부분은 공백문자로 채운다
        /// </summary>
        /// <param name="str">처리할 문자열</param>
        /// <param name="len">최대 표현할 문자수</param>
        /// <param name="isEllipsis">말줄임 표시여부</param>
        /// <returns></returns>
        public static string SubStringWidthPad(string str, int len, bool isEllipsis)
        {
            /*
            // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
            int inCnt = Encoding.Default.GetByteCount(str);
            if (inCnt > len)
            {
                int i = 0;
                for (i = str.Length - 1; inCnt > len; i--)
                {
                    //ANSI 문자는 1, 이외의 문자는 2자리로 계산한다
                    if (str[i] > 0x7f)
                    {
                        inCnt -= 2;
                    }
                    else
                    {
                        inCnt -= 1;
                    }
                }

                // i는 마지막 문자 인덱스이고 substring 의 두번째 파라미터는 길이이기 때문에 + 1 한다.
                str = str.Substring(0, i + 1);

                // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
                inCnt = Encoding.Default.GetByteCount(str);
            }

            //PadRight(len) 이 맞겠지만 유니코드 처리가 되기 때문에 멀티바이트 문자도 1로
            //처리되어 길이가 일정하지 않기 때문에 아래와 같이 계산하여 Padding한다
            str = str.PadRight(str.Length + len - inCnt);

            if (isEllipsis)
            {
                str = string.Format("{0}...", str);
            }

            return str;
            */

            System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            string result = str;
            byte[] buf = myEncoding.GetBytes(str);
            int byteCount = myEncoding.GetByteCount(str);

            if (byteCount > len)
            {
                result = myEncoding.GetString(buf, 0, len);

                if (len != result.Length)
                {
                    result = myEncoding.GetString(buf, 0, len + 1);
                }
                result = result.Replace("?", "");
                result = string.Format("{0}...", result);
            }
            return result;
        }


        /// <summary>
        /// 뉴스 리스트 컨텐츠 내용 자르기
        /// </summary>
        /// <param name="strContent">내용</param>
        /// <returns></returns>
        public static string NewsListContentCut(string strContent, bool isImageEmpty)
        {
            //텍스트            
            if (isImageEmpty)
            {
                strContent = RemoveTagAndReplace(strContent, 196, true);
            }
            //이미지,동영상
            else
            {
                strContent = RemoveTagAndReplace(strContent, 134, true);
            }

            return strContent;
        }

        public static string CommentIcon(string LoginType)
        {
            var txt = "";
            switch (LoginType)
            {
                case "F":
                    txt = "<span class=\"icon-login-facebook\">Facebook</span><span class=\"division-bar\"></span>";
                    break;
                case "K":
                    txt = "<span class=\"icon-login-kakaostory\">Kakao</span><span class=\"division-bar\"></span>";
                    break;
                case "N":
                    txt = "<span class=\"icon-login-naver\">Naver</span><span class=\"division-bar\"></span>";
                    break;
                default:
                    txt = "";
                    break;
            }


            return txt;
        }

        public static string MobileCommentIcon(string LoginType)
        {
            var txt = "";
            switch (LoginType)
            {
                case "F":
                    txt = "<span class=\"sns-icon\"></span>";
                    break;
                case "K":
                    txt = "<span class=\"sns-icon type03\">";
                    break;
                case "N":
                    txt = "<span class=\"sns-icon type04\">";
                    break;
                default:
                    txt = "";
                    break;
            }


            return txt;
        }

        public static string CommentMobileIcon(string LoginType)
        {
            var txt = "";
            switch (LoginType)
            {
                case "F":
                    txt = "<span class=\"icon reply-facebook\">페이스북</span>";
                    break;
                case "K":
                    txt = "<span class=\"icon reply-kakaostory\">카카오</span>";
                    break;
                case "N":
                    txt = "<span class=\"icon reply-naver\">네이버</span>";
                    break;
                default:
                    txt = "";
                    break;
            }
            return txt;
        }

        public static string QuickMenuHashTag(string hashTag, int index)
        {
            var txt = "";
            switch (index)
            {
                case 0:
                    txt = "<span class=\"bg_red\" onclick=\"HashTagLink('"+hashTag+"')\">" + hashTag + "</span>";
                    break;
                case 1:
                    txt = "<span class=\"bg_green\" onclick=\"HashTagLink('" + hashTag + "')\">" + hashTag + "</span>";
                    break;
                case 2:
                    txt = "<span class=\"bg_grawn\" onclick=\"HashTagLink('" + hashTag + "')\">" + hashTag + "</span>";
                    break;
                default:
                    txt = "<span class=\"bg_blue\" onclick=\"HashTagLink('" + hashTag + "')\">" + hashTag + "</span>";
                    break;
            }
            return txt;
        }

        /// <summary>
        /// GetIsMobile 모바일 기기 체크
        /// http://detectmobilebrowsers.com/
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool GetIsMobile(string userAgent)
        {
            bool isMobile = false;
            string u = userAgent;
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                isMobile = true;
            }

            return isMobile;
        }

        /// <summary>
        /// GetIsHttpURL HttpUrl 형식인지 체크 
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool GetIsHttpURL(string linkUrl)
        {
            bool isHttpUrl = false;
            string pattern = @"^(http|https|ftp|)\://(\w*:\w*@)?[-\w.]+(:\d+)?(/([\w/_.]*(\?\S+)?)?)?$";
            Regex regex = new Regex(pattern);

            if ((regex.IsMatch(linkUrl)))
            {
                isHttpUrl = true;
            }

            return isHttpUrl;
        }
    }
}
