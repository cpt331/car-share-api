using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels.Admin
{
    public class TemplateUpdateRequest
    {
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
    }
}