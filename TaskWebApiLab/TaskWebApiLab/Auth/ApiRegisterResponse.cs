namespace TaskWebApiLab.Auth
{
    public class ApiRegisterResponse(string? status, string? message)
    {
        public string? Status { get; set; } = status;
        public string? Message { get; set; } = message;
    }
}
