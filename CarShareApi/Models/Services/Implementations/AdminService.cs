using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Models.Services.Implementations
{
    public class AdminService : IAdminService
    {
        public AdminService(ITemplateRepository templateRepository)
        {
            TemplateRepository = templateRepository;
        }

        private ITemplateRepository TemplateRepository { get; }

        /// <summary>
        ///     Updates the email template used by the system for emails sent to new users
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

        public TemplateViewModel GetTemplate()
        {
            var template = TemplateRepository.FindAll().FirstOrDefault();
            if (template == null)
            {
                var viewModel = new TemplateViewModel
                {
                    Subject = string.Empty,
                    Title = string.Empty,
                    Body = string.Empty,
                    Footer = string.Empty
                };
                return viewModel;
            }
            else
            {
                var viewModel = new TemplateViewModel
                {
                    Subject = template.Subject,
                    Title = template.Title,
                    Body = template.Body,
                    Footer = template.Footer
                };
                return viewModel;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<TemplateField> GetTemplateMergeFields()
        {
            return new List<TemplateField>
            {
                new TemplateField
                {
                    Name = Constants.TemplateNameField,
                    Description = Constants.TemplateNameFieldDescription
                },
                new TemplateField
                {
                    Name = Constants.TemplateEmailField,
                    Description = Constants.TemplateEmailFieldDescription
                },
                new TemplateField
                {
                    Name = Constants.TemplateOTPField,
                    Description = Constants.TemplateOTPFieldDescription
                }
            };
        }
    }
}