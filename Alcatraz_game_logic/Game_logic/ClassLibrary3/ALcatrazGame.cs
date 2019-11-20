using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Alcatraz;

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

            int players = 3;

            ClientClass clientClass = new ClientClass(players);
            List<Client> clientList = new List<Client>();

            for(int i = 0; i < players; i++)
            {
                clientList.Add(clientClass.initializeClient(i,players));
            }

            List<ClientClass> clientClassList = new List<ClientClass>();
            List<Alcatraz> alcatrazList = new List<Alcatraz>();

            foreach (Client clientItem in clientList)
            {
                clientClassList.Add(clientItem.getClientClass());
                alcatrazList.Add(clientItem.getAlcatraz());
         
            }

            int counterForEachTest = 0;
            foreach (ClientClass clientClassItem in clientClassList)
            {
                int counterForEachAlcatraz = 0;
                int iterator = 0;
                foreach (Alcatraz alcatrazItem in alcatrazList)
                {
                    if (counterForEachTest != counterForEachAlcatraz)
                    {
                        clientClassItem.setOther(iterator, alcatrazItem);
                        Console.WriteLine("t" + counterForEachTest + ".setOther( " + iterator + "," + counterForEachAlcatraz);
                        iterator++;
                    }
                    counterForEachAlcatraz++;
                }
                counterForEachTest++;
            }
            for (int ii = 0; ii < players; ii++)
            {
                alcatrazList[ii].showWindow();
                alcatrazList[ii].addMoveListener(clientClassList[ii]);
                alcatrazList[ii].getWindow().FormClosed += new FormClosedEventHandler(Test_FormClosed);
                alcatrazList[ii].start();
            }
            Application.Run();
        }

        public static void Test_FormClosed(object sender, FormClosedEventArgs args)
        {
            Environment.Exit(0);
        }
 
    }
}
