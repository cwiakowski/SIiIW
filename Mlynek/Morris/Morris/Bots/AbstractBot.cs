using System;
using System.Linq;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml.Controls;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    public abstract class AbstractBot : IBot, IDisposable
    {
        public PlayerType PlayerType { get; private set; }
        public FieldState PlayersState { get; private set; }
        protected readonly FieldState _enemyState;
        public TextBlock MovesTextBlock { get; set; }

        protected AbstractBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock)
        {
            PlayersState = playersState;
            PlayerType = playerType;
            MovesTextBlock = movesTextBlock;
            _enemyState = playersState == FieldState.P1 ? FieldState.P2 : FieldState.P1;
        }

        public virtual double CalculateBoardState(Board board, int placedStones)
        {
            return board.GetFields().Count(x => x.State.Equals(PlayersState));
        }
        public abstract ScoreHolder GetBestBoard(Board board, int placedStones);
        public abstract void Dispose();
    }
}