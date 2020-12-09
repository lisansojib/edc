using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class ListZoomMeeting
    {
        [JsonProperty("page_count")]
        public long PageCount { get; set; }

        [JsonProperty("page_number")]
        public long PageNumber { get; set; }

        [JsonProperty("page_size")]
        public long PageSize { get; set; }

        [JsonProperty("total_records")]
        public long TotalRecords { get; set; }

        [JsonProperty("meetings")]
        public IEnumerable<ZoomMeetingDTO> Meetings { get; set; }
    }

    public class ZoomMeetingDTO
    {
        [JsonProperty("uuid")]
        public string UUId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("host_id")]
        public string HostId { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("join_url")]
        public Uri JoinUrl { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
