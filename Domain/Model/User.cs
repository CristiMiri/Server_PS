using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Server.Domain.Model
{
    public enum UserType
    {
        PARTICIPANT,
        ORGANIZER,
        ADMINISTRATOR
    }
    public class User
    {
        public  int     Id { get; set; }
        public  string  Name { get; set; }
        public  string  Email { get; set; }
        public  string  Password { get; set; }
        public  UserType UserType { get; set; }
        public  string  Phone { get; set; }


        public User(int id, string name, string email, string password, UserType userType, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = HashPassword(password);
            UserType = userType;
            Phone = phone;
        }

        public User()
        {
            Id = 0;
            Name = "";
            Email = "";
            Password = "";
            UserType = UserType.PARTICIPANT;
            Phone = "";
        }

        public User(User user)
            : this(user.Id, user.Name, user.Email, user.Password, user.UserType, user.Phone)
        {
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        // Password hashing function to ensure password security.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public override string ToString()
        {
            return $"User with id {Id} and name {Name} has email {Email} and is of type {UserType} and has phone number {Phone}.";
        }
        
        public static List<User> dummyUserData()
        {
            List<User> users = new List<User>();
            users.Add(new User(1, "John Doe", "john@example.com", "password123", UserType.PARTICIPANT, "123456789"));
            users.Add(new User(2, "Jane Smith", "jane@example.com", "password456", UserType.ORGANIZER, "987654321"));
            users.Add(new User(3, "Admin User", "admin@example.com", "adminpassword", UserType.ADMINISTRATOR, "555555555"));
            return users;
        }
    }

}