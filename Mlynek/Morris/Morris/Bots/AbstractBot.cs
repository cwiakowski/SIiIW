using System;
using System.Linq;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    public abstract class AbstractBot : IBot
    {
        public PlayerType PlayerType { get; private set; }
        public FieldState PlayersState { get; private set; }

        protected AbstractBot(FieldState playersState, PlayerType playerType)
        {
            PlayersState = playersState;
            PlayerType = playerType;
        }

        public virtual double CalculateBoardState(Board board)
        {
            return board.GetFields().Count(x => x.State.Equals(PlayersState));
        }

        public abstract Tuple<string, string> MakeAMove(Board board);

        public abstract string PlaceStone(Board board);

        public abstract string RemoveStone(Board board);

    }
}