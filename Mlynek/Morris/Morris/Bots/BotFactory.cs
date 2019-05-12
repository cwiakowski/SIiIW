using System;
using Windows.UI.Xaml.Controls;
using Morris.Models;

namespace Morris.Bots
{
    public class BotFactory
    {
        public static IBot GenerateABot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock)
        {
            switch (playerType)
            {
                case PlayerType.RandomBot:
                    return new RandomBot(playersState, playerType, ref movesTextBlock);
                case PlayerType.SimpleMinMaxBot:
                    return new SimpleMinMaxBot(playersState, playerType, ref movesTextBlock);
                case PlayerType.Human:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
            }

            return null;
        }
    }
}