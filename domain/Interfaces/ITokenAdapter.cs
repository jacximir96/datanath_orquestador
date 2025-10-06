using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Interfaces
{
    public interface ITokenAdapter
    {
        string GenerateToken(string username, int expireMinutes = 60);
    }
}
