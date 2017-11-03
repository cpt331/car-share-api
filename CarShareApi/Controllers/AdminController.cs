using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels.Admin;
using NLog;

namespace CarShareApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiController
    {
        private IAdminService AdminService;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        //inject service to make testing easier
        public AdminController(IAdminService adminService)
        {
            AdminService = adminService;
        }

        [HttpPost, Route("api/admin/updatetemplate")]
        public TemplateUpdateResponse UpdateTemplate(TemplateUpdateRequest request)
        {
            TemplateUpdateResponse response;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(x => x.ErrorMessage));
                response = new TemplateUpdateResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };

            }
            else
            {
                //send request to the admin service and return the response (success or fail)
                response = AdminService.UpdateTemplate(request);
            }
            return response;
        }

        [HttpGet, Route("api/admin/gettemplate")]
        public TemplateViewModel GetTemplate()
        {
            return AdminService.GetTemplate();
        }

        [HttpGet, Route("api/admin/gettemplatefields")]
        public Dictionary<string,string> GetTemplateFields()
        {
            return AdminService.GetTemplateMergeFields();
        }

    }
}
