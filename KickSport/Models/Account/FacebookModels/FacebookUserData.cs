﻿using Newtonsoft.Json;

namespace KickSport.Web.Models.Account.FacebookModels
{
    public class FacebookUserData
    {
        public long Id { get; set; }

        public string Email { get; set; }

        [JsonProperty("email")]
        public string Username { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
