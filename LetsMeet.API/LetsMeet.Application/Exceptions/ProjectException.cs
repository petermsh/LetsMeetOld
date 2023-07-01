using System.Net;

namespace LetsMeet.Application.Exceptions;

public class ProjectException : Exception
{
    public HttpStatusCode ErrorCode { get; }

    public ProjectException(string message, HttpStatusCode errorCode = HttpStatusCode.BadRequest) : base(message)
    {
        ErrorCode = errorCode;
    }
}