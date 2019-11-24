using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerService.Service
{
    public class ClientHandler
    {
        public ClientHandler(Socket clientSocket, Action<string, Socket> action)
        {
            this.ClientSocket = clientSocket;
            this.action = action;

            clientReceiveThread = new Thread(() =>
            {
                string message = string.Empty;
                while (!message.Equals(endMessage))
                {
                    int length = ClientSocket.Receive(buffer);
                    message = Encoding.UTF8.GetString(buffer, 0, length);
                    action(message, ClientSocket);
                }
                Close();
            });

            clientReceiveThread.Start();
        }

        public void Close()
        {
            Send(endMessage);
            ClientSocket.Close();
            clientReceiveThread.Abort();
        }

        public void Send(string message)
        {
            ClientSocket.Send(Encoding.UTF8.GetBytes(message));
        }

        private Action<string, Socket> action;
        private byte[] buffer = new byte[512 * 2];
        private Thread clientReceiveThread;
        const string endMessage = @"quit";

        public string Name { get; private set; }
        public Socket ClientSocket { get; private set; }
    }
}
