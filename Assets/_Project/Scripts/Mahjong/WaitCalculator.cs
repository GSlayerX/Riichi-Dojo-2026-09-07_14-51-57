using System.Collections.Generic;

namespace RiichiDojo.Mahjong
{
    public class WaitCalculator
    {
        private const int TileTypeCount = 34;

        private readonly StandardHandSolver _standardHandSolver;
        private readonly ChiitoitsuSolver _chiitoitsuSolver;
        private readonly KokushiSolver _kokushiSolver;
        private readonly ChurenpotoSolver _churenpotoSolver;

        public WaitCalculator()
        {
            _standardHandSolver = new StandardHandSolver();
            _chiitoitsuSolver = new ChiitoitsuSolver();
            _kokushiSolver = new KokushiSolver();
            _churenpotoSolver = new ChurenpotoSolver();
        }

        public WaitResult CalculateWaits(MahjongHand hand)
        {
            if (hand.TileCount != 13)
            {
                return new WaitResult(
                    hand,
                    new List<TileType>(),
                    new List<HandPattern>());
            }
            
            var waits = new List<TileType>();
            var allPatterns = new List<HandPattern>();
            
            for (int i = 0; i < TileTypeCount; i++)
            {
                TileType candidate = (TileType)i;

                if (hand.GetCount(candidate) >= 4)
                {
                    continue;
                }

                hand.AddTile(candidate);

                var patterns = new List<HandPattern>();

                if (_standardHandSolver.IsWinningHand(hand))
                {
                    patterns.Add(HandPattern.Standard);
                }

                if (_chiitoitsuSolver.IsWinningHand(hand))
                {
                    patterns.Add(HandPattern.Chiitoitsu);
                }

                if (_kokushiSolver.IsWinningHand(hand))
                {
                    patterns.Add(HandPattern.Kokushi);
                }

                if (_churenpotoSolver.IsWinningHand(hand))
                {
                    patterns.Add(HandPattern.Churenpoto);
                }
                
                if (patterns.Count > 0)
                {
                    waits.Add(candidate);
                }

                foreach (HandPattern pattern in patterns)
                {
                    if (!allPatterns.Contains(pattern))
                    {
                        allPatterns.Add(pattern);
                    }
                }
                
                hand.RemoveTile(candidate);
            }

            return new WaitResult(
                hand,
                waits,
                allPatterns);
        }
    }
}