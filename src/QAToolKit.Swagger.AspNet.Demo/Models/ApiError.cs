namespace QAToolKit.Swagger.AspNet.Demo.Models
{
    public class ApiError
    {
        public int StatusCode { get; }
        public string StatusDescription { get; }
        public string Message { get; }
        public int ErrorCode { get; }
    }
}
