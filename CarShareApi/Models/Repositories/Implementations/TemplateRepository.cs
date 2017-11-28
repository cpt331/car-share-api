//======================================
//
//Name: TemplateyRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class TemplateRepository : ITemplateRepository
    {
        //This class inherits the ItemplateRepository register
        public TemplateRepository(CarShareContext context)
        {
            //Inherits the DB context
            Context = context;
        }

        private CarShareContext Context { get; }


        public Template Add(Template item)
        {
            //Function to add a new template item to the DB and save changes
            var template = Context.Templates.Add(item);
            Context.SaveChanges();
            return template;
        }

        public Template Find(int id)
        {
            //Finds a template based on the template ID and returns first input
            var template = Context.Templates.FirstOrDefault(x => x.ID == id);
            return template;
        }

        public List<Template> FindAll()
        {
            //Finds all templates and returns a list
            return Context.Templates.ToList();
        }

        public IQueryable<Template> Query()
        {
            return Context.Templates.AsQueryable();
        }

        public Template Update(Template item)
        {
            //The template state that is currently modified is saved to the DB
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            //Allows the user to delete a template based on ID before saving
            var template = Context.Templates.FirstOrDefault(x => x.ID == id);
            if (template != null)
            {
                Context.Entry(template).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }
    }
}