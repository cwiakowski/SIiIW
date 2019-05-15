using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Morris.Models;
using Morris.Services;

namespace Morris.Controllers
{
    public class BoardController
    {
        public IEnumerable<Mill> LastMills { get; set; }
        public Board Board { get; set; }
        private Grid _grid;
        private Color _p1Color = Colors.WhiteSmoke;
        private Color _p2Color = Colors.Black;
        private Color _emptyColor = (Color)Application.Current.Resources["SystemAccentColor"];

        public Brush Background { get; set; }

        public BoardController(ref Grid grid)
        {
            _grid = grid;
            Board = new Board();
            LastMills = new List<Mill>();
        }

        public void Layout()
        {
            _grid.Children.Clear();
            _grid.RowDefinitions.Clear();
            _grid.ColumnDefinitions.Clear();

            _grid.Background = Background;
            for (int i = 0; i < 7; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition());
                _grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            ColorWholeGrid();
        }

        public void ColorWholeGrid()
        {
            _grid.Children.Clear();
            foreach (var field in Board.GetFields())
            {
                ColorGridCell(field.GridRow, field.GridCol, field.State, field.Cords);
            }
        }

        private void ColorGridCell(int row, int col, FieldState state, string name)
        {
            Canvas canvas = new Canvas {Height = 40, Width = 40};
            canvas.Background = new SolidColorBrush(Colors.Transparent);
            TextBlock text = new TextBlock {Text = name, Foreground = new SolidColorBrush(Colors.Black)};
            var ellipse1 = new Ellipse
            {
                Width = 40, Height = 40
            };
            if (state == FieldState.Empty)
            {
                Rectangle rec = new Rectangle {Width = 16, Height = 16};
                Canvas.SetTop(rec, 12);
                Canvas.SetLeft(rec, 12);
                rec.Fill = new SolidColorBrush(_emptyColor);
                Rectangle rec2 = new Rectangle {Width = 18, Height = 18};
                Canvas.SetTop(rec2, 11);
                Canvas.SetLeft(rec2, 11);
                rec2.Fill = new SolidColorBrush(Colors.Black);
                canvas.Children.Add(rec2);
                canvas.Children.Add(rec);
            }
            else if (state == FieldState.P1)
            {
                ellipse1.Fill = new SolidColorBrush(_p1Color);
                canvas.Children.Add(ellipse1);
            }
            else if (state == FieldState.P2)
            {
                ellipse1.Fill = new SolidColorBrush(_p2Color);
                canvas.Children.Add(ellipse1);
                text.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            }
            canvas.SetValue(Grid.ColumnProperty, col);
            canvas.SetValue(Grid.RowProperty, row);
            Canvas.SetTop(text, 10);
            Canvas.SetLeft(text, 12);
            canvas.Children.Add(text);
            _grid.Children.Add(canvas);
        }

        public bool Move(string start, string stop, PlayersTurn turn)
        {
            var field1 = Board.Get(start);
            var field2 = Board.Get(stop);
            if (field1 == null || field2 == null)
            {
                return false;
            }

            var fieldState = turn.Equals(PlayersTurn.Player1) ? FieldState.P1 : FieldState.P2;

            if (field1.State == fieldState && field2.State == FieldState.Empty)
            {
                var move = fieldState.Equals(FieldState.P1) ? Board.LastP1Moves : Board.LastP2Moves;
                    
                if (move.Start == field2.Cords && move.Stop == field1.Cords)
                {
                    return false;
                }

                if (Board.CountPlayerFields(field1.State) == 3)
                {
                    field1.State = field2.State;
                    field2.State = fieldState;
                    Board.UpdateLastMove(field1.Cords, field2.Cords, field2.State);
                    
                    return true;
                }

                if (AvailableMoves(start).Contains(field2))
                {
                    var temp = field1.State;
                    field1.State = field2.State;
                    field2.State = temp;
                    Board.UpdateLastMove(field1.Cords, field2.Cords, field2.State);
                    return true;
                }
            }

            return false;
        }

        public bool RemoveEnemyStone(PlayersTurn playersTurn, string cords)
        {
            var field = Board.Get(cords);
            if (field == null)
            {
                return false;
            }

            var fieldState = playersTurn.Equals(PlayersTurn.Player1) ? FieldState.P2 : FieldState.P1;
            if (field.State == fieldState)
            {
                field.State = FieldState.Empty;
                return true;
            }

            return false;
        }

        public IEnumerable<Field> GetNeighbors(string cords)
        {
            return Board.GetNeighbors(cords);
        }

        public IEnumerable<Field> AvailableMoves(string cords)
        {
            return Board.GetAvailableMoves(cords);
        }


        public void NewGame()
        {
            Board.InitializeFields();
            Layout();
        }

        public IEnumerable<Mill> GetNewMills()
        {
            List<Mill> mills = new List<Mill>(Board.GetMills());
            
            return mills.Except(LastMills);
        }

        public void UpdateLastMills()
        {
            LastMills = Board.GetMills();
        }

        public bool ChangeValue(PlayersTurn turn, string cords)
        {
            var field = Board.GetFields().FirstOrDefault(x => x.Cords == cords);
            if (field == null)
            {
                return false;
            }

            if (field.State != FieldState.Empty)
            {
                return false;
            }

            field.State = (turn == PlayersTurn.Player1) ? FieldState.P1 : FieldState.P2; 
            return true;
        }


        public int CalculateAmountOfFields(PlayersTurn playersTurn)
        {
            var p = playersTurn.Equals(PlayersTurn.Player1) ? FieldState.P1 : FieldState.P2;
            return Board.GetFields().Count(x => x.State == p);
        }
    }
}