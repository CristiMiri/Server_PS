using System.Net;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using Server.Domain.DTO;
using Server.Domain.Model;
using Newtonsoft.Json.Linq;
using Server.Repositories;

namespace Server.Services
{
    internal class ServerHost
    {
        private static readonly Lazy<ServerHost> instance = new Lazy<ServerHost>(() => new ServerHost());

        private readonly UserService _userService;
        private readonly PresentationService _presentationService;
        private readonly ParticipantService _participantService;
        private readonly Participant_PrezentareRepository _participant_PrezentareRepository;
        private readonly StatisticsService _statisticsService;

        // Private constructor to prevent instantiation from outside
        private ServerHost()
        {
            Console.WriteLine("Starting server");
            _userService = new UserService();
            _presentationService = new PresentationService();
            _participantService = new ParticipantService();
            _participant_PrezentareRepository = new Participant_PrezentareRepository(new ParticipantRepository(), new PresentationRepository());
            _statisticsService = new StatisticsService();
        }

        // Public static method to provide access to the singleton instance
        public static ServerHost Instance => instance.Value;

        public async Task Setup()
        {
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");
            int port = 8001;
            TcpListener listener = new TcpListener(ipAd, port);
            listener.Start();
            Console.WriteLine($"Server started on {ipAd}:{port}");
            Console.WriteLine("The local End point is  :" + listener.LocalEndpoint);

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1000000];
            int byteCount;

            while ((byteCount = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                Console.WriteLine($"Received: {request}");                                


                PackageDTO response = await ProcessRequestAsync(request);

                // Serialize the response
                string responseJson = JsonConvert.SerializeObject(response);
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);
                // Send the length of the response first
                byte[] lengthBytes = BitConverter.GetBytes(responseBytes.Length);
                await stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
                // Send the actual response
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                //Console.WriteLine("Sent: " + responseJson);
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }

        private async Task<PackageDTO> ProcessRequestAsync(string request)
        {
            try
            {
                var jsonRequest = JsonConvert.DeserializeObject<Dictionary<string, object>>(request);
                string service = jsonRequest["service"].ToString();
                string method = jsonRequest["method"].ToString();
                var parameters = jsonRequest["parameters"];

                switch (service)
                {
                    case "UserService":
                        return await HandleUserServiceRequestAsync(method, parameters);
                    case "PresentationService":
                        return await HandlePresentationServiceRequestAsync(method, parameters);
                    case "ParticipantService":
                        return await HandleParticipantServiceRequestAsync(method, parameters);
                    case "StatisticsService":
                        return await HandleStatisticsServiceRequestAsync(method, parameters);
                    default:
                        return new PackageDTO { Result = false, Message = "Service not found" };
                }
            }
            catch (Exception ex)
            {
                return new PackageDTO { Result = false, Message = $"Error processing request: {ex.Message}" };
            }
        }

