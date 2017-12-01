//======================================
//
//Name: FakeTemplateRepository.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Linq;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    //implemented template repository to allow for appropriate methods
    //to enable testing. Templates are the message body sent in the emails to
    //new users of the service
    public class FakeTemplateRepository : ITemplateRepository
    {
        public List<Template> Templates { get; set; }

        public FakeTemplateRepository(List<Template> templates)
        {
            //implements the Templates table data
            Templates = templates;
        }

        public Template Add(Template item)
        {
            //add item to the Templates repo
            Templates.Add(item);
            return item;
        }

        public Template Find(int id)
        {
            //find Templates base on template ID
            return Templates.FirstOrDefault(x => x.ID == id);
        }

        public List<Template> FindAll()
        {
            //return all Templates to list
            return Templates;
        }

        public IQueryable<Template> Query()
        {
            //return querable Templates list
            return Templates.AsQueryable();
        }

        public Template Update(Template item)
        {
            //update the Template with new entry
            Templates.RemoveAll(x => x.ID == item.ID);
            Templates.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            //remove the Templates from the table
            Templates.RemoveAll(x => x.ID == id);
        }
    }
}
