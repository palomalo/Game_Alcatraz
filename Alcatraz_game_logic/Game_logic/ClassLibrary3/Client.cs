using Alcatraz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alcatraz
{
    class Client
    {
        private ClientClass clientClass;
        private Alcatraz alcatraz;
        private int playerID;

        public Client()
        {
            
        }
        public Client(ClientClass clientClass,Alcatraz alcatraz, int playerID)
        {
            this.clientClass = clientClass;
            this.alcatraz = alcatraz;
            this.playerID = playerID;
        }

        public void setPlayerID(int playerID)
        {
            this.playerID = playerID;
        }
        public int getPlayerID()
        {
            return playerID;
        }        

        public ClientClass getClientClass()
        {
            return clientClass;
        }
        public Alcatraz getAlcatraz()
        {
            return alcatraz;
        }
    }
}
