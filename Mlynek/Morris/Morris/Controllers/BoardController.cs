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
        private Board _board;
        private Grid _grid;
        private Color _p1Color = Colors.WhiteSmoke;
        private Color _p2Color = Colors.Black;
        private Color _emptyColor = (Color)Application.Current.Resources["SystemAccentColor"];

        public Brush Background { get; set; }

        public BoardController(ref Grid grid)
        {
            _grid = grid;
            _board = new Board();
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
            foreach (var field in _board.GetFields())
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
            var field1 = _board.Get(start);
            var field2 = _board.Get(stop);
            if (field1 == null || field2 == null)
            {
                return false;
            }

            var fieldState = turn.Equals(PlayersTurn.Player1) ? FieldState.P1 : FieldState.P2;

            if (field1.State == fieldState && field2.State == FieldState.Empty)
            {
                if (_board.CountPlayerFields(field1.State) == 3)
                {
                    field1.State = field2.State;
                    field2.State = fieldState;
                    return true;
                }

                if (AvailableMoves(start).Contains(field2))
                {
                    var temp = field1.State;
                    field1.State = field2.State;
                    field2.State = temp;                    
                    return true;
                }
            }

            return false;
        }

        public bool RemoveEnemyStone(PlayersTurn playersTurn, string cords)
        {
            var field = _board.Get(cords);
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
            return _board.GetNeighbors(cords);
        }

        public IEnumerable<Field> AvailableMoves(string cords)
        {
            var field = _board.Get(cords);
            if (field != null)
            {
                if (_board.GetFields().Count(f => f.State == field.State) == 3)
                    return _board.GetFields().Where(f => f.State == FieldState.Empty);
            }
            var list = _board.GetNeighbors(cords);
            return list.Where(x => x.State == FieldState.Empty);
        }


        public void NewGame()
        {
            _board.InitializeFields();
            Layout();
        }

        public IEnumerable<Mill> GetNewMills()
        {
            List<Mill> mills = new List<Mill>(_board.GetMills());
            
            return mills.Except(LastMills);
        }

        public void UpdateLastMills()
        {
            LastMills = _board.GetMills();
        }

        public void ChangeValue(int state, string cords)
        {
            _board.UpdateField(state, cords);
        }

        public bool ChangeValue(PlayersTurn turn, string cords)
        {
            var field = _board.GetFields().FirstOrDefault(x => x.Cords == cords);
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

        public void GenerateExample()
        {
            _board.OuterFields[0].State = FieldState.P1;
            _board.OuterFields[2].State = FieldState.P1;
            _board.OuterFields[3].State = FieldState.P1;
            _board.OuterFields[6].State = FieldState.P1;
            _board.InnerFields[7].State = FieldState.P1;
            _board.InnerFields[0].State = FieldState.P1;
            _board.InnerFields[1].State = FieldState.P1;
            _board.MiddleFields[5].State = FieldState.P1;
            _board.MiddleFields[2].State = FieldState.P1;

            _board.OuterFields[1].State = FieldState.P2;
            _board.OuterFields[4].State = FieldState.P2;
            _board.OuterFields[5].State = FieldState.P2;
            _board.MiddleFields[1].State = FieldState.P2;
            _board.MiddleFields[0].State = FieldState.P2;
            _board.MiddleFields[4].State = FieldState.P2;
            _board.MiddleFields[6].State = FieldState.P2;
            _board.InnerFields[2].State = FieldState.P2;
            _board.InnerFields[3].State = FieldState.P2;
        }


        public int CalculateAmountOfFields(PlayersTurn playersTurn)
        {
            var p = playersTurn.Equals(PlayersTurn.Player1) ? FieldState.P1 : FieldState.P2;
            return _board.GetFields().Count(x => x.State == p);
        }
    }
}