using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Akka.Actor;
using Alcatraz;
using GameInterface;

namespace Alcatraz

{
    public class Test
    {
        
        public Test()
        {

        }
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int players = 2;

            ClientClass clientClass = new ClientClass(players);
            List<Client> clientList = new List<Client>();
            string test = "wwww";
            Client clientItem = clientClass.initializeClient(0,players);

            List<ClientClass> clientClassList = new List<ClientClass>();
            List<Alcatraz> alcatrazList = new List<Alcatraz>();
 
           // foreach (Client clientItem in clientList)
            //{
            clientItem.getClientClass();
            clientItem.getAlcatraz();
            clientItem.getAlcatraz();
     
            clientItem.getAlcatraz().showWindow();
            clientItem.getAlcatraz().addMoveListener(clientItem.getClientClass());
            clientItem.getAlcatraz().getWindow().FormClosed += new FormClosedEventHandler(Test_FormClosed);
            clientItem.getAlcatraz().start();

            Application.Run();
        }

        public static void Test_FormClosed(object sender, FormClosedEventArgs args)
        {
            Environment.Exit(0);
        }
 
    }
}
