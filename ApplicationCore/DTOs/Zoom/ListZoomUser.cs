using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class ListZoomUser
    {
        [JsonProperty("page_count")]
        public long PageCount { get; set; }

        [JsonProperty("page_number")]
        public long PageNumber { get; set; }

        [JsonProperty("page_size")]
        public long PageSize { get; set; }

        [JsonProperty("total_records")]
        public long TotalRecords { get; set; }

        [JsonProperty("users")]
        public IEnumerable<ZoomUser> Users { get; set; }
    }

    public class ZoomUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("pmi")]
        public long Pmi { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("verified")]
        public long Verified { get; set; }

        [JsonProperty("dept")]
        public string Dept { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("last_login_time")]
        public DateTimeOffset LastLoginTime { get; set; }

        [JsonProperty("last_client_version")]
        public string LastClientVersion { get; set; }

        [JsonProperty("pic_url")]
        public Uri PicUrl { get; set; }

        [JsonProperty("im_group_ids")]
        public string[] ImGroupIds { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
