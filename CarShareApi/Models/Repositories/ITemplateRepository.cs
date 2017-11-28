//======================================
//
//Name: ITemplateRepository.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Repositories
{
    public interface ITemplateRepository : IRepository<Template, int>
    {
    }
}