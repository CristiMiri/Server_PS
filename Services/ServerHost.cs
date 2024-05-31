using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net.Sockets;
using System.Collections;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Server.Domain.DTO;
using Server.Domain.Model;
using System.ServiceModel.Channels;

namespace Server.Services
{
    internal class ServerHost
    {

        private readonly UserService _userService;
        private readonly PresentationService _presentationService;
        public ServerHost()
        {
            Console.WriteLine("Starting server");
            _userService = new UserService();
            _presentationService = new PresentationService();
        }

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
            byte[] buffer = new byte[1024];
            int byteCount;

            while ((byteCount = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                Console.WriteLine($"Received: {request}");

                (bool success, string responseMessage) = await ProcessRequestAsync(request);
                var response = new Dictionary<string, object>
                {
                    { "Result", success },
                    { "Message", responseMessage }
                };

                // Serialize the response
                string responseJson = JsonConvert.SerializeObject(response);
                //string responseJson = JsonConvert.SerializeObject(response);
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("Sent: " + responseJson);
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }

        private async Task<(bool Result, string Message)> ProcessRequestAsync(string request)
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
                    // Add cases for other services here
                    default:
                        return (false, "Service not found");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error processing request: {ex.Message}");
            }
        }
        private async Task<(bool Result, string Message)> HandleUserServiceRequestAsync(string method, object parameters)
        {
            switch (method)
            {
                case "CreateUser":
                    var userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                    bool created = _userService.CreateUser(userDTO);
                    return (created, created ? "User created successfully" : "Failed to create user");

                case "DeleteUser":
                    userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                    bool deleted = _userService.DeleteUser(userDTO);
                    return (deleted, deleted ? "User deleted successfully" : "Failed to delete user");

                case "GetAllUsers":
                    var users = _userService.GetAllUsers();
                    string allUsersMessage = users != null ? "Users retrieved successfully" : "Failed to retrieve users";
                    return (users != null, JsonConvert.SerializeObject(users));

                case "GetUserbyId":
                    int id = int.Parse(parameters.ToString());
                    var user = _userService.GetUserbyId(id);
                    string userByIdMessage = user != null ? "User retrieved successfully" : "Failed to retrieve user";
                    return (user != null, JsonConvert.SerializeObject(user));

                case "GetUsersByEmailAndPassword":
                    var creds = JsonConvert.DeserializeObject<Dictionary<string, string>>(parameters.ToString());
                    user = _userService.GetUsersByEmailAndPassword(creds["email"], creds["password"]);
                    string usersByEmailAndPasswordMessage = user != null ? "User retrieved successfully" : "Failed to retrieve user";
                    return (user != null, JsonConvert.SerializeObject(user));

                case "GetUsersByUserType":
                    var userType = (UserType)Enum.Parse(typeof(UserType), parameters.ToString());
                    users = _userService.GetUsersByUserType(userType);
                    string usersByUserTypeMessage = users != null ? "Users retrieved successfully" : "Failed to retrieve users";
                    return (users != null, JsonConvert.SerializeObject(users));

                case "UpdateUser":
                    userDTO = JsonConvert.DeserializeObject<UserDTO>(parameters.ToString());
                    bool updated = _userService.UpdateUser(userDTO);
                    return (updated, updated ? "User updated successfully" : "Failed to update user");

                default:
                    return (false, "Method not found");
            }
        }


        //private async Task<(bool Result, string Message)> HandlePresentationRequestAsync(string method, object parameters)
        //{
        //    switch (method)
        //    {
        //        case "CreatePresentation":
        //            PresentationDTO presentation = JsonConvert.DeserializeObject<PresentationDTO>(parameters.ToString());
        //            bool created= _presentationService.CreatePresentation(presentation);

        //        case "UpdatePresentation":
        //        case "DeletePresentation":
        //        case "GetPresentation":
        //        case "GetAllPresentation":
        //        case "GetPresentationsbySection":
        //    }
        //}

        //static async Task Setup(string[] args)
        //{
        //    IPAddress ipAd = IPAddress.Parse("127.0.0.1");
        //    List<IPEndPoint> endpoints = new List<IPEndPoint>
        //    {
        //        new IPEndPoint(ipAd, 5000),
        //        new IPEndPoint(ipAd, 5001)
        //    };

        //    List<Task> listenerTasks = new List<Task>();

        //    foreach (var endpoint in endpoints)
        //    {
        //        listenerTasks.Add(StartListenerAsync(endpoint));
        //    }

        //    await Task.WhenAll(listenerTasks);
        //}

        //private static async Task StartListenerAsync(IPEndPoint endpoint)
        //{
        //    TcpListener listener = new TcpListener(endpoint);
        //    listener.Start();
        //    Console.WriteLine($"Server started on {endpoint}");

        //    while (true)
        //    {
        //        TcpClient client = await listener.AcceptTcpClientAsync();
        //        Console.WriteLine($"Client connected on {endpoint}");
        //        _ = HandleClientAsync(client);
        //    }
        //}

        //private static async Task HandleClientAsync(TcpClient client)
        //{
        //    using NetworkStream stream = client.GetStream();
        //    byte[] buffer = new byte[1024];
        //    int byteCount;

        //    while ((byteCount = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        //    {
        //        string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
        //        Console.WriteLine($"Received: {request}");

        //        string response = "Message received";
        //        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
        //        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        //        Console.WriteLine("Sent: Message received");
        //    }

        //    client.Close();
        //    Console.WriteLine("Client disconnected.");
        //}

    }
}