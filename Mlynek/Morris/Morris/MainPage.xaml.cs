using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Morris.Bots;
using Morris.Controllers;
using Morris.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Morris
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GameController controller;
        private BotRequest player1 = new BotRequest(){ PlayerType = PlayerType.Human, PlayersState = FieldState.P1};
        private BotRequest player2 = new BotRequest() { PlayerType = PlayerType.Human, PlayersState = FieldState.P2 };
        private Tuple<ObservableCollection<BotRequest>, ObservableCollection<BotRequest>> _requests = new Tuple<ObservableCollection<BotRequest>, ObservableCollection<BotRequest>>(new ObservableCollection<BotRequest>(), new ObservableCollection<BotRequest>());

        public MainPage()
        {
            this.InitializeComponent();
            GenerateRequests();
            ApplicationView.PreferredLaunchViewSize = new Size(800, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            controller = new GameController(Display, Moves, StateTextBlock, ref Commands, ref BtnRandom);
        }

        private void GenerateRequests()
        {
            var times = new List<int>() {0, 2, 5, 10, 15, 20};
            var depths = new List<int>() { 1, 2, 3, 5, 6, 7};
            var bots = new List<PlayerType>() {PlayerType.SimpleMinMaxBot, PlayerType.VeryStrongMinMaxBot};
            _requests.Item1.Add(new BotRequest() { PlayerType = PlayerType.RandomBot, PlayersState = FieldState.P1});
            _requests.Item2.Add(new BotRequest() { PlayerType = PlayerType.RandomBot, PlayersState = FieldState.P2});
            foreach (var pt in bots)
            {
                foreach (var d in depths)
                {
                    if (d >= 5)
                    {
                        foreach (var t in times)
                        {
                            _requests.Item1.Add(new BotRequest() { PlayerType = pt, PlayersState = FieldState.P1, MaxDepth = d, MaxTime = t });
                            _requests.Item2.Add(new BotRequest() { PlayerType = pt, PlayersState = FieldState.P2, MaxDepth = d, MaxTime = t });
                        }
                    }
                    else
                    {
                        _requests.Item1.Add(new BotRequest() { PlayerType = pt, PlayersState = FieldState.P1, MaxDepth = d, MaxTime = 0 });
                        _requests.Item2.Add(new BotRequest() { PlayerType = pt, PlayersState = FieldState.P2, MaxDepth = d, MaxTime = 0 });
                    }
                }
            }
        }

        private void BtnNew_OnClick(object sender, RoutedEventArgs e)
        {
            Lines.Visibility = Visibility.Visible;
            Moves.Visibility = Visibility.Visible;
            Commands.Text = string.Empty;
            controller.NewGame(player1, player2);
        }

        private void Send_OnClick(object sender, RoutedEventArgs e)
        {
            if (controller.Act(CommandInput.Text.TrimEnd()))
            {
                CommandInput.Text = string.Empty;
            }
            else
            {
                Commands.Text = $"{Commands.Text}WRONG COMMAND\n";
            }
        }

        private void CommandInput_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (controller.Act(CommandInput.Text))
                {
                    CommandInput.Text = string.Empty;
                }
                else
                {
                    Commands.Text = $"{Commands.Text}WRONG COMMAND\n";
                }
            }

            if (e.Key == VirtualKey.N)
            {
                controller.ActBot();
            }
        }

        private void CommandInput_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.N)
            {
                CommandInput.Text = string.Empty;
            }
        }

        private  void P2AiToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (P2AiToggleButton.IsChecked.Value)
                {
                    ComboBox2.Visibility = Visibility.Visible;
                }
                else
                {
                    ComboBox2.Visibility = Visibility.Collapsed;
                    player2.PlayerType = PlayerType.Human;
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.StackTrace, $"Error {ex.Message}").ShowAsync();
            }
        }

        private void P1AiToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (P1AiToggleButton.IsChecked.Value)
                {
                    ComboBox.Visibility = Visibility.Visible;
                }
                else
                {
                    ComboBox.Visibility = Visibility.Collapsed;
                    player1.PlayerType = PlayerType.Human;
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.StackTrace, $"Error {ex.Message}").ShowAsync();
            }
        }

        private void ComboBox_OnDropDownClosed(object sender, object o)
        {
            if (ComboBox.SelectedIndex <= _requests.Item1.Count)
            {
                player1 = _requests.Item1[ComboBox.SelectedIndex];
            }
            else
            {
                player1.PlayerType = PlayerType.Human;
            }
            
        }

        private void ComboBox2_OnDropDownClosed(object sender, object o)
        {
            if (ComboBox2.SelectedIndex <= _requests.Item2.Count)
            {
                player2 = _requests.Item2[ComboBox2.SelectedIndex];
            }
            else
            {
                player2.PlayerType = PlayerType.Human;
            }
        }
    }
}
