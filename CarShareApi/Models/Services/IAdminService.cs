using System.Collections.Generic;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Models.Services
{
    public interface IAdminService
    {
        TemplateUpdateResponse UpdateTemplate(TemplateUpdateRequest request);
        TemplateViewModel GetTemplate();
        List<TemplateField> GetTemplateMergeFields();
    }
}