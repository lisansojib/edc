using Newtonsoft.Json;
using System;

namespace ApplicationCore.DTOs
{
    public class CreateingZoomMeetingDTO
    {
        public string Topic { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Agenda { get; set; }
        public ZoomMeetingSettings Settings { get; set; }
    }

    public class CreateZoomMeeting 
    {
        public CreateZoomMeeting()
        {
            Type = ZoomMeetingType.Scheduled;
            Password = ExtensionMethods.GeneratePassword(6, 2);
            Timezone = "America/New_York";
        }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("type")]
        public ZoomMeetingType Type { get; set; }

        /// <summary>
        /// Should only be used for Scheduled and / or recurring webinars with fixed time
        /// </summary>
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Meeting duration minutes. Used for schedule meetings only.
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// If you are creating this meeting on behalf of another user. 
        /// Then provide that user id as scheduled for.
        /// </summary>
        [JsonProperty("schedule_for")]
        public string ScheduleFor { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("agenda")]
        public string Agenda { get; set; }

        /// <summary>
        /// Use this onle for a meeting with Recurring meeting with fixed time.
        /// </summary>
        [JsonProperty("recurrence")]
        public ZoomMeetingRecurrence Recurrence { get; set; }

        [JsonProperty("settings")]
        public ZoomMeetingSettings Settings { get; set; }
    }

    public class ZoomMeetingRecurrence
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("repeat_interval")]
        public string RepeatInterval { get; set; }

        [JsonProperty("weekly_days")]
        public string WeeklyDays { get; set; }

        [JsonProperty("monthly_day")]
        public string MonthlyDay { get; set; }

        [JsonProperty("monthly_week")]
        public string MonthlyWeek { get; set; }

        [JsonProperty("monthly_week_day")]
        public string MonthlyWeekDay { get; set; }

        [JsonProperty("end_times")]
        public string EndTimes { get; set; }

        [JsonProperty("end_date_time")]
        public string EndDateTime { get; set; }
    }

    public class ZoomMeetingSettings
    {
        public ZoomMeetingSettings()
        {
            ApprovalType = ZoomMeetingApproval.Manual;
            RegistrationType = ZoomMeetingRegistrationType.Attendees_register_once_and_can_attend_any_of_the_occurrences;
            Audio = ZooomMeetingAudioType.VOIP;
            AutoRecording = ZooomMeetingAutoRecording.NONE;
        }

        [JsonProperty("host_video")]
        public bool HostVideo { get; set; }

        [JsonProperty("participant_video")]
        public bool ParticipantVideo { get; set; }

        /// <summary>
        /// Host meeting in china
        /// </summary>
        [JsonProperty("cn_meeting")]
        public bool CnMeeting { get; set; }

        /// <summary>
        /// Host meeting in India
        /// </summary>
        [JsonProperty("in_meeting")]
        public bool InMeeting { get; set; }

        /// <summary>
        /// Allow participants to join the meeting before the host starts the meeting. This field can only used for scheduled or recurring meetings. If waiting room is enabled, the join before host setting will be disabled.
        /// </summary>
        [JsonProperty("join_before_host")]
        public bool JoinBeforeHost { get; set; }

        [JsonProperty("mute_upon_entry")]
        public bool MuteUponEntry { get; set; }

        [JsonProperty("watermark")]
        public bool Watermark { get; set; }

        [JsonProperty("use_pmi")]
        public bool UsePmi { get; set; }

        [JsonProperty("approval_type")]
        public ZoomMeetingApproval ApprovalType { get; set; }

        [JsonProperty("registration_type")]
        public ZoomMeetingRegistrationType RegistrationType { get; set; }

        [JsonProperty("audio")]
        public string Audio { get; set; }

        [JsonProperty("auto_recording")]
        public string AutoRecording { get; set; }

        [JsonProperty("enforce_login")]
        public string EnforceLogin { get; set; }

        [JsonProperty("enforce_login_domains")]
        public string EnforceLoginDomains { get; set; }

        [JsonProperty("alternative_hosts")]
        public string AlternativeHosts { get; set; }

        [JsonProperty("global_dial_in_countries")]
        public string[] GlobalDialInCountries { get; set; }

        [JsonProperty("registrants_email_notification")]
        public string RegistrantsEmailNotification { get; set; }
    }
}
