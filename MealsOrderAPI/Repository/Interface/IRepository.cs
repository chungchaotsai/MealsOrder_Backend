using MealsOrderAPI.Models;
using Microsoft.AspNetCore.OData.Results;

namespace MealsOrderAPI.Repository.Interface
{
    // https://stackoverflow.com/questions/30112403/why-do-examples-of-the-repository-pattern-never-deal-with-database-connection-ex
    public interface IRepository<T>
    {
        public Task Add(T entity);
        public Task Delete(T entity);
        public Task Update(T entity);
        public IQueryable<T> List();
        public SingleResult<User> Get(int id);

    }
}
