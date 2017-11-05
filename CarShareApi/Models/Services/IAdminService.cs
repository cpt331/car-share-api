﻿using System.Collections.Generic;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Models.Services
{
    //This interface provides the overarching activities related to
    //The admin service actions
    public interface IAdminService
    {
        TemplateUpdateResponse UpdateTemplate(TemplateUpdateRequest request);
        TemplateViewModel GetTemplate();
        List<TemplateField> GetTemplateMergeFields();
    }
}