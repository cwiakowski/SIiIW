using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    public class RandomBot : AbstractBot
    {
        private Random _random;
        private int _max;

        public RandomBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock) : base(playersState, playerType, ref movesTextBlock)
        {
            _random = new Random();
            _max = 100;
        }

        public Tuple<string, string> MakeAMove(Board board)
        {
            var availableMoves = new List<Field>();
            var fields = board.GetFields().Where(x => x.State == PlayersState).ToList();
            int rand = 0;
            while (!availableMoves.Any())
            {
                rand = _random.Next(_max) % fields.Count();
                availableMoves = board.GetAvailableMoves(fields[rand%fields.Count].Cords).ToList();
            }
            return new Tuple<string, string>(fields[rand % fields.Count].Cords, availableMoves[rand % availableMoves.Count].Cords);
        }

        public string PlaceStone(Board board)
        {
            var fields = board.GetFields().Where(x => x.State == FieldState.Empty).ToList();
            var rand = _random.Next(_max) % fields.Count();
            return fields[rand].Cords;
        }

        public string RemoveStone(Board board)
        {
            FieldState enemyState = PlayersState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
            var fields = board.GetFields().Where(x => x.State == enemyState).ToList();
            var rand = _random.Next(_max) % fields.Count();
            return fields[rand].Cords;
        }

        public override ScoreHolder GetBestBoard(Board board, int placedStones)
        {
            if (18 <= placedStones)
            {
                var move = MakeAMove(board);
                var b = board.Copy();
                var mills = b.GetMills().ToList();
                var field1 = b.Get(move.Item1);
                var field2 = b.Get(move.Item2);
                var temp = field1.State;
                field1.State = field2.State;
                field2.State = temp;
                string rs = string.Empty;
                if (b.GetMills().Except(mills).Any())
                {
                    rs = RemoveStone(b);
                    b.Get(rs).State = FieldState.Empty;
                }

                var sh = new ScoreHolder() {Board = b, Decision = $"{move.Item1} {move.Item2}"};
                if (rs != string.Empty)
                {
                    sh.Decision = $"{sh.Decision} -{rs}";
                }
                return sh;
            }
            else
            {
//                placedStones++;
                var stone = PlaceStone(board);
                var b = board.Copy();
                b.Get(stone).State = PlayersState;
                var mills = b.GetMills().ToList();
                string rs = string.Empty;
                if (b.GetMills().Except(mills).Any())
                {
                    b.Get(RemoveStone(b)).State = FieldState.Empty;
                    b.Get(rs).State = FieldState.Empty;
                }
                var sh = new ScoreHolder() { Board = b, Decision = $"{stone}" };
                if (rs != string.Empty)
                {
                    sh.Decision = $"{sh.Decision} -{rs}";
                }
                return sh;
            }
        }
    }
}