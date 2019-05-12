using System;
using System.Collections.Generic;
using System.Linq;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    public class RandomBot : AbstractBot
    {
        private Random _random;
        private int _max;

        public RandomBot(FieldState playersState, PlayerType playerType) : base(playersState, playerType)
        {
            _random = new Random();
            _max = 100;
        }

        public override Tuple<string, string> MakeAMove(Board board)
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

        public override string PlaceStone(Board board)
        {
            var fields = board.GetFields().Where(x => x.State == FieldState.Empty).ToList();
            var rand = _random.Next(_max) % fields.Count();
            return fields[rand].Cords;
        }

        public override string RemoveStone(Board board)
        {
            FieldState enemyState = PlayersState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
            var fields = board.GetFields().Where(x => x.State == enemyState).ToList();
            var rand = _random.Next(_max) % fields.Count();
            return fields[rand].Cords;
        }
    }
}