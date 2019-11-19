using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Alcatraz;

namespace Alcatraz
{

    public class Test : MoveListener
    {
        private Alcatraz[] other = new Alcatraz[4];
        private int numPlayer = 3;

        public Test()
        {
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Test t1 = new Test();
            Alcatraz a1 = new Alcatraz();
            int players = 3;
          
            List<Test> testList = new List<Test>();
            List<Alcatraz> alcatrazList = new List<Alcatraz>();

            for (int i = 0; i < players; i++)
            {

                t1 = new Test();
                a1 = new Alcatraz();
   
                t1.setNumPlayer(players);
                //i is in this case the playersID
                a1.init(players, i);
                Console.WriteLine("a1.init("+players+" , "+i);

                for (int j = 1; j < players + 1; j++)
                {
                    int help = j - 1;
                    a1.getPlayer(help).Name = "Player " + j;

                    Console.WriteLine("getPlayer("+help+").Name = Player "+j);
                }
                
                alcatrazList.Add(a1);
                testList.Add(t1);
            }
            int counterForEachTest = 0;
            foreach (Test testListItem in testList)
            {
                int counterForEachAlcatraz = 0;
                int iterator = 0;
                foreach(Alcatraz alcatrazItem in alcatrazList)
                {
                    if (counterForEachTest != counterForEachAlcatraz)
                    {
                        testListItem.setOther(iterator, alcatrazItem);
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
                alcatrazList[ii].addMoveListener(testList[ii]);
                alcatrazList[ii].getWindow().FormClosed += new FormClosedEventHandler(Test_FormClosed);
                alcatrazList[ii].start();
            }
            Application.Run();
        }

        public static void Test_FormClosed(object sender, FormClosedEventArgs args)
        {
            Environment.Exit(0);
        }

        public void setOther(int i, Alcatraz t)
        {
            this.other[i] = t;
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
            for (int i = 0; i < getNumPlayer()-1; i++)
            {
                other[i].doMove(other[i].getPlayer(player.Id), other[i].getPrisoner(prisoner.Id), rowOrCol, row, col);
                Console.WriteLine("Player " + other[i].getPlayer(player.Id) + "Prisoner " + prisoner + "row " + row + "col" + col);
            }
        }

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
