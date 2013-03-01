using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacToR.Web.Game;

namespace TicTacToR.Web.SignalrHubs
{
    [HubName("GameNotificationHub")]
    [Authorize]
    public class GameNotificationHub : Microsoft.AspNet.SignalR.Hub
    {
        public void EnterGame()
        {

        }

        /// <summary>
        /// Incoming Challenge
        /// </summary>
        /// <param name="connectionId">The Challenged</param>
        public void Challenge(string connectionId, string userId)
        {
            this.Clients.Client(connectionId).getChallengeResponse(Context.ConnectionId, userId);
        }

        /// <summary>
        /// Incoming Challenge Acceptance
        /// </summary>
        /// <param name="connectionId">The Challenger</param>
        public void ChallengeAccepted(string connectionId)
        {
            GameDetails details = Manager.Instance.NewGame(Context.ConnectionId, connectionId);
            this.Groups.Add(Context.ConnectionId, details.GameId.ToString());
            this.Groups.Add(connectionId, details.GameId.ToString());
            this.Clients.All.beginGame(details);
        }

        /// <summary>
        /// Incoming Challenge Refusal
        /// </summary>
        /// <param name="connectionId">The Challenger</param>
        public void ChallengeRefused(string connectionId)
        {

        }

        public void GameMove(string gameGuid, dynamic rowCol)
        {
            GameDetails game = Manager.Instance.Game(new Guid(gameGuid));
            if (game != null)
            {
                string returnString = game.SetPlayerMove(rowCol, Context.User.Identity.Name);
                if (returnString != string.Empty)
                {
                    this.Clients.Group(game.GameId.ToString()).DrawPlay(rowCol, game, returnString);
                }
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            string connectionId = Context.ConnectionId;
            string connectionName = string.Empty;
            GameDetails gd = null;
            if (Context.User != null && Context.User.Identity != null
                && Context.User.Identity.IsAuthenticated)
            {
                gd = Manager.Instance.UpdateCache(
                    Context.User.Identity.Name,
                    Context.ConnectionId,
                    ConnectionStatus.Connected);
                connectionName = Context.User.Identity.Name;
                
            }
            if (gd != null && gd.GameStatus == 0)
            {
                this.Groups.Add(connectionId, gd.GameId.ToString());
                this.Clients.Client(connectionId).rejoinGame(Manager.Instance.AllUsers(), connectionName, gd);
                this.Clients.Group(gd.GameId.ToString()).rejoinGame(Manager.Instance.AllUsers(), connectionName, gd);
            }
            else
            {
                this.Clients.Caller.updateSelf(Manager.Instance.AllUsers(), connectionName);
            }
            this.Clients.Others.joined(new
                {
                    UserId = connectionName,
                    ConnectionStatus = (int)ConnectionStatus.Connected,
                    ConnectionId = connectionId
                },
                DateTime.Now.ToString());

            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            Manager.Instance.Disconnect(Context.ConnectionId);
            return Clients.All.leave(Context.ConnectionId,
                DateTime.Now.ToString());
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            string connectionName = string.Empty;
            if (!string.IsNullOrEmpty(Context.User.Identity.Name))
            {
                Manager.Instance.UpdateCache(
                    Context.User.Identity.Name,
                    Context.ConnectionId,
                    ConnectionStatus.Connected);
                connectionName = Context.User.Identity.Name;
            }
            return Clients.All.rejoined(connectionName);
        }
    }
}