using System.Web.Http;
using NEC.TEAM.WebAPI.Models;

namespace NEC.TEAM.WebAPI.Controllers
{
    [RoutePrefix("api/caseManagement")]
    public class CaseController : ApiController
    {
        const string VIDEO = "Video";
        const string PHOTO = "Photo";
        const string DOCUMENT = "Document";

        [Route("GetLinkedCases", Name = "GetLinkedCases")]
        public DigitalCaseDto GetLinkedCases()
        {
            #region Case 1

            DigitalCaseDto case1 = new DigitalCaseDto();
            case1.CaseId = "case10011010101";
            case1.CaseNumber = 1001;
            case1.CaseTitle = "Case 1";

            MediaDto media1 = new MediaDto();
            media1.ContentId = "media11111";
            media1.Name = "Photo 1";
            media1.ContentType = PHOTO;
            media1.URL = "http://criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot7zz.jpg";

            MediaDto media2 = new MediaDto();
            media2.ContentId = "media11112";
            media2.Name = "Video 1";
            media2.ContentType = VIDEO;
            media2.URL = "http://www.criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot15zz.jpg";

            MediaDto media3 = new MediaDto();
            media3.ContentId = "media11113";
            media3.Name = "Document 1";
            media3.ContentType = DOCUMENT;
            media3.URL = "http://www.criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot13zz.jpg";

            case1.AddMedia(media1);
            case1.AddMedia(media2);
            case1.AddMedia(media3);

            #endregion

            #region Case 2

            DigitalCaseDto case2 = new DigitalCaseDto();
            case2.CaseId = "case10011010122";
            case2.CaseNumber = 1002;
            case2.CaseTitle = "Case 2";

            MediaDto media4 = new MediaDto();
            media4.ContentId = "media11121";
            media4.Name = "Photo 3";
            media4.URL = "http://criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot7zz.jpg";

            MediaDto media5 = new MediaDto();
            media5.ContentId = "media11122";
            media5.Name = "Photo 4";
            media5.URL = "http://www.criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot15zz.jpg";

            MediaDto media6 = new MediaDto();
            media6.ContentId = "media11123";
            media6.ContentType = DOCUMENT;
            media6.Name = "Document 5";
            media6.URL = "http://www.criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot13zz.jpg";

            case2.AddMedia(media4);
            case2.AddMedia(media5);
            case2.AddMedia(media6);

            #endregion

            #region Case 3

            DigitalCaseDto case3 = new DigitalCaseDto();
            case3.CaseId = "cas10011010122";
            case3.CaseNumber = 1003;
            case3.CaseTitle = "Case 3";

            MediaDto media7 = new MediaDto();
            media7.ContentId = "media11121";
            media7.Name = "Photo 30";
            media7.URL = "http://criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot7zz.jpg";

            MediaDto media8 = new MediaDto();
            media8.ContentId = "media11122";
            media8.Name = "Photo 31";
            media8.URL = "http://www.criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot15zz.jpg";

            MediaDto media9 = new MediaDto();
            media9.ContentId = "media11123";
            media9.ContentType = DOCUMENT;
            media9.Name = "Document 5";
            media9.URL = "http://www.criminaljusticeschools.com/wp-content/uploads/2013/01/mugshot13zz.jpg";

            case3.AddMedia(media7);
            case3.AddMedia(media8);
            case3.AddMedia(media9);

            #endregion

            case1.LinkToCase(case2);
            case1.LinkMedia("Match", media1, media4);

            case2.LinkToCase(case3);
            case2.LinkMedia("Referred in Doc", media4, media9, Direction.To);

            return case1;
        }
    }
}
