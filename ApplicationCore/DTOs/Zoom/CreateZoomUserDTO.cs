using Newtonsoft.Json;

namespace ApplicationCore.DTOs
{
    public class CreateZoomUserDTO
    {
        public CreateZoomUserDTO()
        {
            Action = "create";
        }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("user_info")]
        public ZoomUserInfo UserInfo { get; set; }
    }

    public class ZoomUserInfo
    {
        public ZoomUserInfo()
        {
            Type = UserPlanType.Basic;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("type")]
        public UserPlanType Type { get; set; }

        /// <summary>
        /// Max Length 64
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Max Length 64
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Only used for Action.AutoCreate
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
