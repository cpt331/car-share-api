using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories.Implementations
{
    public class TemplateRepository : ITemplateRepository
    {
        public TemplateRepository(CarShareContext context)
        {
            Context = context;
        }

        private CarShareContext Context { get; }


        public Template Add(Template item)
        {
            var template = Context.Templates.Add(item);
            Context.SaveChanges();
            return template;
        }

        public Template Find(int id)
        {
            var template = Context.Templates.FirstOrDefault(x => x.ID == id);
            return template;
        }

        public List<Template> FindAll()
        {
            return Context.Templates.ToList();
        }

        public IQueryable<Template> Query()
        {
            return Context.Templates.AsQueryable();
        }

        public Template Update(Template item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            var template = Context.Templates.FirstOrDefault(x => x.ID == id);
            if (template != null)
            {
                Context.Entry(template).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }
    }
}