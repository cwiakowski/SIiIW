using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public IEnumerable<Mill> Mills { get; set; }
        private Board _board;
        private Grid _grid;
        private Color _p1Color = Colors.Blue;
        private Color _p2Color = Colors.Orange;
        private Color _emptyColor = Colors.White;

        public Brush Background { get; set; }

        public BoardController(ref Grid grid)
        {
            _grid = grid;
            _board = new Board();
            Mills = new List<Mill>();
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
            foreach (var field in _board.GetFields())
            {
                ColorGridCell(field.GridRow, field.GridCol, field.State, field.Cords);
            }
        }

        private void ColorGridCell(int row, int col, FieldState state, string name)
        {
            Canvas canvas = new Canvas {Height = 40, Width = 40};
            TextBlock text = new TextBlock {Text = name, Foreground = new SolidColorBrush(Colors.Black)};
//            var ellipse1 = new Ellipse();
//            ellipse1.Fill = new SolidColorBrush(Windows.UI.Colors.SteelBlue);
//            ellipse1.Width = 38;
//            ellipse1.Height = 38;
            if (state == FieldState.Empty)
            {
                canvas.Background = new SolidColorBrush(_emptyColor);
            }
            else if (state == FieldState.P1)
            {
                canvas.Background = new SolidColorBrush(_p1Color);
            }
            else if (state == FieldState.P2)
            {
                canvas.Background = new SolidColorBrush(_p2Color);
            }
            //canvas.Children.Add(ellipse1);
            canvas.SetValue(Grid.ColumnProperty, col);
            canvas.SetValue(Grid.RowProperty, row);
            canvas.Children.Add(text);
            _grid.Children.Add(canvas);
        }

        public bool Move(string start, string stop)
        {
            var field1 = _board.Get(start);
            var field2 = _board.Get(stop);
            if (field1 == null || field2 == null)
            {
                return false;
            }

            if (_board.CountPlayerFields(field1.State) == 3)
            {
                if (field2.State == FieldState.Empty)
                {
                    var temp = field1.State;
                    field1.State = field2.State;
                    field2.State = temp;
                    Mills = _board.GetMills();
                    return true;
                }
            }

            if (AvailableMoves(start).Contains(field2))
            {
                var temp = field1.State;
                field1.State = field2.State;
                field2.State = temp;
                Mills = _board.GetMills();
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
            var list = _board.GetNeighbors(cords);
            var field = _board.Get(cords);
            return list.Where(x => x.State == FieldState.Empty);
        }


        public void NewGame()
        {
            _board.InitializeFields();
            Layout();
        }

        public bool IsMill()
        {
            Mills = _board.GetMills();
            if (Mills.Any())
            {
                return false;
            }
            return true;
        }

        public void ChangeValue(int state, int list, int ind)
        {
            _board.UpdateField(state, list, ind);
            Mills = _board.GetMills();
        }

        public void ChangeValue(int state, string cords)
        {
            _board.UpdateField(state, cords);
            Mills = _board.GetMills();
        }

        public string GameState()
        {
            return _board.ToString();
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
    }
}