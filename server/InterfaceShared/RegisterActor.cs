using Akka.Actor;
using Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInterface
{
    public class RegisterActor : ReceiveActor, IHandle<string>
    {

        private ICancelable _helloTask;
        public RegisterActor()
        {

            string path = @"c:\temp\";


            Receive<Hello>(hello =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, hello.Message);
                Sender.Tell(hello);
            });

            Receive<Client>(client =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, client.UniqueName + "--" + client.Address + "--" /*+ client.port*/);
                if (!File.Exists(path + client.UniqueName + ".txt"))
                    File.WriteAllText(path + client.UniqueName + ".txt", "name:" + client.UniqueName + "ip:" + client.Address /*+ "port:" + client.port*/);
                else
                    Sender.Tell("already registered");

                Sender.Tell("already registered", ActorRefs.NoSender);
                //Sender.Tell("already registered", Self);
                this.Self.Tell("already registered");
                //Self.Tell("Self send");
                Sender.Tell(" Server " + client.UniqueName);

            });

            Receive<Terminated>(terminated =>
            {
                Console.WriteLine(terminated.ActorRef);
                Console.WriteLine("Was address terminated? {0}", terminated.AddressTerminated);

            }); 
        }

        protected override void PreStart()
        {
            //_helloTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
            //    TimeSpan.FromSeconds(1), Context.Self, new Hello("hi"), ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            _helloTask.Cancel();
        }

        public void Handle(string message)
        {
            Sender.Tell("received", Self);
            Console.WriteLine(message);
        }
    }
}
