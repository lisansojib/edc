namespace ApplicationCore
{
    public class Constants
    {
        public const string SYMMETRIC_SECURITY_KEY = "8b327b47-2e48-4116-9134-dcbcd5aff40b";
        public const string THUMBNAIL_IMAGE = "100x100";
    }

    public static class UserRoles
    {
        public const string User = "User";
        public const string Admin = "Admin";
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
    }

    public static class ValueFieldTypes
    {
        public const int SPEAKERS = 1;
        public const int SPONSORS = 2;
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
}
