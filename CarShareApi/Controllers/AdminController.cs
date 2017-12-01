//======================================
//
//Name: AdminController.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarShareApi.Models.Services;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Controllernlogs
{
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiController
    {
        //private static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAdminService AdminService;

        //inject service to make testing easier
        public AdminController(IAdminService adminService)
        {
            AdminService = adminService;
        }

        [HttpPost]
        [Route("api/admin/updatetemplate")]
        public TemplateUpdateResponse UpdateTemplate(TemplateUpdateRequest request)
        {
            TemplateUpdateResponse response;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => 
                ModelState[key].Errors.Select(x => x.ErrorMessage));
                response = new TemplateUpdateResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };
            }
            else
            {
                //send request to the admin service and return the 
                //response (success or fail)
                response = AdminService.UpdateTemplate(request);
            }
            return response;
        }

        [HttpGet]
        [Route("api/admin/gettemplate")]
        public TemplateViewModel GetTemplate()
        {
            return AdminService.GetTemplate();
        }

        [HttpGet]
        [Route("api/admin/gettemplatefields")]
        public List<TemplateField> GetTemplateFields()
        {
            return AdminService.GetTemplateMergeFields();
        }
    }
}