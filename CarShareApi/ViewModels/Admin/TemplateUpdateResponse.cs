namespace CarShareApi.ViewModels.Admin
{
    public class TemplateUpdateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}