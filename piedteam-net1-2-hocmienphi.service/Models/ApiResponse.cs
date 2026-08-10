namespace piedteam_net1_2_hocmienphi.service.Models;

public class ApiResponse
{
    public bool IsSuccess { get; set; } //Kq trả ra có thành công k ?
    public required string Message { get; set; } //Login thành công | Login thất bại
    public object? Data { get; set; }
    public object? Errors { get; set; }
    public string? TraceId { get; set; }
    // Mỗi request sẽ được định danh 1 traceId
    // TraceId dùng để phân biệt các request khác nhau 
    
    public DateTime TimestampUtc { get; set; }
}

public static class ResponseBuilder
{
    public static ApiResponse SuccessResponse(object? data, string message, string? traceId = null)
    {
        return new ApiResponse()
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }
    
    public static ApiResponse ErrorResponse(object? error, string message, string? traceId = null)
    {
        return new ApiResponse()
        {
            IsSuccess = false,
            Message = message,
            Errors = error,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }
}