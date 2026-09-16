namespace RiichiDojo.Mahjong
{
    public class ChurenpotoSolver
    {
        public bool IsWinningHand(MahjongHand hand)
        {
            if (hand.TileCount != 14)
            {
                return false;
            }

            int suit = -1;

            foreach (TileType tile in hand.Tiles)
            {
                int index = (int)tile;

                if (index >= 27)
                {
                    return false;
                }

                int tileSuit = index / 9;

                if (suit == -1)
                {
                    suit = tileSuit;
                }
                else if (tileSuit != suit)
                {
                    return false;
                }
            }

            int first = suit * 9;

            int[] requiredCounts =
            {
                3, 1, 1, 1, 1, 1, 1, 1, 3
            };

            for (int rank = 0; rank < 9; rank++)
            {
                TileType tile =
                    (TileType)(first + rank);

                if (hand.GetCount(tile) < requiredCounts[rank])
                {
                    return false;
                }
            }

            return true;
        }
    }
}