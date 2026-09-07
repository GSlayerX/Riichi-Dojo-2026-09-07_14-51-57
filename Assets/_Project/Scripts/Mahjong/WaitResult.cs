using System.Collections.Generic;

namespace RiichiDojo.Mahjong
{
    public class WaitResult
    {
        public MahjongHand Hand { get; }
        public IReadOnlyList<TileType> Waits { get; }

        public WaitResult(
            MahjongHand hand,
            IReadOnlyList<TileType> waits)
        {
            Hand = hand;
            Waits = waits;
        }
    }
}