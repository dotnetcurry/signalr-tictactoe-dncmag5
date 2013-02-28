using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using TicTacToR.Web.SignalrHubs;

namespace TicTacToR.Web.Game
{
    public class Manager
    {
        private static readonly Manager instance
            = new Manager();

        private Dictionary<string, UserCredential> _connections;
        private Dictionary<Guid, GameDetails> _games;

        static Manager()
        {

        }

        private Manager()
        {
            _connections = new Dictionary<string, UserCredential>();
            _games = new Dictionary<Guid, GameDetails>();
        }

        public static Manager Instance
        {
            get
            {
                return instance;
            }
        }

        public GameDetails Game(Guid gameId)
        {
            GameDetails game;
            if (!_games.ContainsKey(gameId))
            {
                _games[gameId] = new GameDetails { GameId = gameId };
            }
            game = _games[gameId];
            return game;
        }

        public object AllUsers()
        {
            var u = _connections.Values.Select(s => new
            {
                UserId = s.UserId,
                ConnectionStatus = (int)s.ConnectionStatus,
                ConnectionId = s.Sessions[s.Sessions.Count - 1].ConnectionId
            });
            return u;
        }

        private void PingClients()
        {
            var hubContext =
                    GlobalHost
                        .ConnectionManager
                            .GetHubContext<GameNotificationHub>();
            foreach (var item in _connections.Values)
            {
                TimeSpan span =
                    new TimeSpan(item.GetSessionLengthInTicks());
                ConnectionSession session =
                    item.Sessions[item.Sessions.Count - 1];
                hubContext.Clients.All
                    .tick(session.ConnectionId, item.UserId, span.ToString());
            }
        }

        private void CreateNewUserSession(string userId, string connectionId)
        {
            UserCredential currentCred = new UserCredential
            {
                ConnectionStatus = ConnectionStatus.Connected,
                UserId = userId
            };
            currentCred.Sessions.Add(new ConnectionSession
            {
                ConnectionId = connectionId,
                ConnectedTime = DateTime.Now.Ticks,
                DisconnectedTime = 0L//,
                //ParentUser = currentCred
            });
            _connections.Add(userId, currentCred);
        }

        private void UpdateUserSession(string userId,
            string connectionId, ConnectionStatus status)
        {
            UserCredential currentCred = _connections[userId];

            ExpireSession(currentCred);
            currentCred.Sessions.Add(new ConnectionSession
            {
                ConnectionId = connectionId,
                ConnectedTime = DateTime.Now.Ticks,
                DisconnectedTime = 0L//,
                //ParentUser = currentCred
            });
            currentCred.ConnectionStatus = status;
        }

        private static void ExpireSession(UserCredential currentCred)
        {
            ConnectionSession currentSession =
                currentCred.Sessions.Find(sess =>
                    sess.DisconnectedTime == 0);
            if (currentSession != null
                && currentSession.DisconnectedTime == 0)
            {
                currentSession.DisconnectedTime = DateTime.Now.Ticks;
            }
        }

        internal void UpdateCache(string userId,
            string connectionId, ConnectionStatus status)
        {
            if (_connections.ContainsKey(userId)
                && !string.IsNullOrEmpty(userId))
            {
                UpdateUserSession(userId, connectionId, status);
            }
            else
            {
                CreateNewUserSession(userId, connectionId);
            }
        }

        internal void Disconnect(string connectionId)
        {
            ConnectionSession session = null;
            if (_connections.Values.Count > 0)
            {
                foreach (var currentCredi in _connections.Values)
                {
                    session = currentCredi.Sessions.Find(ss =>
                        ss.ConnectionId == connectionId);
                    if (session != null)
                    {
                        session.DisconnectedTime = DateTime.Now.Ticks;
                        break;
                    }
                }
            }
        }

        internal void Logout(string userId)
        {
            ExpireSession(this._connections[userId]);
            // Save to DB If required
            this._connections.Remove(userId);
        }

        internal GameDetails NewGame(string player1Id, string player2Id)
        {
            UserCredential player1 = _connections.Values.FirstOrDefault<UserCredential>
                (c => c.Sessions.FirstOrDefault<ConnectionSession>
                    (s => s.ConnectionId == player1Id) != null);
            UserCredential player2 = _connections.Values.FirstOrDefault<UserCredential>
                (c => c.Sessions.FirstOrDefault<ConnectionSession>
                    (s => s.ConnectionId == player2Id) != null);
            GameDetails newGame = new GameDetails
            {
                GameId = Guid.NewGuid(),
                User1Id = player1,
                User2Id = player2,
                NextTurn = player1.UserId
            };
            _games[newGame.GameId] = newGame;
            return newGame;
        }
    }
}