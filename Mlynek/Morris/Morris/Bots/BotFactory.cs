using System;
using Windows.UI.Xaml.Controls;
using Morris.Models;

namespace Morris.Bots
{
    public class BotFactory
    {
        public static IBot GenerateABot(BotRequest request, ref TextBlock movesTextBlock)
        {
            switch (request.PlayerType)
            {
                case PlayerType.RandomBot:
                    return new RandomBot(request.PlayersState, request.PlayerType, ref movesTextBlock);
                case PlayerType.SimpleMinMaxBot:
                    return new MinMaxBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.VeryStrongMinMaxBot:
                    return new MinMaxBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.HugeMinMaxBot:
                    return new MinMaxBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.AmazingMinMaxBot:
                    return new MinMaxBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.SimpleAlfaBetaBot:
                    return new AlfaBetaBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.VeryStrongAlfaBetaBot:
                    return new AlfaBetaBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.HugeAlfaBetaBot:
                    return new AlfaBetaBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.AmazingAlfaBetaBot:
                    return new AlfaBetaBot(request.PlayersState, request.PlayerType, ref movesTextBlock, request.MaxDepth, request.MaxTime);
                case PlayerType.Human:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.PlayerType), request.PlayerType, null);
            }

            return null;
        }
    }

    public class BotRequest
    {
        public FieldState PlayersState { get; set; }
        public PlayerType PlayerType { get; set; }
        public int MaxDepth { get; set; }
        public int MaxTime { get; set; }

        public override string ToString()
        {
            return $"{PlayerType, -12} /t{MaxTime} /d{MaxDepth}";
        }
    }
}