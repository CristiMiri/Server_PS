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
        private EmailSender _emailSender;
        public UserService() {
            _userRepository = new UserRepository();
            _emailSender = new EmailSender();
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

        internal bool NotifyEmailUpdate(UserDTO? userDTO)
        {
            string subject = "Actualizare Date Personale";
            string body = "Datele Voastre Personale au fost actualizate\n";
            body += "email: " + userDTO.Email + "\n";
            body += "name :" + userDTO.Name + "\n";
            body += "password: " + userDTO.Password + "\n";
            body += "phone" + userDTO.Phone + "\n";
            try
            {
                _emailSender.SendMail(userDTO.Email, subject, body);
            }
            catch (Exception ex)
            {
                return false;
            }
            Console.WriteLine("Email sent to " + userDTO.Email);
            return true;
        }

        internal bool NotifyWhatsAppUpdate(UserDTO? userDTO)
        {
            string accountSid = "YOUR_ACCOUNT_SID";
            string authToken = "YOUR_AUTH_TOKEN";
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
            body: "This is a test message from Twilio",
            from: new PhoneNumber("whatsapp:YOUR_TWILIO_WHATSAPP_NUMBER"),
            to: new PhoneNumber("whatsapp:RECIPIENT_WHATSAPP_NUMBER")
            );

            Console.WriteLine($"WhatsApp message sent: {message.Sid}");
        }
    }
}
