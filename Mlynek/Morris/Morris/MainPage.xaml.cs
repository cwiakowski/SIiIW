using System;
using System.Collections.Generic;
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
        private PlayerType player1 = PlayerType.Human;
        private PlayerType player2 = PlayerType.Human;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(800, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            controller = new GameController(Display, Moves, StateTextBlock, ref Commands, ref BtnRandom);
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
                    player2 = PlayerType.RandomBot;
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
                    player1 = PlayerType.Human;
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.StackTrace, $"Error {ex.Message}").ShowAsync();
            }
        }

        private void ComboBox_OnDropDownClosed(object sender, object o)
        {
            if (ComboBox.SelectedIndex == 0)
            {
                player1 = PlayerType.RandomBot;
            }
            else if (ComboBox.SelectedIndex == 1)
            {
                player1 = PlayerType.SimpleMinMaxBot;
            }
        }

        private void ComboBox2_OnDropDownClosed(object sender, object o)
        {
            if (ComboBox2.SelectedIndex == 0)
            {
                player2 = PlayerType.RandomBot;
            }
            else if (ComboBox2.SelectedIndex == 1)
            {
                player2 = PlayerType.SimpleMinMaxBot;
            }
        }
    }
}
