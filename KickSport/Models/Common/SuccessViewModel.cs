namespace KickSport.Web.Models.Common
{
    public class SuccessViewModel<TModel>
    {
        public string Message { get; set; }

        public TModel Data { get; set; }
    }
}
