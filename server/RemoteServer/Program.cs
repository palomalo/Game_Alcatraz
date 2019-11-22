using Akka.Actor;
using GameInterface;
using Newtonsoft.Json;
using System;

namespace Server
{

    class ServerActor : ReceiveActor
    {
        private IActorRef _remoteActor;
        private int _gameCapacity;
        private ICancelable _clientTask;

        public int sequenceID { get; set; }              //seqence ID assigned by Server at registration
        public String unique_name { get; set; }          // unique player name   
        public int preferred_group_size { get; set; }    //  preferred group size            
        public String ipAddress { get; set; }            // address of client to contact him back         
        public int port { get; }                         //port which is used for the
        
        /*
        public ServerActor(IActorRef remoteActor)
        {
            _remoteActor = remoteActor;
            Context.Watch(_remoteActor);
            Receive<Client>(client =>
            {
                Console.WriteLine("Received {1} from {0}", Sender, client.unique_name);
            });

            Receive<JoinGame>(sayHello =>
            {
                _remoteActor.Tell(new Client(1, "hello", 2, "localhost", 5522));
            });

            Receive<Terminated>(terminated =>
            {
                Console.WriteLine(terminated.ActorRef);
                Console.WriteLine("Was address terminated? {0}", terminated.AddressTerminated);
                _clientTask.Cancel();
            });

        }
        */

        class JoinGame { }
        class SayHello { }


        protected override void PreStart()
        {
            _clientTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1), Context.Self, new SayHello(), ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            _clientTask.Cancel();
        }
    }


    class Program
        {
         
            static void Main(string[] args)
            {
                string actorSystemName = "peer2";
                Console.Title = actorSystemName;

                try
                {
                    using (var actorSystem = ActorSystem.Create(actorSystemName))
                    {

                        var localChatActor = actorSystem.ActorOf(Props.Create<Client>(), "client");
                        //var child = actorSystem.ActorOf(Props.Create<Client>(), "ClientChild");

                        string remoteActorAddress = "akka.tcp://peer1@localhost:6666/user/MessagingActor";
                        var remoteChatActor = actorSystem.ActorSelection(remoteActorAddress);

                        if (remoteChatActor != null)
                        {
                            string line = string.Empty;
                            while (line != null)
                            {
                                line = Console.ReadLine();
                                //string json = JsonConvert.SerializeObject(line);
                              

                                remoteChatActor.Tell(line, localChatActor);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Could not get remote actor ref");
                            Console.ReadLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }


        }
    }

