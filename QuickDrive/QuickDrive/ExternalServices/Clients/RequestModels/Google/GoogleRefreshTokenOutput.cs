﻿using Newtonsoft.Json;

namespace QuickDrive.ExternalServices.ServiceClients.RequestModels
{
    public class GoogleRefreshTokenOutput
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}