using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Interfaces
{
    public interface IUserRepository<T> where T:class
    {
        Task<T> GetUser(string name, string password);
    }
}
