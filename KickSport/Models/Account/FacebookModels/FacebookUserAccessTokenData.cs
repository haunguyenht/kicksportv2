using Newtonsoft.Json;

namespace KickSport.Web.Models.Account.FacebookModels
{
    public class FacebookUserAccessTokenData
    {
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }
}
