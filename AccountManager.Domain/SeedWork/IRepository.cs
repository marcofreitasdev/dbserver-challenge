using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManager.Domain.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        void Add(T aggregate);
        void Update(T aggregate);
        void Delete(T aggregate);
        Task<T> FindByIdAsync(Guid aggregateId);
        Task<IEnumerable<T>> GetAll();
    }
}
