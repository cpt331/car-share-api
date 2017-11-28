//======================================
//
//Name: TemplateUpdateResponse.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels.Admin
{
    public class TemplateUpdateResponse
    {
        //provides the message to the end user to show whether the the return
        //was successful or a failure and display a message. if successful 
        //return additional booking details

        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}