using piedteam_net1_2_hocmienphi.service.Models;

namespace piedteam_net1_2_hocmienphi.api.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
            /*
            // anh tạo ra 1 cái try catch.
            // Sau đó anh ngồi chờ kết quả của cái Request này trả ra 
            // Req lúc này tại vì bị next rồi, req sẽ đi xuống controller, service, repo.
            // Vì await ở đây nên mình có thể nhận được Response của Req
            // Response của Req ở đây có 2 trường hợp: 
                // 200: Data bình thường 
                // 1 Exception Error
                    // Catch nó. 
            
            // Theo ae tại sao phải có Global Exception.
                // Hạn chế duplicate logic try-catch 
                // Mình không thể cover hết tất cả các Exception được 
                // Tránh bị crash app vì nếu có 1 exception k được bắt thì app nổ 
                */
        }
        catch (Exception ex)
        {
            var statusCode = MapStatusCode(ex);
            var errorMessage = GetErrorMessage(ex, statusCode);
            var detailError  = BuildErrorDetail(ex);
            
            // HttpContext context. Context này đại diện cho request và response 
            // Nếu muốn độ lại status của response thì tương tác với Context 
            
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            
            var response = ResponseBuilder.ErrorResponse(null, errorMessage, context.TraceIdentifier);
            
            await context.Response.WriteAsJsonAsync(response);
        }
    }
    
    // Có được Exception, thì mình cần 2 thứ để trả ra.
        // Code: 400, 401, 422, 500
        // Message của Exception 

        private static int MapStatusCode(Exception ex)
        {
            return ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
        
        private static string GetErrorMessage(Exception ex, int statusCode)
        {
            return statusCode >= 500 ? "Unexpected error occurred" : ex.Message;
        }

        private static object? BuildErrorDetail(Exception ex)
        {
            return new
            {
                detail = ex.Message,
                exceptionType = ex.GetType().FullName,
                innerDetail = ex.InnerException?.Message,
                rootCauseDetail = ex.GetBaseException().Message
            };
        }
}


