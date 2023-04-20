namespace APMS.Common
{
    public class ApiCallResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}