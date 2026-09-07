namespace RiichiDojo.Mahjong
{
    public class MahjongHand
    {
        private const int TileTypeCount = 34;

        private readonly int[] _tileCounts = new int[TileTypeCount];

        public int GetCount(TileType tile)
        {
            return _tileCounts[(int)tile];
        }

        public void AddTile(TileType tile)
        {
            _tileCounts[(int)tile]++;
        }

        public void RemoveTile(TileType tile)
        {
            _tileCounts[(int)tile]--;
        }

        public int TileCount
        {
            get
            {
                int total = 0;

                for (int i = 0; i < TileTypeCount; i++)
                {
                    total += _tileCounts[i];
                }

                return total;
            }
        }
    }
}