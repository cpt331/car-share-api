using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;
using CarShareApi.ViewModels.Admin;

namespace CarShareApi.Models.Services.Implementations
{
    //this service holds all administrative functions associated with a user 
    //who would log in to administrate the Ewebah service

    public class AdminService : IAdminService
    {
        public AdminService(ITemplateRepository templateRepository)
        {
            //inherit the template repo from the DB
            TemplateRepository = templateRepository;
        }

        private ITemplateRepository TemplateRepository { get; }

        /// <summary>
        ///     Updates the email template used by the system for emails sent to new users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TemplateUpdateResponse UpdateTemplate(
            TemplateUpdateRequest request)
        {
            //this method looks for all templates and returns the first entry
            //it then updates the template fields based on the parsed object
            //to update in the database

            //look for first template and assign request variable to fields
            var template = TemplateRepository.FindAll().FirstOrDefault();
            if (template == null)
            {
                template = new Template
                {
                    //if no template exists create one
                    Subject = request.Subject,
                    Title = request.Title,
                    Body = request.Body,
                    Footer = request.Footer
                };
                TemplateRepository.Add(template);
            }
            else
            {
                //if template exists, override the existing one
                template.Subject = request.Subject;
                template.Title = request.Title;
                template.Body = request.Body;
                template.Footer = request.Footer;
                TemplateRepository.Update(template);
            }

            //return successful response
            return new TemplateUpdateResponse
            {
                Success = true,
                Message = "Template has been updated successfully."
            };
        }

        public TemplateViewModel GetTemplate()
        {
            //this method allows the template to be returned and output or used


            //look for first template and assign request variable to fields
            var template = TemplateRepository.FindAll().FirstOrDefault();
            if (template == null)
            {
                var viewModel = new TemplateViewModel
                {
                    //if no template exists show null values in fields
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
                    //if template exists, assign value of existing fields
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
            //merged fields have been allowed to simplify the use of the admin 
            //updating the records. the below list allows the translation of a
            //template field to hold the name and description of each variable

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