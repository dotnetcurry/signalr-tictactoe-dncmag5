using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToR.Web.Game
{
    public enum ConnectionStatus
    {
        Connected = 0,
        Disconnected,
        Refreshed,
        Challenged,
        Challenging,
        Playing
    }
}
