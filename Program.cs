// See https://aka.ms/new-console-template for more information
using Server.Domain.DTO;
using Server.Repositories;
using Server.Services;

namespace Server;
class Program
{
    static async Task Main(string[] args)
    {       
        ServerHost serviceHost = new ServerHost();
        UserRepository userRepository = new UserRepository();
        if (userRepository.ReadUsers().Count() > 0)
        {
            Console.WriteLine("Connected to DB succesfull.");
            await serviceHost.Setup();
        }                
    }
}