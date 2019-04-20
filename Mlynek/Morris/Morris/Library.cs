using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Morris
{
    public class Library
    {
        private const int Size = 7;
        private const int On = 1;
        private const int Off = 0;
        private int _moves = 0;
        private bool _won = false;
        private int [,] _board = new int[Size, Size];
        private Color _p1Color = Colors.Blue;
        private Color _p2Color = Colors.Orange;
        private Color _emptyColor = Colors.White;

        public Brush Background { get; set; }

        public void Show(string content, string title)
        {
            IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
        }

        private bool Winner()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (_board[col, row] == On)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Toggle(Grid grid, int row, int column)
        {
            _board[row, column] = (_board[row, column] == On ? Off : On);
            Canvas canvas = (Canvas) grid.Children.Single(w =>
                Grid.GetRow((Canvas) w) == row && Grid.GetColumn((Canvas) w) == column);
            canvas.Background = _board[row, column] == On
                ? new SolidColorBrush(_p1Color)
                : new SolidColorBrush(_p2Color);
        }

        private void Add(Grid grid, int row, int col)
        {
            Canvas canvas = new Canvas();
            canvas.Height = 40;
            canvas.Width = 40;
            canvas.Background = new SolidColorBrush(_p1Color);
            canvas.Tapped += (object sender, TappedRoutedEventArgs e) =>
            {
                if (!_won)
                {
                    canvas = (Canvas) sender;
                    row = (int) canvas.GetValue(Grid.RowProperty);
                    col = (int) canvas.GetValue(Grid.ColumnProperty);
                    Toggle(grid, row, col);
                    if (0 < row)
                        Toggle(grid, row-1, col);

                    if (row < Size-1)
                        Toggle(grid, row+1, col);

                    if (0 < col)
                        Toggle(grid, row, col-1);

                    if (col < Size - 1)
                        Toggle(grid, row, col+1);

                    _moves++;
                    if (Winner())
                    {
                        Show($"WELL DONE! You won in {_moves} moves", "Siema eniu");
                        _won = true;
                    }
                }
            };
            canvas.SetValue(Grid.ColumnProperty, col);
            canvas.SetValue(Grid.RowProperty, row);
            grid.Children.Add(canvas);
        }

        private void Layout(ref Grid grid)
        {
            _moves = 0;
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            grid.Background = Background;
            for (int i = 0; i < Size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Add(grid, row , col);
                }
            }
        }

        public void New(Grid grid)
        {
            Layout(ref grid);
            _won = false;
            for (int col = 0; col < Size; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    _board[col, row] = On;
                }
            }
        }

    }
}