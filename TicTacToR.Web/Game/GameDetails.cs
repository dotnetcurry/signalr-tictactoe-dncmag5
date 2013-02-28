using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToR.Web.Game
{
    public class GameDetails
    {
        public Guid GameId { get; set; }
        public UserCredential User1Id { get; set; }
        public UserCredential User2Id { get; set; }
        private int[,] GameMatrix { get; set; }
        public string NextTurn { get; set; }
        public string Message { get; set; }
        public int GameStatus { get; set; }

        public GameDetails()
        {
            GameMatrix = new int[3, 3];
        }

        private string CheckGameStatus()
        {
            string status = CheckRows();
            if (string.IsNullOrEmpty(status))
            {
                status = CheckCols();
            }
            if (string.IsNullOrEmpty(status))
            {
                status = CheckDiagonal();
            }
            Message = !string.IsNullOrEmpty(status) ? status + " wins!" : string.Empty; 
            if (string.IsNullOrEmpty(status))
            {
                status = CheckDraw();
                Message = status; 
            }
            
            return status;
        }

        private string CheckDraw()
        {
            bool isDefault = false;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (GameMatrix[row, col] == default(int))
                    {
                        isDefault = true;
                        GameStatus = 0;
                        break;
                    }
                }
                if (isDefault)
                {
                    break;
                }
            }
            if (!isDefault)
            {
                GameStatus = 2;
            }
            return isDefault ? "In Progress": "Game Drawn";
        }

        public string SetPlayerMove(dynamic rowCol, string currentPlayerId)
        {
            int x = int.Parse(rowCol.row.ToString());
            int y = int.Parse(rowCol.col.ToString());
            string returnString = string.Empty;

            if (!string.IsNullOrEmpty(currentPlayerId)
                && GameMatrix[x - 1, y - 1] == default(int))
            {
                if (currentPlayerId == User1Id.UserId)
                {
                    returnString = "O";
                    GameMatrix[x - 1, y - 1] = 1;
                    NextTurn = User2Id.UserId;
                }
                else
                {
                    returnString = "X";
                    GameMatrix[x - 1, y - 1] = 10;
                    NextTurn = User1Id.UserId;
                }
            }
            CheckGameStatus();
            return returnString;
        }

        string CheckRows()
        {
            for (int row = 0; row < 3; row++)
            {
                int rowValue = 0;
                for (int col = 0; col < 3; col++)
                {
                    rowValue += GameMatrix[row, col];
                }
                if (rowValue == 3)
                {
                    GameStatus = 1;
                    return User1Id.UserId;
                }
                else if (rowValue == 30)
                {
                    GameStatus = 1;
                    return User2Id.UserId;
                }
            }
            return string.Empty;
        }

        string CheckCols()
        {
            for (int col = 0; col < 3; col++)
            {
                int colValue = 0;
                for (int row = 0; row < 3; row++)
                {
                    colValue += GameMatrix[row, col];
                }
                if (colValue == 3)
                {
                    GameStatus = 1;
                    return User1Id.UserId;
                }
                else if (colValue == 30)
                {
                    GameStatus = 1;
                    return User2Id.UserId;
                }
            }
            return string.Empty;
        }

        string CheckDiagonal()
        {
            int diagValueF = 0;
            int diagValueB = 0;
            for (int positonF = 0, positonB = 2; positonF < 3; positonF++, positonB--)
            {
                diagValueF += GameMatrix[positonF, positonF];
                diagValueB += GameMatrix[positonF, positonB];
            }
            if (diagValueF == 3)
            {
                GameStatus = 1;
                return User1Id.UserId;
            }
            else if (diagValueF == 30)
            {
                GameStatus = 1;
                return User2Id.UserId;
            }
            if (diagValueB == 3)
            {
                GameStatus = 1;
                return User1Id.UserId;
            }
            else if (diagValueB == 30)
            {
                GameStatus = 1;
                return User2Id.UserId;
            }
            return string.Empty;
        }
    }
}