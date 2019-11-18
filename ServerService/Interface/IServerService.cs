using ServerService.Client;
using ServerService.Configuration;
using System;

namespace ServerService.Interface
{
    public interface IServerService
    {
        string Register(ClientDemo clientToRegister);

        string SetPrimaryServer(string ipAddress, int port);

        void UpdatePlayerList(ClientDemo[] clients);

        void DeletePlayerFromList(ClientDemo clientToRemove);

        void Start(ClientDemo[] clients, int gameID);

        void InitializeServerService(Action<string> _notifier);

        void StartAccepting();

        void DisconnectClient(string clientName);
    }
}
