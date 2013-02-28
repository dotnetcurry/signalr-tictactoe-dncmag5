using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToR.Web.Game
{
    public class UserCredential
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }
        public List<ConnectionSession> Sessions { get; set; }
        public long GetSessionLengthInTicks()
        {
            long totalSession = 0;
            foreach (var session in Sessions)
            {
                if (session.DisconnectedTime != 0)
                {
                    totalSession += session.DisconnectedTime - session.ConnectedTime;
                }
                else
                {
                    totalSession += DateTime.Now.Ticks - session.ConnectedTime;
                }
            }
            return totalSession;
        }

        public UserCredential()
        {
            Sessions = new List<ConnectionSession>();
        }
    }
}
