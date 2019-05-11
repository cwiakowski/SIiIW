using System;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Morris.Models;

/*
 * CO JESZCZE
 * sprawdzanie czy kamienie > 2
 * sprawdzanie czy jest jakas herustyka wybrana
 * implementacja interfejsu bota co odpierdala losowy szajs
 * implementacja minmax/alfabeta
 * implementacja jakistam heurystyk siema
 */

namespace Morris.Controllers
{
    public class GameController
    {
        public BoardController BoardController { get; set; }
        public GameState State { get; set; }
        private int _moves = 0;
        private TextBlock _movesTextBlock;
        private TextBlock _headerTextBlock;
        private bool _won = false;
        private PlayersTurn _playersTurn;
        private int _stones;

        public GameController(Grid grid, TextBlock textBlock, TextBlock header)
        {
            _movesTextBlock = textBlock;
            _headerTextBlock = header;
            State = GameState.Off;
            BoardController = new BoardController(ref grid);
        }

        public void PrintMills()
        {
            var mills = BoardController.GetNewMills();
            if (mills.Any())
            {
                var sb = new StringBuilder();
                foreach (var m in mills)
                {
                    sb.Append($"{m.ToString()}\n");
                }

                sb.Append(BoardController.LastMills.Count());
                BoardController.UpdateLastMills();
                State = GameState.RemovingStone;
                new MessageDialog(sb.ToString(), "Mills").ShowAsync();
            }
            BoardController.UpdateLastMills();
        }


        public bool Act(string input)
        {
            if (State == GameState.Off)
            {
                return false;
            }

            try
            {
                var s = input.Split(' ');
                if (s.Length == 1 && State.Equals(GameState.PlacingStones))
                {
                    if (BoardController.ChangeValue(_playersTurn, s[0]))
                    {
                        if (!BoardController.GetNewMills().Any())
                        {
                            _playersTurn = _playersTurn == PlayersTurn.Player1
                                ? PlayersTurn.Player2
                                : PlayersTurn.Player1;
                        }
                        else
                        {
                            State = GameState.RemovingStone;
                        }
                        _stones++;
                        if (18 <= _stones)
                        {
                            State = GameState.InGame;
                        }
                        PrintMills();
                        return true;
                    }

                    return false;
                }
                else if(s.Length == 1 && State.Equals(GameState.RemovingStone))
                {
                    if (BoardController.RemoveEnemyStone(_playersTurn, s[0]))
                    {
                        _playersTurn = _playersTurn == PlayersTurn.Player1
                            ? PlayersTurn.Player2
                            : PlayersTurn.Player1;
                        if (18 <= _stones)
                        {
                            State = GameState.InGame;
                        }
                        else
                        {
                            State = GameState.PlacingStones;
                        }
                        BoardController.UpdateLastMills();
                        if (IsGameOver())
                        {
                            _headerTextBlock.Text = "Mlynek v3.2.5";
                            State = GameState.Off;
                            new MessageDialog($"{_playersTurn} IS A LOSER", "GAME OVER").ShowAsync();
                        }
                        return true;
                    }

                    return false;
                }
                else if (s.Length == 2 && State.Equals(GameState.InGame))
                {
                    bool res = BoardController.Move(s[0], s[1], _playersTurn);
                    if (res)
                    {
                        _moves++;
                        if (!BoardController.GetNewMills().Any())
                        {
                            _playersTurn = _playersTurn == PlayersTurn.Player1
                                ? PlayersTurn.Player2
                                : PlayersTurn.Player1;
                        }
                        PrintMills();
                    }

                    return res;
                }
                else if (s[0] == "neigh")
                {
                    new MessageDialog(Neigh(s[1]), $"{s[1]} Neighbors").ShowAsync();
                    return true;
                }

            }
            catch (Exception e)
            {
                new MessageDialog(e.StackTrace, $"Error {e.Message}").ShowAsync();
                return false;
            }
            finally
            {
                UpdateBoard();
            }
            
            return false;
        }

        private bool IsGameOver()
        {
            return State == GameState.InGame && BoardController.CalculateAmountOfFields(_playersTurn) == 2;
        }

        public string Neigh(string cords)
        {
            var s = new StringBuilder();
            var neighbors = BoardController.GetNeighbors(cords);
            s.Append("Neighbors: ");
            foreach (var n in neighbors)
            {
                s.Append($"{n.Cords} ");
            }

            s.Append("\nAvailable moves: ");
            foreach (var n in BoardController.AvailableMoves(cords))
            {
                s.Append($"{n.Cords} ");
            }

            return s.ToString();
        }

        public void NewGame()
        {
            _stones = 0;
            State = GameState.PlacingStones;
            BoardController.NewGame();
            _playersTurn = PlayersTurn.Player1;
            UpdateHeader();

        }

        private void UpdateHeader()
        {
            switch (State)
            {
                case GameState.PlacingStones:
                    _headerTextBlock.Text = $"{_playersTurn}: place a stone";
                    break;
                case GameState.InGame:
                    _headerTextBlock.Text = $"{_playersTurn}: make a move";
                    break;
                case GameState.RemovingStone:
                    _headerTextBlock.Text = $"{_playersTurn}: remove enemy's stone";
                    break;
            }
        }


        public void GenerateExample()
        {
            BoardController.GenerateExample();
            _stones = 18;
            State = GameState.InGame;
            _playersTurn = PlayersTurn.Player1;
            UpdateBoard();
        }

        public void UpdateBoard()
        {
            BoardController.ColorWholeGrid();
            UpdateHeader();
        }
    }
}