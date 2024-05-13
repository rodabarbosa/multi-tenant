using System.Net;

namespace MultiTenant.Domain.Models;

public class Response<T>(bool success, int code, T? data, string? message)
{
    public Response(bool success, HttpStatusCode code, T? data, string? message)
        : this(success, (int)code, data, message)
    {
    }

    public Response()
        : this(false, 500, default, "UNKNOWN")
    {
    }

    public T? Data { get; set; } = data;
    public bool Success { get; set; } = success;
    public int Code { get; set; } = code;
    public string? Message { get; set; } = message;
}
