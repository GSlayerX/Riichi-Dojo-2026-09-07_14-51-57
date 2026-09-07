using System.Collections.Generic;

namespace RiichiDojo.Mahjong
{
    public class WaitResult
    {
        public MahjongHand Hand { get; }
        public IReadOnlyList<TileType> Waits { get; }
        public IReadOnlyList<HandPattern> Patterns { get; }

        public WaitResult(
            MahjongHand hand,
            IReadOnlyList<TileType> waits,
            IReadOnlyList<HandPattern> patterns)
        {
            Hand = hand;
            Waits = waits;
            Patterns = patterns;
        }
    }
}