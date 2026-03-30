using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateAccsesToken(int userId, string email, string role);
        string CreateRefreshToken();
    }
}
