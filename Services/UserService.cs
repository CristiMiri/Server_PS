using Server.Domain.DTO;
using Server.Domain.Model;
using Server.Repositories;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class UserService : IUserService
    {
        private UserRepository _userRepository;
        public UserService() {
            _userRepository = new UserRepository();
        }

        public bool CreateUser(UserDTO userDTO)
        {
            return _userRepository.CreateUser(userDTO);
        }

        public bool DeleteUser(UserDTO userDTO)
        {
            return _userRepository.DeleteUser(userDTO.Id);
        }

        public List<UserDTO> GetAllUsers()
        {
            return _userRepository.ReadUsers();
        }

        public UserDTO GetUserbyId(int id)
        {
            return _userRepository.ReadUserById(id);
        }

        public UserDTO GetUsersByEmailAndPassword(string email, string password)
        {
            return _userRepository.ReadUserByEmailAndPassword(email,password);   
        }

        public List<UserDTO> GetUsersByUserType(UserType userType)
        {
            return _userRepository.ReadUsersByUserType(userType);
        }

        public bool UpdateUser(UserDTO userDTO)
        {
            return _userRepository.UpdateUser(userDTO);
        }
    }
}
