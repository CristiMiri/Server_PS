using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.DTO
{
    internal class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string Phone { get; set; }

        // Default constructor
        public UserDTO() { }

        // Constructor to initialize the DTO from a User entity
        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
            UserType = user.UserType;
            Phone = user.Phone;
        }

        public override string ToString()
        {
            return $"UserDTO with id {Id} and name {Name} has email {Email} and is of type {UserType} and has phone number {Phone}.";
        }

        // Converts a list of User entities to a list of UserDTOs
        public static List<UserDTO> FromUserList(List<User> users)
        {
            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                userDTOs.Add(new UserDTO(user));
            }
            return userDTOs;
        }
    }
}
