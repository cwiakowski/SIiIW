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
        //Library lib = new Library();
        private GameController controller;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(800, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            controller = new GameController(Display, Moves, StateTextBlock);
            //            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(1400, 1000));
        }

        private void BtnNew_OnClick(object sender, RoutedEventArgs e)
        {
            //lib.New(Display);
            Lines.Visibility = Visibility.Visible;
            Moves.Visibility = Visibility.Visible;
            controller.NewGame();
            UpdateBoard();
            Commands.Text = string.Empty;
        }

        private void Send_OnClick(object sender, RoutedEventArgs e)
        {
            if (controller.Act(CommandInput.Text))
            {
                Commands.Text = $"{Commands.Text}> {CommandInput.Text}\n";
                CommandInput.Text = string.Empty;
            }
            else
            {
                Commands.Text = $"{Commands.Text}WRONG COMMAND\n";
            }

            if (controller.State == GameState.InGame)
            {
                UpdateBoard();
            }
        }

        private void UpdateBoard()
        {
            controller.UpdateBoard();
        }


        private void CommandInput_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (controller.Act(CommandInput.Text))
                {
                    Commands.Text = $"{Commands.Text}> {CommandInput.Text}\n";
                    CommandInput.Text = string.Empty;
                }
                else
                {
                    Commands.Text = $"{Commands.Text}WRONG COMMAND\n";
                }
                if (controller.State == GameState.InGame)
                {
                    UpdateBoard();
                }
            }
        }

        private void BtnRandom_OnClick(object sender, RoutedEventArgs e)
        {
            controller.NewGame();
            
            Commands.Text = string.Empty;
            controller.GenerateExample();
            UpdateBoard();
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
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.StackTrace, $"Error {ex.Message}").ShowAsync();
            }
        }
    }
}
