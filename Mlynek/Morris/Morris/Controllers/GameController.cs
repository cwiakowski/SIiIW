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

            if (_playersTurn.Equals(PlayersTurn.Player1) && _bot1 != null)
                return false;

            if (_playersTurn.Equals(PlayersTurn.Player2) && _bot2 != null)
                return false;

            BoardController.UpdateLastMills();

            try
            {
                var s = input.Split(' ');
                if (s[0] == "m")
                {
                    new MessageDialog(Neigh(s[1]), $"{s[1]} Neighbors").ShowAsync();
                    return true;
                }
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
                        else
                        {
                            State = GameState.RemovingStone;
                        }
                        
                    }

                    return res;
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
                if (_stepByStepButton.IsChecked.Value)
                {
                    ActBot();
                }
            }

            return false;
        }

        public void ActBot()
        {
            if (!IsGameOver())
            {
                if (_playersTurn.Equals(PlayersTurn.Player1) && _bot1 != null)
                    ActBot(_bot1);
                else if (_playersTurn.Equals(PlayersTurn.Player2) && _bot2 != null)
                    ActBot(_bot2);
            }
        }


        public void ActBot(IBot bot)
        {
            if (bot == null)
                return;

            if (State == GameState.Off)
                return;
            var data = bot.GetBestBoard(BoardController.Board, _stones);
            BoardController.Board = data.Board;
            _commandsTextBlock.Text = $"{_commandsTextBlock.Text}{_playersTurn}: {data.Decision}\n";
            PrintMills();
            _moves++;
            _stones++;
            if (_stones >= 18)
            {
                State = GameState.InGame;
            }
            else
            {
                State = GameState.PlacingStones;                
            }
            NextPlayersTurn();
            if (IsGameOver())
            {
                _headerTextBlock.Text = "Mlynek v3.2.5";
                State = GameState.Off;
                new MessageDialog($"{_playersTurn} IS A LOSER", "GAME OVER").ShowAsync();
            }
            UpdateBoard();
            if (_stepByStepButton.IsChecked.Value)
            {
                ActBot();
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



        public void UpdateBoard()
        {
            BoardController.ColorWholeGrid();
            UpdateHeader();
            _movesTextBlock.Text = $"Moves: {_moves}";
        }

        public void GenerateBots(PlayerType p1, PlayerType p2)
        {
            _bot1 = BotFactory.GenerateABot(FieldState.P1, p1, ref _movesTextBlock);
            _bot2 = BotFactory.GenerateABot(FieldState.P2, p2, ref _movesTextBlock);
        }

        private void NextPlayersTurn()
        {
            _playersTurn = _playersTurn == PlayersTurn.Player1
                ? PlayersTurn.Player2
                : PlayersTurn.Player1;
        }
    }
}