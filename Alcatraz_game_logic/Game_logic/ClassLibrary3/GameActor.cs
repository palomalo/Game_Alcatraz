using System;
using Akka.Actor;
using Akka.Event;
using Alcatraz;
using Interface;

namespace GameInterface
{
    /// <summary>
    /// Actor that just replies the message that it received earlier
    /// </summary>
    public class GameActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        public GameActor()
        {

            Receive<Hello>(hello =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, hello.Message);
                Sender.Tell(hello);
            });

            Receive<Msg1>(msg => {
                // echo message back to sender
                Sender.Tell(msg);
            });

            Receive<Players>(player => {
                // echo message back to sender
                Sender.Tell(player.printPlayerCounter());
            });

            Receive<Move>(player => {
                // echo message back to sender
                Sender.Tell("ss");
            });

        }
    }

    public class Msg1
    {
        public string Content { get; set; }
    }
}
