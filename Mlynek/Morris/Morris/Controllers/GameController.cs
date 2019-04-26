using System;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Morris.Models;

namespace Morris.Controllers
{
    public class GameController
    {
        public BoardController BoardController { get; set; }
        public GameState State { get; set; }
        private int _moves = 0;
        private TextBlock _movesTextBlock;
        private bool _won = false;

        public GameController(Grid grid, TextBlock textBlock)
        {
            _movesTextBlock = textBlock;
            State = GameState.Off;
            BoardController = new BoardController(ref grid);
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
                if (s[0] == "move")
                {
                    _moves++;
                    return BoardController.Move(s[1], s[2]);
                }
                else if (s[0] == "add")
                {
                    BoardController.ChangeValue(int.Parse(s[1]), s[2]);
                    return true;
                }
                else if (s[0] == "neigh")
                {
                    new MessageDialog(Neigh(s[1]), $"{s[1]} Neighbors").ShowAsync();
                    return true;
                }
                else if (s[0] == "test")
                {
                    new MessageDialog($"{Math.Abs(-1)%8}", $"TEST").ShowAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                new MessageDialog(e.StackTrace, $"Error {e.Message}").ShowAsync();
                return false;
            }

            return false;
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
            State = GameState.On;
            BoardController.NewGame();
        }

        public string BoardStatus()
        {
            return BoardController.GameState();
        }

        public void GenerateExample()
        {
            BoardController.GenerateExample();
        }

        public void UpdateBoard()
        {
            _movesTextBlock.Text = $"Moves: {_moves / 2}";
            BoardController.ColorWholeGrid();
        }
    }
}