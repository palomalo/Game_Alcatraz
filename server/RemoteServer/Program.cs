using System;
using Akka.Actor;
using Akka.Configuration;
using GameInterface;
using System.Configuration;

namespace Server
{

    class ClientActor : ReceiveActor
    {
        private IActorRef _remoteActor;
        private int _gameCapacity;
        private ICancelable _clientTask;

        //public int sequenceID { get; set; }              //seqence ID assigned by Server at registration
        //public String unique_name { get; set; }          // unique player name   
        //public int preferred_group_size { get; set; }    //  preferred group size            
        //public String ipAddress { get; set; }            // address of client to contact him back         
        //public int port { get; }                         //port which is used for the
        public ClientActor(IActorRef remoteActor)
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

        class JoinGame { }
        class SayHello { }

        //class HelloActor : ReceiveActor
        //{
        //    private IActorRef _remoteActor;
        //    private int _helloCounter;
        //    private ICancelable _helloTask;

        //    public HelloActor(IActorRef remoteActor)
        //    {
        //        _remoteActor = remoteActor;
        //        Context.Watch(_remoteActor);
        //        Receive<Hello>(hello =>
        //        {
        //            Console.WriteLine("Received {1} from {0}", Sender, hello.Message);
        //        });

        //        Receive<SayHello>(sayHello =>
        //        {
        //            _remoteActor.Tell(new Hello("hello" + _helloCounter++));
        //        });

        //        Receive<Terminated>(terminated =>
        //        {
        //            Console.WriteLine(terminated.ActorRef);
        //            Console.WriteLine("Was address terminated? {0}", terminated.AddressTerminated);
        //            _helloTask.Cancel();
        //        });
        //    }

        //    protected override void PreStart()
        //    {
        //        _helloTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
        //            TimeSpan.FromSeconds(1), Context.Self, new SayHello(), ActorRefs.NoSender);
        //    }

        //    protected override void PostStop()
        //    {
        //        _helloTask.Cancel();
        //    }
        //}

    
        class Program
        {
            //static void Main(string[] args)
            //{
            //    using (var system = ActorSystem.Create("Server", ConfigurationFactory.ParseString(@"
            //    akka {  
            //        actor.provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
            //        remote {
            //            helios.tcp {
            //          port = 8090
            //          hostname = localhost
            //            }
            //        }
            //    }")))
            //    {
            //        Console.ReadKey();
            //    }
            //}

            //static void Main(string[] args)
            //{
            //    string actorSystemName = "peer2";
            //    Console.Title = actorSystemName;

            //    try
            //    {
            //        using (var actorSystem = ActorSystem.Create(actorSystemName))
            //        {

            //            var localChatActor = actorSystem.ActorOf(Props.Create<MessagingActor>(), "MessagingActor");
            //            var child = actorSystem.ActorOf(Props.Create<MessagingActor>(), "MessagingActorChild");

            //            string remoteActorAddress = "akka.tcp://peer1@localhost:6666/user/MessagingActor";
            //            var remoteChatActor = actorSystem.ActorSelection(remoteActorAddress);

            //            if (remoteChatActor != null)
            //            {
            //                string line = string.Empty;
            //                while (line != null)
            //                {
            //                    line = Console.ReadLine();
            //                    remoteChatActor.Tell(line, child);
            //                }
            //            }
            //            else
            //            {
            //                Console.WriteLine("Could not get remote actor ref");
            //                Console.ReadLine();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //    }
            //}

            static void Main(string[] args)
            {
                string actorSystemName = "peer2";
                Console.Title = actorSystemName;

                try
                {
                    using (var actorSystem = ActorSystem.Create(actorSystemName))
                    {

                        var localChatActor = actorSystem.ActorOf(Props.Create<MessagingActor>(), "MessagingActor");
                        var child = actorSystem.ActorOf(Props.Create<MessagingActor>(), "MessagingActorChild");

                        string remoteActorAddress = "akka.tcp://peer1@localhost:6666/user/MessagingActor";
                        var remoteChatActor = actorSystem.ActorSelection(remoteActorAddress);

                        if (remoteChatActor != null)
                        {
                            string line = string.Empty;
                            while (line != null)
                            {
                                line = Console.ReadLine();
                                remoteChatActor.Tell(line, child);
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
}
