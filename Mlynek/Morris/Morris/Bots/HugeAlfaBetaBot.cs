using System.Linq;
using Windows.UI.Xaml.Controls;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    public class HugeAlfaBetaBot : SimpleAlfaBetaBot
    {
        public HugeAlfaBetaBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock) : base(playersState, playerType, ref movesTextBlock)
        {
            MaxDepth = 3;
        }

        public HugeAlfaBetaBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock, int maxDepth, int maxTime) : base(playersState, playerType, ref movesTextBlock, maxDepth, maxTime)
        {

        }

        public override double CalculateBoardState(Board board, int placedStones)
        {
            if (board.IsGameOver(placedStones, _enemyState))
            {
                return double.MaxValue;
            }

            double score = board.GetMills().Count(mill => mill.Field1.State.Equals(PlayersState)) * 3 -
                           board.GetMills().Count(mill => mill.Field1.State.Equals(_enemyState)) * 3;
            score += board.GetDoubles(PlayersState) * 2 -
                     board.GetDoubles(_enemyState) * 2;
            score += board.GetFields().Count(x => x.State.Equals(PlayersState)) * 9 -
                     board.GetFields().Count(x => x.State.Equals(_enemyState)) * 6;

            return score;
        }
    }
}