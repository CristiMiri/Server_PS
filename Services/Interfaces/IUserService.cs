using Server.Domain.DTO;
using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    internal interface IUserService
    {
        bool CreateUser(UserDTO userDTO);
        bool UpdateUser(UserDTO userDTO);
        bool DeleteUser(UserDTO userDTO);

        UserDTO GetUserbyId(int id);
        List<UserDTO> GetAllUsers();
        UserDTO GetUsersByEmailAndPassword(string email, string password);
        List<UserDTO> GetUsersByUserType(UserType userType);
    }
}
