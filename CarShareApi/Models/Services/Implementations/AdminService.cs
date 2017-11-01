using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Models.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private ITemplateRepository TemplateRepository { get; set; }

        public AdminService(ITemplateRepository templateRepository)
        {
            TemplateRepository = templateRepository;
        }

        /// <summary>
        /// Updates the email template used by the system for emails sent to new users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TemplateUpdateResponse UpdateTemplate(TemplateUpdateRequest request)
        {
            var template = TemplateRepository.FindAll().FirstOrDefault();
            if (template == null)
            {
                template = new Template
                {
                    Subject = request.Subject,
                    Title = request.Title,
                    Body = request.Body,
                    Footer = request.Footer
                };
                TemplateRepository.Add(template);
            }
            else
            {
                template.Subject = request.Subject;
                template.Title = request.Title;
                template.Body = request.Body;
                template.Footer = request.Footer;
                TemplateRepository.Update(template);
            }

            return new TemplateUpdateResponse
            {
                Success = true,
                Message = "Template has been updated successfully."
            };
        }
    }
}