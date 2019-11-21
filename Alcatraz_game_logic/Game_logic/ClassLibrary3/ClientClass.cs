using Akka.Actor;
using GameInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Alcatraz
{
    class ClientClass : MoveListener
    {

        private int numPlayer;
        private Alcatraz[] other;
        public string remoteActorAddressClient1;
        private string remoteActorAddressClient2;
        private IActorRef localChatActor;
        private IActorRef child;
        private ActorSystem actorSystem;
        private string actorSystemName;
        public ActorSelection remoteChatActorClient1;
        public ActorSelection remoteChatActorClient2;

        private ClientClass clientClass;

        public ClientClass(int numberOfPlayers)
        {
            other = new Alcatraz[numberOfPlayers];
            numPlayer = numberOfPlayers;
            this.actorSystemName = "client0";
            actorSystem = ActorSystem.Create(actorSystemName);

            remoteActorAddressClient1 = "akka.tcp://client1@localhost:2222/user/GameActor";
            remoteActorAddressClient2 = "akka.tcp://client2@localhost:3333/user/GameActor";
            this.remoteChatActorClient1 = actorSystem.ActorSelection(remoteActorAddressClient1);
            this.remoteChatActorClient2 = actorSystem.ActorSelection(remoteActorAddressClient2);
            this.localChatActor = actorSystem.ActorOf(Props.Create<GameActor>(), "GameActor");
            this.child = actorSystem.ActorOf(Props.Create<GameActor>(), "GameActorClient0Child");
        }

        public Client initializeClient(int playerID, int numberOfPlayer)
        {

            clientClass.setClientClass(numberOfPlayer);

            Alcatraz clientAlcatraz = new Alcatraz();
            clientClass.setNumPlayer(numPlayer);
            clientAlcatraz.init(numPlayer, playerID);

            for (int j = 1; j < numPlayer + 1; j++)
            {
                int help = j - 1;
                clientAlcatraz.getPlayer(help).Name = "Player " + j;
            }

            Client client = new Client(clientClass, clientAlcatraz, playerID);

            this.remoteChatActorClient1.Tell(client, this.child);

            return client;
        }


        public void setClientClass(int numberOfPlayer)
        {
            this.clientClass = new ClientClass(numberOfPlayer);
        }
        public ClientClass getClientClass()
        {
            return this.clientClass;
        }


        public int getNumPlayer()
        {
            return numPlayer;
        }

        public void setNumPlayer(int numPlayer)
        {
            this.numPlayer = numPlayer;
        }

        public void setOther(int i, Alcatraz t)
        {
            this.other[i] = t;
        }

        public void doMove(Player player, Prisoner prisoner, int rowOrCol, int row, int col)
        {
            Console.WriteLine("moving " + prisoner + " to " + (rowOrCol == Alcatraz.ROW ? "row" : "col") + " " + (rowOrCol == Alcatraz.ROW ? row : col));
            Console.WriteLine("ID" + player.Id);


            /*  if (player.Id != i)
              {
                  this.remoteChatActorClient1.Tell(this.convertMove(player, prisoner, rowOrCol, row, col), this.child);
                  Console.WriteLine("send Move " + this.convertMove(player, prisoner, rowOrCol, row, col) + " to client " + i);
              }
              else { */
            this.remoteChatActorClient1.Tell(this.convertMove(player, prisoner, rowOrCol, row, col), this.child);
            //AKKA send to client i 
            //Console.WriteLine("send Move " + this.convertMove(player, prisoner, rowOrCol, row, col) +" to client "+);

            // }

        }

        public Move convertMove(Player player, Prisoner prisoner, int rowOrCol, int row, int col)
        {
            return new Move(player, prisoner, rowOrCol, row, col);
        }

        /* public void receiveMove(Move receivedMove)
         {
             Player player = receivedMove.getPLayer();
             Prisoner prisoner = receivedMove.getPrisoner();
             int rowOrCol = receivedMove.getRowOrCol();
             int row = receivedMove.getRow();
             int col = receivedMove.getCol();
             // Move returnMove = new Move(player, prisoner, rowOrCol, row, col);
             this.doMove(player, prisoner, rowOrCol, row,col);  
         }*/


        public void undoMove()
        {
            Console.WriteLine("Undoing move");
        }

        public void gameWon(Player player)
        {
            Console.WriteLine("Player " + player.Id + " wins.");
        }

        public static void main(String[] args)
        {
            Console.WriteLine("main main main");
        }
    }
}
