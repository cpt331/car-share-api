//======================================
//
//Name: TemplateUpdateRequest.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels.Admin
{
    public class TemplateUpdateRequest
    {
        //Template update request provides objects that relate to the template 
        //table within a database including Fields that hold the subject,
        //title, the email body and email footer

        public string Subject { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
    }
}