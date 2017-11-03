using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Models.Services
{
    public interface IAdminService
    {
        TemplateUpdateResponse UpdateTemplate(TemplateUpdateRequest request);
        TemplateViewModel GetTemplate();
        Dictionary<string, string> GetTemplateMergeFields();
    }
}
