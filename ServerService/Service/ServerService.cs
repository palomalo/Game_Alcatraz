using Microsoft.Extensions.Logging;
using ServerService.Client;
using ServerService.Configuration;
using ServerService.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerService.Service
{
    public class ServerService : IServerService
    {
        private ILogger<ServerService> Logger { get; set; }
        private Socket _serverSocket;
        private List<ClientHandler> _clients = new List<ClientHandler>();
        private Action<string> _notifier;
        private Thread _acceptingThread;

        public ServerService(ILogger<ServerService> logger)
        {
            Logger = logger;
        }

        public void InitializeServerService(Action<string> notifier)
        {
            _notifier = notifier;
            string _ipAddress = string.Empty;
            if(ServerOptions.IPAddress == null || ServerOptions.IPAddress.Equals(string.Empty))
            {
                _ipAddress = IPAddress.Loopback.ToString();
            }
            else
            {
                _ipAddress = ServerOptions.IPAddress;
            }

            try
            {
                var _ipEndpoint = new IPEndPoint(IPAddress.Parse(_ipAddress), ServerOptions.Port);

                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(_ipEndpoint);
                _serverSocket.Listen(5);
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
                Logger.LogError("Could not start TCP communication for the Server. See details below: ");
                Logger.LogError(msg);
                throw ex;
            }
        }

        public void DisconnectClient(string clientName)
        {
            foreach (var item in _clients)
            {
                if (item.Name.Equals(clientName))
                {
                    item.Close();
                    _clients.Remove(item);
                    break;
                }
            }
        }

        public void StartAccepting()
        {
            _acceptingThread = new Thread(new ThreadStart(() =>
            {
                while (_acceptingThread.IsAlive)
                {
                    try
                    {
                        _clients.Add(new ClientHandler(_serverSocket.Accept(), new Action<string, Socket>(NewMessageReceived)));
                    }
                    catch (Exception ex)
                    {
                        var msg = $"{ex.Message} : {ex.InnerException}";
                        Logger.LogError("Failed to register client to start server communication. See details below: ");
                        Logger.LogError(msg);
                    }
                }
            }));

            _acceptingThread.IsBackground = true;
            _acceptingThread.Start();
        }

        private void NewMessageReceived(string message, Socket senderSocket)
        {
            _notifier(message);

            foreach (var item in _clients) 
            {
                if(item.ClientSocket != senderSocket)
                {
                    item.Send(message);
                }
            }
        }

        public string Register(ClientDemo clientToRegister)
        {
            throw new NotImplementedException();
        }

        public string SetPrimaryServer(string ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayerList(ClientDemo[] clients)
        {
            throw new NotImplementedException();
        }

        public void DeletePlayerFromList(ClientDemo clientToRemove)
        {
            throw new NotImplementedException();
        }

        public void Start(ClientDemo[] clients, int gameID)
        {
            throw new NotImplementedException();
        }
    }
}
