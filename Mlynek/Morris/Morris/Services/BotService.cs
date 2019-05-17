using System.Linq;
using Morris.Models;

namespace Morris.Services
{
    public class BotService
    {
        public static double CalculateBoardState(PlayerType playerType, Board board, int placedStones, FieldState fieldState, int level)
        {
            switch (playerType)
            {
                case PlayerType.AmazingAlfaBetaBot:
                    return AmazingHeury(board, placedStones, fieldState, level);
                case PlayerType.AmazingMinMaxBot:
                    return AmazingHeury(board, placedStones, fieldState, level);
                case PlayerType.SimpleMinMaxBot:
                    return SimpleHeury(board, placedStones, fieldState, level);
                case PlayerType.SimpleAlfaBetaBot:
                    return SimpleHeury(board, placedStones, fieldState, level);
                case PlayerType.VeryStrongAlfaBetaBot:
                    return VeryStrongHeury(board, placedStones, fieldState, level);
                case PlayerType.VeryStrongMinMaxBot:
                    return VeryStrongHeury(board, placedStones, fieldState, level);
                case PlayerType.HugeMinMaxBot:
                    return HugeHeury(board, placedStones, fieldState, level);
                case PlayerType.HugeAlfaBetaBot:
                    return HugeHeury(board, placedStones, fieldState, level);

            }

            return 0;
        }

        public static double AmazingHeury(Board board, int placedStones, FieldState fieldState, int level)
        {
            if (board.IsGameOver(placedStones, fieldState))
            {
                return double.MinValue/level;
            }

            var enemy = fieldState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
            if (board.IsGameOver(placedStones, enemy))
            {
                return double.MinValue/level;
            }
            var blockedMyStones1 = board.GetFields().Where(x => x.State.Equals(fieldState) && board.GetAvailableMoves(x.Cords).Count() == 0).Count();
            var blockedEnemyStones1 = board.GetFields().Where(x => x.State.Equals(enemy) && board.GetAvailableMoves(x.Cords).Count() == 0).Count();
            var mills = board.GetMills();
            var myMills = mills.Where(m => m.Field1.State == fieldState).Count();
            var enemyMills = mills.Where(m => m.Field1.State == enemy).Count();
            var myDoubles = board.GetDoubles(fieldState);
            var enemyDoubles = board.GetDoubles(enemy);
            return blockedEnemyStones1 * 3 + myMills * 10 - blockedMyStones1 * 2 - enemyMills * 20 + myDoubles*2 - enemyDoubles*2;
        }

        public static double VeryStrongHeury(Board board, int placedStones,FieldState fieldState, int level)
        {
            if (board.IsGameOver(placedStones, fieldState))
            {
                return double.MinValue / level;
            }

            var enemy = fieldState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
            if (board.IsGameOver(placedStones, enemy))
            {
                return double.MinValue / level;
            }
            var ans = 0;
            var blockedMyStones = board.GetFields().Where(x => x.State.Equals(fieldState) && board.GetAvailableMoves(x.Cords).Count() == 0).Count();
            var blockedEnemyStones = board.GetFields().Where(x => x.State.Equals(enemy) && board.GetAvailableMoves(x.Cords).Count() == 0).Count();
            return blockedEnemyStones * 3 - blockedMyStones * 2;
        }

        public static double HugeHeury(Board board, int placedStones, FieldState fieldState, int level)
        {
            if (board.IsGameOver(placedStones, fieldState))
            {
                return double.MinValue / level;
            }

            var enemy = fieldState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
            if (board.IsGameOver(placedStones, enemy))
            {
                return double.MinValue / level;
            }
            var blockedMyStones1 = board.GetFields().Where(x => x.State.Equals(fieldState) && board.GetAvailableMoves(x.Cords).Count() == 0).Count();
            var blockedEnemyStones1 = board.GetFields().Where(x => x.State.Equals(enemy) && board.GetAvailableMoves(x.Cords).Count() == 0).Count();
            var mills = board.GetMills();
            var myMills = mills.Where(m => m.Field1.State == fieldState).Count();
            var enemyMills = mills.Where(m => m.Field1.State == enemy).Count();
            return blockedEnemyStones1 * 3 + myMills * 10 - blockedMyStones1 * 2 - enemyMills * 20;
        }

        public static double SimpleHeury(Board board, int placedStones, FieldState fieldState, int level)
        {
            if (board.IsGameOver(placedStones, fieldState))
            {
                return double.MinValue / level;
            }

            var enemy = fieldState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
            if (board.IsGameOver(placedStones, enemy))
            {
                return double.MinValue / level;
            }
            return board.CountPlayerFields(fieldState) * 3 - board.CountPlayerFields(enemy);
        }
    }
}