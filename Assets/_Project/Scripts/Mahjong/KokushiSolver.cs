namespace RiichiDojo.Mahjong
{
    public class KokushiSolver
    {
        private const int TileTypeCount = 34;

        public bool IsWinningHand(MahjongHand hand)
        {
            if (hand.TileCount != 14)
            {
                return false;
            }

            int uniqueTerminalHonorCount = 0;
            bool hasPair = false;

            for (int i = 0; i < TileTypeCount; i++)
            {
                int count = hand.GetCount((TileType)i);

                if (!IsTerminalOrHonor((TileType)i))
                {
                    if (count > 0)
                    {
                        return false;
                    }

                    continue;
                }

                if (count == 1)
                {
                    uniqueTerminalHonorCount++;
                }
                else if (count == 2)
                {
                    uniqueTerminalHonorCount++;
                    hasPair = true;
                }
                else if (count > 2)
                {
                    return false;
                }
            }

            return uniqueTerminalHonorCount == 13 && hasPair;
        }

        private bool IsTerminalOrHonor(TileType tile)
        {
            if ((int)tile >= 27)
            {
                return true;
            }

            int rank = (int)tile % 9;

            return rank == 0 || rank == 8;
        }
    }
}