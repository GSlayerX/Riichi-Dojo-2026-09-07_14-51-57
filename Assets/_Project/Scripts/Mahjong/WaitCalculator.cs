using System.Collections.Generic;

namespace RiichiDojo.Mahjong
{
    public class WaitCalculator
    {
        private const int TileTypeCount = 34;

        private readonly StandardHandSolver _standardHandSolver;

        public WaitCalculator()
        {
            _standardHandSolver = new StandardHandSolver();
        }

        public IReadOnlyList<TileType> CalculateWaits(MahjongHand hand)
        {
            var waits = new List<TileType>();

            for (int i = 0; i < TileTypeCount; i++)
            {
                TileType candidate = (TileType)i;

                if (hand.GetCount(candidate) >= 4)
                {
                    continue;
                }

                hand.AddTile(candidate);

                if (_standardHandSolver.IsWinningHand(hand))
                {
                    waits.Add(candidate);
                }

                hand.RemoveTile(candidate);
            }

            return waits;
        }
    }
}