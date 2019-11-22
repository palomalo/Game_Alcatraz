using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInterface
{
    public class Client: ReceiveActor
    {
        public int sequenceID { get; set; }              //seqence ID assigned by Server at registration
        public String unique_name { get; set; }          // unique player name   
        public int preferred_group_size { get; set; }    //  preferred group size            
        public String ipAddress { get; set; }            // address of client to contact him back         
        public int port { get; }                         //port which is used for the

        public Client() {

            Receive<Hello>(hello =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, hello.Message);
                Sender.Tell(hello);
            });

            Receive<Client>(client =>
            {
                //Console.WriteLine("[{0}]: {1}", Sender, client.unique_name + "--" + client.ipAddress + "--" + client.port);
                //if (!File.Exists(path + client.unique_name + ".txt"))
                //    File.WriteAllText(path + client.unique_name + ".txt", "name:" + client.unique_name + "ip:" + client.ipAddress + "port:" + client.port);
                //else
                //    Sender.Tell("already registered");

                //Sender.Tell("already registered", ActorRefs.NoSender);
                //Sender.Tell("already registered", Self);
                //this.Self.Tell("already registered");
                //Self.Tell("Self send");
                //Sender.Tell(" Server " + client.unique_name);

            });

        }

        public Client(int sequenceID, String unique_name, int preferred_group_size, String ipAddress, int port)
        {
            this.sequenceID = sequenceID;
            this.unique_name = unique_name;
            this.preferred_group_size = preferred_group_size;
            this.ipAddress = ipAddress;
            this.port = port;
        }



    }
}
