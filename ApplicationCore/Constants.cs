namespace ApplicationCore
{
    public class Constants
    {
        public const string SYMMETRIC_SECURITY_KEY = "8b327b47-2e48-4116-9134-dcbcd5aff40b";
        public const string THUMBNAIL_IMAGE = "100x100";
        public const string MAX_ZOOM_USER_ID = "QBzo0AMOT6WYB8ywmfuXog";
        public const int DEFAULT_ZOOM_MEETING_DURATION = 30;
    }

    public static class UserRoles
    {
        public const string User = "User";
        public const string Admin = "Admin";
    }

    public static class GuestRoles
    {
        public const string GUEST = "Guest";
        public const string MEMBER = "Member";
        public const string SPEAKER = "Speaker";
    }

    public static class ErrorTypes
    {
        public const string InternalServerError = "Internal Server Error.";
        public const string BadRequest = "Bad Rquest.";
    }

    public static class ErrorMessages
    {
        public const string ItemNotFound = "Item not found.";
        public const string AuthenticatinError = "Authentication error.";
        public const string MissingRequiredItem = "Missing required item.";
        public const string ElasticSearchError = "Elastic search error.";
    }

    public static class AdditionalClaimTypes
    {
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string HasAds = "HasAds";
        public const string ContactEmail = "PrimaryEmail";
        public const string ContactNo = "ContactNo";
        public const string PhotoUrl = "PhotoUrl";
        public const string FullName = "FullName";
        public const string IsGuest = "IsGuest";
        public const string ZoomUserId = "ZoomUserId";
    }

    public static class ValueFieldTypes
    {
        public const int SPEAKERS = 1;
        public const int SPONSORS = 2;
    }

    public static class ValueFieldTypeNames
    {
        public const string GraphTypeName = "Types of Graphs";
        public const string EventTypeName = "Types of Events";
        public const string GuestTypeName = "Types of Guests";
    }

    public static class UploadFolders
    {
        public const string UPLOAD_PATH = "uploads";
        public const string COMPANIES = "companies";
        public const string ANNOUNCEMENTS = "announcements";
        public const string PARTICIPANTS = "participants";
        public const string SPONSORS = "sponsors";
        public const string SPEAKERS = "speakers";
        public const string EVENTS = "events";
    }

    public static class PreviewFileType
    {
        public const string IMAGE = "image";
        public const string AUDIO = "audio";
        public const string VIDEO = "video";
        public const string OFFICE = "office";
        public const string PDF = "pdf";
        public const string GDOCS = "gdocs";
        public const string TEXT = "text";
        public const string HTML = "html";
    }

    public static class ZooomMeetingAudioType
    {
        public const string BOTH = "both";
        public const string TELEPHONY = "telephony";
        public const string VOIP = "voip";
    }

    public static class ZooomMeetingAutoRecording
    {
        public const string LOCAL = "local";
        public const string CLOUD = "cloud";
        public const string NONE = "none";
    }
}
