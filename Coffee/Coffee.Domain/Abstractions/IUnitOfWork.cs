using Coffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<Drink> DrinkRepository { get; }
        //IRepository<RitualUrn> RitualUrnRepository { get; }
        //IRepository<Hall> HallRepository { get; }
        public Task RemoveDatbaseAsync();
        public Task CreateDatabaseAsync();
        public Task SaveAllAsync();
    }
}
