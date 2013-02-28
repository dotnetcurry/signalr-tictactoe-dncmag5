using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToR.Web.Game
{
    public class ConnectionSession
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public long ConnectedTime { get; set; }
        public long DisconnectedTime { get; set; }
        //public UserCredential ParentUser { get; set; }
    }
}
