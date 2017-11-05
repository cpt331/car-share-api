namespace CarShareApi.ViewModels.Admin
{
    public class TemplateViewModel
    {
        //The templateview model provides access to the fields 
        //from the template table that will hold subject title body and footer

        public string Subject { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
    }
}