using System.Net;

namespace Carvana
{
    public enum ResultStatus
    {
        Unspecified,
        ClientError,
        Succeeded = HttpStatusCode.OK,
        InvalidRequest = HttpStatusCode.BadRequest,
        MissingResource = HttpStatusCode.NotFound,
        ExpiredResource = HttpStatusCode.Gone,
        ProcessingError = HttpStatusCode.InternalServerError,
        DependencyFailure = HttpStatusCode.ServiceUnavailable,
        Conflict = HttpStatusCode.Conflict,
    }
}
