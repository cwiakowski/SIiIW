using System;
using Morris.Models;

namespace Morris.Bots
{
    public interface IBot
    {
        ScoreHolder GetBestBoard(Board board, int placedStones);
    }
}