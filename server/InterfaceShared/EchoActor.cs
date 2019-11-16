using System;
using Akka.Actor;
using System.IO;
using Akka.Configuration;

namespace GameInterface
{
    /// <summary>
    /// Actor that just replies the message that it received earlier
    /// </summary>
    public class EchoActor : ReceiveActor, IHandle<string>
    {

        public EchoActor()
        {
            string path = @"c:\temp\";
            Receive<Hello>(hello =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, hello.Message);
                Sender.Tell(hello);
            });

            Receive<Client>(client =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, client.unique_name + "--" + client.ipAddress + "--" + client.port);
                if (!File.Exists(path + client.unique_name + ".txt"))
                    File.WriteAllText(path + client.unique_name + ".txt", "name:" + client.unique_name + "ip:" + client.ipAddress + "port:" + client.port);
                else
                    Sender.Tell("already registered");

                Sender.Tell("already registered", ActorRefs.NoSender);
                //Sender.Tell("already registered", Self);
                this.Self.Tell("already registered");
                //Self.Tell("Self send");
                Sender.Tell(" Server " + client.unique_name);

            });
        }
        public void Handle(string message)
        {
            Sender.Tell("received");
            Sender.Tell("received", Self);
            Console.WriteLine(message);
        }
    }
}

