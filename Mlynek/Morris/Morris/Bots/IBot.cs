using System;
using Morris.Models;

namespace Morris.Bots
{
    public interface IBot
    {
        Tuple<string, string> MakeAMove(Board board);
        string PlaceStone(Board board);
        string RemoveStone(Board board);
    }
}