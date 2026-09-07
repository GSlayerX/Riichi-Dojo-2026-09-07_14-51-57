using System.Collections.Generic;

namespace RiichiDojo.Mahjong
{
    public class WaitCalculator
    {
        private const int TileTypeCount = 34;

        private readonly StandardHandSolver _standardHandSolver;
        private readonly ChiitoitsuSolver _chiitoitsuSolver;
        private readonly KokushiSolver _kokushiSolver;

        public WaitCalculator()
        {
            _standardHandSolver = new StandardHandSolver();
            _chiitoitsuSolver = new ChiitoitsuSolver();
            _kokushiSolver = new KokushiSolver();
        }

        public WaitResult CalculateWaits(MahjongHand hand)
        {
            if (hand.TileCount != 13)
            {
                return new WaitResult(
                    hand,
                    new List<TileType>());
            }
            
            var waits = new List<TileType>();

            for (int i = 0; i < TileTypeCount; i++)
            {
                TileType candidate = (TileType)i;

                if (hand.GetCount(candidate) >= 4)
                {
                    continue;
                }

                hand.AddTile(candidate);

                if (_standardHandSolver.IsWinningHand(hand)
                    || _chiitoitsuSolver.IsWinningHand(hand)
                    || _kokushiSolver.IsWinningHand(hand))
                {
                    waits.Add(candidate);
                }

                hand.RemoveTile(candidate);
            }

            return new WaitResult(hand, waits);
        }
    }
}