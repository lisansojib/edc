using ApplicationCore;
using System.Net;

namespace Presentation.Models
{
    public class ErrorResponseModel
    {
        public ErrorResponseModel()
        {
        }

        public ErrorResponseModel(string type, string title, HttpStatusCode status, string traceId, object errors)
        {
            Type = type;
            Title = title;
            Status = status;
            TraceId = traceId;
            Errors = errors;
        }

        public string Type { get; set; }
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        public string TraceId { get; set; }
        public object Errors { get; set; }
    }

    public class BadRequestResponseModel : ErrorResponseModel
    {
        public BadRequestResponseModel(string title, object errors)
            : base(ErrorTypes.BadRequest, title, HttpStatusCode.BadRequest, "", errors)
        {
        }
    }

    public class InternalServerErrorResponseModel : ErrorResponseModel
    {
        public InternalServerErrorResponseModel(string title, string traceId, object errors)
            : base(ErrorTypes.InternalServerError, title, HttpStatusCode.InternalServerError, traceId, errors)
        {
        }
    }
}
