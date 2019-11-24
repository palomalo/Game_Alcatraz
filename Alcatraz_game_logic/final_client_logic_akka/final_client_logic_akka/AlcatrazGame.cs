using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Akka.Actor;
using Alcatraz;

namespace Alcatraz

{
    public class Test : MoveListener
    {
        private static Client clientItem;
        private static Client[] clientItems;
        private static ClientClass clientClass;
        private static Alcatraz clientAlcatraz;
        private static Alcatraz[] other;
        private static ClientData[] data;
        private int numPlayer;
        private static Boolean boolVar = false;
        private static Test t1;
        private static string line;

        public Test()
        {

        }

        //  [STAThread]
        public static void Main(String[] args)
        {

            data = new ClientData[1];
            data[0] = new ClientData("akka.tcp://client2@localhost:", 2222, "/user/GameActor", 1, "client2");
            other = new Alcatraz[data.Length+1];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            clientClass = new ClientClass(data,"client1");
            clientItem = clientClass.initializeClient(1, data.Length+1, data, "client1");

            t1 = new Test();
            t1.setNumPlayer(data.Length+1);


            // line = Console.ReadLine();
            line = "";
            //System.Threading.Thread.Sleep(5000);

            while (line != null)
            {
                line = Console.ReadLine();
                if(line == "start")
                {
                    clientAlcatraz = clientItem.getAlcatraz();
                    clientItem.getAlcatraz().showWindow();
                    clientAlcatraz.addMoveListener(t1);
                    clientItem.getAlcatraz().getWindow().FormClosed += new FormClosedEventHandler(Test_FormClosed);
                    clientItem.getAlcatraz().start();
                    Application.Run();
                }
               
            }

        }
        

        public void setOther(int i, Alcatraz t)
        {
            other[i] = t;
        }

        public static void receiveClients(Client[] clients)
        {
            clientItems = clients;
            Console.WriteLine(clients.Length);
            for(int i = 0; i < clients.Length; i++)
            {
                t1.setOther(i,clients[i].getAlcatraz());
            }
            line = "start";
        }

        public static void Test_FormClosed(object sender, FormClosedEventArgs args)
        {
            Environment.Exit(0);
        }

        public int getNumPlayer()
        {
            return numPlayer;
        }

        public void setNumPlayer(int numPlayer)
        {
            this.numPlayer = numPlayer;
        }

        public void doMove(Player player, Prisoner prisoner, int rowOrCol, int row, int col)
        {
            Console.WriteLine("moving " + prisoner + " to " + (rowOrCol == Alcatraz.ROW ? "row" : "col") + " " + (rowOrCol == Alcatraz.ROW ? row : col));
            Console.WriteLine("ID" + player.Id);
            ActorSelection[] remoteActors = ClientClass.getRemoteChatActorClient();

            for(int i = 0; i < remoteActors.Length; i++)
            {
                remoteActors[i].Tell(this.convertMove(player, prisoner, rowOrCol, row, col), clientClass.getChild());
            }
        }

        public Move convertMove(Player player, Prisoner prisoner, int rowOrCol, int row, int col)
        {
            return new Move(player, prisoner, rowOrCol, row, col);
        }

        public void undoMove()
        {
            Console.WriteLine("Undoing move");
        }

        public void gameWon(Player player)
        {
            Console.WriteLine("Player " + player.Id + " wins.");
        }

    /*    public static void main(String[] args)
        {
            Console.WriteLine("main main main");
        }*/
    }
}
