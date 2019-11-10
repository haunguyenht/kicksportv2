using Microsoft.AspNetCore.Mvc;

namespace KickSport.Web.Models.Common
{
    public class BadRequestViewModel : ProblemDetails
    {
        public string Message { get; set; }
    }
}
