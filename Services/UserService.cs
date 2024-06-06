using Server.Domain.DTO;
using Server.Domain.Model;
using Server.Repositories;
using Server.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Server.Services
{
    internal class UserService : IUserService
    {
        private UserRepository _userRepository;
        private EmailSender _emailSender;
        public UserService()
        {
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
            return _userRepository.ReadUserByEmailAndPassword(email, password);
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

        internal async Task<bool> NotifyWhatsAppUpdate(UserDTO? userDTO)
        {
            string accountSid = "AC32e0e72e94485338299a13964d5a62be";
            string authToken = "f5a9c4a00556b19f3b1703871a98897d";
            string body = "Datele Voastre Personale au fost actualizate\n";
            body += "email: " + userDTO.Email + "\n";
            body += "name :" + userDTO.Name + "\n";
            body += "password: " + userDTO.Password + "\n";
            body += "phone: " + userDTO.Phone + "\n";
            string from =   "whatsapp:+14155238886";
            string to =     "whatsapp:+40" + userDTO.Phone;
;



            TwilioClient.Init(accountSid, authToken);
            var message = await MessageResource.CreateAsync(
            body:   body,
            from:   new PhoneNumber(from),
            to:     new PhoneNumber(to)
            );
            if (message.Status == MessageResource.StatusEnum.Sent)
            {
                Console.WriteLine("Message sent successfully!");
                return true;
            }
            else
            {
                Console.WriteLine($"Message failed with error code: {message.ErrorCode}");
                return false;
            }            
        }

    }

}
