using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Morris.Bots;
using Morris.Models;

/*
 * CO JESZCZE
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
        private TextBox _commandsTextBlock;
        private AppBarToggleButton _stepByStepButton;
        private PlayersTurn _playersTurn;
        private int _stones;
        private IBot _bot1;
        private IBot _bot2;

        public GameController(Grid grid, TextBlock textBlock, TextBlock header, ref TextBox commands,
            ref AppBarToggleButton btnRandom)
        {
            _stepByStepButton = btnRandom;
            _commandsTextBlock = commands;
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
//                var sb = new StringBuilder();
//                foreach (var m in mills)
//                {
//                    sb.Append($"{m.ToString()}\n");
//                }

//                sb.Append(BoardController.LastMills.Count());
                BoardController.UpdateLastMills();
                State = GameState.RemovingStone;
                //new MessageDialog(sb.ToString(), "Mills").ShowAsync();
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
                        _commandsTextBlock.Text = $"{_commandsTextBlock.Text}+ {_playersTurn}: {s[0]}\n";
                        if (!BoardController.GetNewMills().Any())
                        {
                            NextPlayersTurn();
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
                        _commandsTextBlock.Text = $"{_commandsTextBlock.Text}- {_playersTurn}: {s[0]}\n";
                        NextPlayersTurn();
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
                        _commandsTextBlock.Text = $"{_commandsTextBlock.Text}> {_playersTurn}: {s[0]} {s[1]}\n";
                        _moves++;
                        if (!BoardController.GetNewMills().Any())
                        {
                            NextPlayersTurn();
                        }
                        PrintMills();
                        
                    }

                    return res;
                }
                else if (s[0] == "next")
                {
                    if (_playersTurn.Equals(PlayersTurn.Player1) && _bot1 != null)
                        ActBot(_bot1);
                    else if (_playersTurn.Equals(PlayersTurn.Player2) && _bot2 != null)
                        ActBot(_bot2);
                    return true;
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
                if (!IsGameOver() && _stepByStepButton.IsChecked.Value)
                {
                    if (_playersTurn.Equals(PlayersTurn.Player1) && _bot1 != null)
                        ActBot(_bot1);
                    if (_playersTurn.Equals(PlayersTurn.Player2) && _bot2 != null)
                        ActBot(_bot2);
                }
            }

            return false;
        }


        public void ActBot(IBot bot)
        {
            if (bot == null)
                return;

            if (State == GameState.Off)
                return;

            switch (State)
            {
                case GameState.InGame:
                    var tuple = bot.MakeAMove(BoardController.Board);
                    if (BoardController.Move(tuple.Item1, tuple.Item2, _playersTurn))
                    {
                        _commandsTextBlock.Text = $"{_commandsTextBlock.Text}> {_playersTurn}: {tuple.Item1} {tuple.Item2}\n";
                        _moves++;
                        if (!BoardController.GetNewMills().Any())
                        {
                            NextPlayersTurn();
                        }
                        PrintMills();
                    }
                    break;
                case GameState.PlacingStones:
                    var c = bot.PlaceStone(BoardController.Board);
                    if (BoardController.ChangeValue(_playersTurn, c))
                    {
                        _commandsTextBlock.Text = $"{_commandsTextBlock.Text}+ {_playersTurn}: {c}\n";
                        if (!BoardController.GetNewMills().Any())
                        {
                            NextPlayersTurn();
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
                        
                    }
                    break;
                case GameState.RemovingStone:
                    var cords = bot.RemoveStone(BoardController.Board);
                    if (BoardController.RemoveEnemyStone(_playersTurn, cords))
                    {
                        _commandsTextBlock.Text = $"{_commandsTextBlock.Text}- {_playersTurn}: {cords}\n";
                        NextPlayersTurn();
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
                    }

                    break;
            }
            UpdateBoard();
            if (!IsGameOver() && _stepByStepButton.IsChecked.Value)
            {
                if (_playersTurn.Equals(PlayersTurn.Player1) && _bot1 != null)
                    ActBot(_bot1);
                if (_playersTurn.Equals(PlayersTurn.Player2) && _bot2 != null)
                    ActBot(_bot2);
            }
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

        public void NewGame(PlayerType player1, PlayerType player2)
        {
            //new MessageDialog(player1.ToString(), player2.ToString()).ShowAsync();
            GenerateBots(player1, player2);
            _stones = 0;
            _moves = 0;
            State = GameState.PlacingStones;
            BoardController.NewGame();
            _playersTurn = PlayersTurn.Player1;
            UpdateHeader();
            if (!player1.Equals(PlayerType.Human))
            {
                ActBot(_bot1);
            }
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
            _movesTextBlock.Text = $"Moves: {_moves}";
        }

        public void GenerateBots(PlayerType p1, PlayerType p2)
        {
            if (p1.Equals(PlayerType.RandomBot))
            {
                _bot1 = new RandomBot(FieldState.P1, p1);
            }
            else if(p1.Equals(PlayerType.Human))
            {
                _bot1 = null;
            }

            if (p2.Equals(PlayerType.RandomBot))
            {
                _bot2 = new RandomBot(FieldState.P2, p2);
            }
            else if (p2.Equals(PlayerType.Human))
            {
                _bot2 = null;
            }
        }

        private void NextPlayersTurn()
        {
            _playersTurn = _playersTurn == PlayersTurn.Player1
                ? PlayersTurn.Player2
                : PlayersTurn.Player1;
        }
    }
}