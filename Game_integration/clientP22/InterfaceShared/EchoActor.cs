﻿using System;
using Akka.Actor;
using Akka.Event;
using Interface;

namespace GameInterface
{
    /// <summary>
    /// Actor that just replies the message that it received earlier
    /// </summary>
    public class EchoActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        public EchoActor()
        {

            Receive<Hello>(hello =>
            {
                Console.WriteLine("[{0}]: {1}", Sender, hello.Message);
                Sender.Tell(hello);
            });

            Receive<Msg>(msg => {
                // echo message back to sender
                Sender.Tell(msg);
            });

            Receive<Players>(player => {
                // echo message back to sender
                Sender.Tell(player.printPlayerCounter());
            });

        }
    }

    public class Msg
    {
        public string Content { get; set; }
    }
}