        private async Task<PackageDTO> HandleUserServiceRequestAsync(string method, object parameters)
        {
            try
            {
                switch (method)
                {
                    case "CreateUser":
                        var userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                        bool created = _userService.CreateUser(userDTO);
                        return new PackageDTO { Result = created, Message = created ? "User created successfully" : "Failed to create user", Data = new UserDTO() };

                    case "DeleteUser":
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                        bool deleted = _userService.DeleteUser(userDTO);
                        return new PackageDTO { Result = deleted, Message = deleted ? "User deleted successfully" : "Failed to delete user", Data = new { } };

                    case "GetAllUsers":
                        var users = _userService.GetAllUsers();
                        string allUsersMessage = users != null ? "Users retrieved successfully" : "Failed to retrieve users";
                        return new PackageDTO { Result = users != null, Message = allUsersMessage, Data = users };

                    case "GetUserbyId":
                        int id = int.Parse(parameters.ToString());
                        var user = _userService.GetUserbyId(id);
                        string userByIdMessage = user != null ? "User retrieved successfully" : "Failed to retrieve user";
                        return new PackageDTO { Result = user != null, Message = userByIdMessage, Data = user };

                    case "GetUsersByEmailAndPassword":
                        var creds = JsonConvert.DeserializeObject<Dictionary<string, string>>(parameters.ToString());
                        user = _userService.GetUsersByEmailAndPassword(creds["email"], creds["password"]);
                        string usersByEmailAndPasswordMessage = user != null ? "User retrieved successfully" : "Failed to retrieve user";
                        return new PackageDTO { Result = user != null, Message = usersByEmailAndPasswordMessage, Data = user };

                    case "GetUsersByUserType":
                        JObject jsonObject = JObject.Parse(parameters.ToString());
                        // Access the value of the "userType" property
                        int userTypeValue = (int)jsonObject["userType"];
                        UserType userType = (UserType)Enum.ToObject(typeof(UserType), userTypeValue); // Convert int to enum
                        users = _userService.GetUsersByUserType(userType);
                        string usersByUserTypeMessage = users != null ? "Users retrieved successfully" : "Failed to retrieve users";
                        return new PackageDTO { Result = users != null, Message = usersByUserTypeMessage, Data = users };

                    case "UpdateUser":
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                        bool updated = _userService.UpdateUser(userDTO);
                        return new PackageDTO { Result = updated, Message = updated ? "User updated successfully" : "Failed to update user", Data = new { } };
                    case "NotifyWhatsAppUpdate":
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                        bool updateResult = _userService.NotifyWhatsAppUpdate(userDTO);
                        return new PackageDTO { Result = true, Message = updateResult ? "User updated successfully" : "Failed to update user", Data = new { } };
                    case "NotifyEmailUpdate":
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                        bool updateResult2 = _userService.NotifyEmailUpdate(userDTO);
                        return new PackageDTO { Result = true, Message = updateResult2 ? "User updated successfully" : "Failed to update user", Data = new { } };
                    default:
                        return new PackageDTO { Result = false, Message = "Method not found" , Data = new { } };
                }
            }
            catch (Exception ex)
            {
                return new PackageDTO { Result = false, Message = $"Error processing request: {ex.Message}" };
            }
        }
        private async Task<PackageDTO> HandlePresentationServiceRequestAsync(string method, object parameters)
        {
            try
            {
                switch (method)
                {
                    case "CreatePresentation":
                        var presentationDTO = JsonConvert.DeserializeObject<PresentationDTO>(parameters.ToString());
                        bool created = _presentationService.CreatePresentation(presentationDTO);
                        return new PackageDTO { Result = created, Message = created ? "Presentation created successfully" : "Failed to create presentation" , Data = new { } };
                    case "UpdatePresentation":
                        presentationDTO = JsonConvert.DeserializeObject<PresentationDTO>(parameters.ToString());
                        bool updated = _presentationService.UpdatePresentation(presentationDTO);
                        return new PackageDTO { Result = updated, Message = updated ? "Presentation updated successfully" : "Failed to update presentation" , Data = new { } };
                    case "DeletePresentation":
                        PresentationDTO presentation = JsonConvert.DeserializeObject<PresentationDTO>(parameters.ToString());
                        bool deleted = _presentationService.DeletePresentation(presentation.Id);
                        return new PackageDTO { Result = deleted, Message = deleted ? "Presentation deleted successfully" : "Failed to delete presentation" ,Data = new { } };
                    case "GetPresentation":
                        int id = int.Parse(parameters.ToString());
                        presentation = _presentationService.GetPresentation(id);
                        string presentationMessage = presentation != null ? "Presentation retrieved successfully" : "Failed to retrieve presentation";
                        return new PackageDTO { Result = presentation != null, Message = presentationMessage, Data = presentation };
                    case "GetAllPresentation":
                        var presentations = _presentationService.GetAllPresentation();
                        foreach (PresentationDTO p in presentations)
                        {
                            ParticipantDTO author = _participantService.GetParticipant(p.IdAuthor);
                            p.Author.Add(author);
                            p.Participants = _participant_PrezentareRepository.ReadParticipantsByPresentation(p);
                        }
                        string allPresentationMessage = presentations != null ? "Presentations retrieved successfully" : "Failed to retrieve presentations";
                        return new PackageDTO { Result = presentations != null, Message = allPresentationMessage, Data = presentations };
                    case "GetPresentationsbySection":
                        /*JObject jsonObject = JObject.Parse(parameters.ToString());
                        // Access the value of the "userType" property
                        int userTypeValue = (int)jsonObject["userType"];
                        UserType userType = (UserType)Enum.ToObject(typeof(UserType), userTypeValue); // Convert int to enum*/
                        JObject jsonObject = JObject.Parse(parameters.ToString());
                        int sectionValue = (int)jsonObject["section"];
                        Section section = (Section)Enum.ToObject(typeof(Section), sectionValue); // Convert int to enum                        
                        presentations = _presentationService.GetPresentationsbySection(section);
                        foreach (PresentationDTO p in presentations)
                        {
                            ParticipantDTO author = _participantService.GetParticipant(p.IdAuthor);
                            p.Author.Add(author);
                            p.Participants = _participant_PrezentareRepository.ReadParticipantsByPresentation(p);
                        }
                        string presentationsBySectionMessage = presentations != null ? "Presentations retrieved successfully" : "Failed to retrieve presentations";
                        return new PackageDTO { Result = presentations != null, Message = presentationsBySectionMessage, Data = presentations };

                    default:
                        return new PackageDTO { Result = false, Message = "Method not found" };
                }
            }
            catch (Exception ex)
            {
                return new PackageDTO { Result = false, Message = $"Error processing request: {ex.Message}" };
            }
        }
        private async Task<PackageDTO> HandleParticipantServiceRequestAsync(string method, object parameters)
        {
            try
            {
                switch (method)
                {
                    case "CreateParticipant":
                        var participantDTO = JsonConvert.DeserializeObject<ParticipantDTO>(parameters.ToString());
                        bool created = _participantService.CreateParticipant(participantDTO);
                        return new PackageDTO { Result = created, Message = created ? "Participant created successfully" : "Failed to create participant" ,Data = new { } };
                    case "UpdateParticipant":
                        participantDTO = JsonConvert.DeserializeObject<ParticipantDTO>(parameters.ToString());
                        bool updated = _participantService.UpdateParticipant(participantDTO);
                        return new PackageDTO { Result = updated, Message = updated ? "Participant updated successfully" : "Failed to update participant", Data = new { } };
                    case "DeleteParticipant":
                        participantDTO = JsonConvert.DeserializeObject<ParticipantDTO>(parameters.ToString());
                        int id = participantDTO.Id;
                        bool deleted = _participantService.DeleteParticipant(id);
                        return new PackageDTO { Result = deleted, Message = deleted ? "Participant deleted successfully" : "Failed to delete participant" , Data = new { } };
                    case "UpsertParticipant":
                        participantDTO = JsonConvert.DeserializeObject<ParticipantDTO>(parameters.ToString());
                        bool upserted = _participantService.UpsertParticipant(participantDTO);
                        return new PackageDTO { Result = upserted, Message = upserted ? "Participant upserted successfully" : "Failed to upsert participant" , Data = new { } };
                    case "GetParticipant":
                        id = int.Parse(parameters.ToString());
                        ParticipantDTO participant = _participantService.GetParticipant(id);
                        string participantMessage = participant != null ? "Participant retrieved successfully" : "Failed to retrieve participant";
                        return new PackageDTO { Result = participant != null, Message = participantMessage, Data = participant };
                    case "GetAll":
                        var participants = _participantService.GetAll();

                        string allParticipantsMessage = participants != null ? "Participants retrieved successfully" : "Failed to retrieve participants";
                        return new PackageDTO { Result = participants != null, Message = allParticipantsMessage, Data = participants };
                    case "GetParticipantsbySection":
                        var section = (Section)Enum.Parse(typeof(Section), parameters.ToString());
                        participants = _participantService.GetParticipantsbySection(section);
                        string participantsBySectionMessage = participants != null ? "Participants retrieved successfully" : "Failed to retrieve participants";
                        return new PackageDTO { Result = participants != null, Message = participantsBySectionMessage, Data = participants };
                    case "GetLastParticipantId":
                        int lastId = _participantService.GetLastParticipantId();
                        return new PackageDTO { Result = true, Message = "Last participant id retrieved successfully", Data = lastId };
                    case "AcceptParticipant":
                        participant = JsonConvert.DeserializeObject<ParticipantDTO>(parameters.ToString());
                        _participantService.AcceptParticipant(participant.Email);
                        return new PackageDTO { Result = true, Message = "Participant accepted successfully" , Data = new { } };
                    case "RejectParticipant":
                        participant = JsonConvert.DeserializeObject<ParticipantDTO>(parameters.ToString());
                        _participantService.RejectParticipant(participant.Email);
                        return new PackageDTO { Result = true, Message = "Participant rejected successfully", Data = new { } };
                    case "GetParticipantsPhotos":
                        var photos = _participantService.GetParticipantsPhotos();
                        string photosMessage = photos != null ? "Photos retrieved successfully" : "Failed to retrieve photos";
                        return new PackageDTO { Result = photos != null, Message = photosMessage, Data = photos };
                    case "GetParticipantCV":
                        string path = parameters.ToString();
                        var cv = _participantService.GetParticipantCV(path);
                        string cvMessage = cv != null ? "CV retrieved successfully" : "Failed to retrieve CV";
                        return new PackageDTO { Result = cv != null, Message = cvMessage, Data = cv };
                    case "SaveParticipantPhoto":
                        var photo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(parameters.ToString());
                        bool saved = _participantService.SaveParticipantPhoto(photo);
                        return new PackageDTO { Result = saved, Message = saved ? "Photo saved successfully" : "Failed to save photo", Data = new { } };
                    case "SaveParticipantCV":
                        var cvData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(parameters.ToString());
                        bool savedCV = _participantService.SaveParticipantCV(cvData);
                        return new PackageDTO { Result = savedCV, Message = savedCV ? "CV saved successfully" : "Failed to save CV", Data = new { } };
                    case "CreateParticipantPresentation":
                        //parameters ={"relation": {"Item1": 12,"Item2": 1}}
                        var participantPresentation = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(parameters.ToString());
                        bool createdParticipantPresentation = _participantService.CreateParticipantPresentation(participantPresentation["relation"].Item1, participantPresentation["relation"].Item2);                        
                        return new PackageDTO { Result = createdParticipantPresentation, Message = createdParticipantPresentation ? "Participant presentation created successfully" : "Failed to create participant presentation" ,Data=new { } };

                    default:
                        return new PackageDTO { Result = false, Message = "Method not found" };
                }
            }
            catch (Exception ex)
            {
                return new PackageDTO { Result = false, Message = $"Error processing request: {ex.Message}" };
            }
        }
        private async Task<PackageDTO> HandleStatisticsServiceRequestAsync(string method, object parameters)
        {
            try
            {
                switch (method)
                {
                    case "GetNumberOfParticipantsByConference":
                        var participantsByConference = _statisticsService.GetNumberOfParticipantsByConference();
                        string participantsByConferenceMessage = participantsByConference != null ? "Participants retrieved successfully" : "Failed to retrieve participants";
                        return new PackageDTO { Result = participantsByConference != null, Message = participantsByConferenceMessage, Data = participantsByConference };
                    case "GetNumberOfParticipantsBySection":
                        var participantsBySection = _statisticsService.GetNumberOfParticipantsBySection();
                        string participantsBySectionMessage = participantsBySection != null ? "Participants retrieved successfully" : "Failed to retrieve participants";
                        return new PackageDTO { Result = participantsBySection != null, Message = participantsBySectionMessage, Data = participantsBySection };
                    case "GetNumberOfPresentationsByAuthor":
                        var presentationsByAuthor = _statisticsService.GetNumberOfPresentationsByAuthor();
                        string presentationsByAuthorMessage = presentationsByAuthor != null ? "Presentations retrieved successfully" : "Failed to retrieve presentations";
                        return new PackageDTO { Result = presentationsByAuthor != null, Message = presentationsByAuthorMessage, Data = presentationsByAuthor };
                    case "GetNumberOfPresentationsPerDay":
                        var presentationsPerDay = _statisticsService.GetNumberOfPresentationsPerDay();
                        string presentationsPerDayMessage = presentationsPerDay != null ? "Presentations retrieved successfully" : "Failed to retrieve presentations";
                        return new PackageDTO { Result = presentationsPerDay != null, Message = presentationsPerDayMessage, Data = presentationsPerDay };

                    default:
                        return new PackageDTO { Result = false, Message = "Method not found" };
                }
            }
            catch (Exception ex)
            {
                return new PackageDTO { Result = false, Message = $"Error processing request: {ex.Message}" };
            }
        }
    }
}
