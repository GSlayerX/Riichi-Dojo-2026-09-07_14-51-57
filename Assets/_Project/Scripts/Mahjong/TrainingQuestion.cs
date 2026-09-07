using System.Collections.Generic;

namespace RiichiDojo.Mahjong
{
    public class TrainingQuestion
    {
        public MahjongHand Hand { get; }
        public IReadOnlyList<TileType> Waits { get; }

        public TrainingQuestion(
            MahjongHand hand,
            IReadOnlyList<TileType> waits)
        {
            Hand = hand;
            Waits = waits;
        }
    }
}