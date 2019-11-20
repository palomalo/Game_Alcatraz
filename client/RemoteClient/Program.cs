using System;
using Akka.Actor;
using Akka.Configuration;
using GameInterface;

namespace ClientPlayer
{
    class Program
    {

        class ClientActor : ReceiveActor
        {
            private IActorRef _remoteActor;
            private int _gameCapacity;
            private ICancelable _clientTask;

            public int sequenceID { get; set; }              //seqence ID assigned by Server at registration
            public String unique_name { get; set; }          // unique player name   
            public int preferred_group_size { get; set; }    //  preferred group size            
            public String ipAddress { get; set; }            // address of client to contact him back         
            public int port { get; }                         //port which is used for the
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

            protected override void PreStart()
            {
                _clientTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1), Context.Self, new SayHello(), ActorRefs.NoSender);
            }

            protected override void PostStop()
            {
                _clientTask.Cancel();
            }

            protected override void Unhandled(object message)
            {
                //Do something with the message.
            }
        }

        class JoinGame { }
        class SayHello { }


        static void Main(string[] args)
        {
            string actorSystemName = "peer1";
            Console.Title = actorSystemName;

            try
            {
                using (var actorSystem = ActorSystem.Create(actorSystemName))
                {

                    var localChatActor = actorSystem.ActorOf(Props.Create<MessagingActor>(), "MessagingActor");
                    var child = actorSystem.ActorOf(Props.Create<MessagingActor>(), "MessagingActorChild");

                    string remoteActorAddress = "akka.tcp://peer2@localhost:5555/user/MessagingActor";
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

