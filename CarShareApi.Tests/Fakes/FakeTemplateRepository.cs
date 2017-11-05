using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeTemplateRepository : ITemplateRepository
    {
        public List<Template> Templates { get; set; }

        public FakeTemplateRepository(List<Template> templates)
        {
            Templates = templates;
        }

        public Template Add(Template item)
        {
            Templates.Add(item);
            return item;
        }

        public Template Find(int id)
        {
            return Templates.FirstOrDefault(x => x.ID == id);
        }

        public List<Template> FindAll()
        {
            return Templates;
        }

        public IQueryable<Template> Query()
        {
            return Templates.AsQueryable();
        }

        public Template Update(Template item)
        {
            Templates.RemoveAll(x => x.ID == item.ID);
            Templates.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            Templates.RemoveAll(x => x.ID == id);
        }
    }
}
