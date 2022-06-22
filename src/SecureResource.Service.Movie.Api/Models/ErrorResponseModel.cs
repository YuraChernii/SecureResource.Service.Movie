using System.Net;

namespace SecureResource.Service.Movie.Api.Models
{
    public class ErrorResponseModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ErrorResponseModel()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="type">Error Type</param>
        /// <param name="title">Error Title</param>
        /// <param name="status">Http status <see cref="HttpStatusCode"/></param>
        /// <param name="traceId">Trace Id</param>
        /// <param name="errors">Error object</param>
        public ErrorResponseModel(string type, string title, HttpStatusCode status, string traceId, object errors)
        {
            Type = type;
            Title = title;
            Status = status;
            TraceId = traceId;
            Errors = errors;
        }

        /// <summary>
        /// Error Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Error Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Http Status Code <see cref="HttpStatusCode"/>
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Trace Id
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// Error object
        /// </summary>
        public object Errors { get; set; }
    }
}
