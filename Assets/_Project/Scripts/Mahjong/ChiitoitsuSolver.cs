namespace RiichiDojo.Mahjong
{
    public class ChiitoitsuSolver
    {
        private const int TileTypeCount = 34;

        public bool IsWinningHand(MahjongHand hand)
        {
            if (hand.TileCount != 14)
            {
                return false;
            }

            int pairCount = 0;

            for (int i = 0; i < TileTypeCount; i++)
            {
                int count = hand.GetCount((TileType)i);

                if (count == 2)
                {
                    pairCount++;
                }
                else if (count != 0)
                {
                    return false;
                }
            }

            return pairCount == 7;
        }
    }
}