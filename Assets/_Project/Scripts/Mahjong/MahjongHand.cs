using System;
using System.Collections.Generic;

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
            int index = (int)tile;

            if (_tileCounts[index] >= 4)
            {
                throw new InvalidOperationException(
                    $"Cannot have more than 4 copies of {tile}.");
            }

            _tileCounts[index]++;
        }

        public void RemoveTile(TileType tile)
        {
            int index = (int)tile;

            if (_tileCounts[index] <= 0)
            {
                throw new InvalidOperationException(
                    $"Cannot remove {tile} because it is not in the hand.");
            }

            _tileCounts[index]--;
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
        
        public IReadOnlyList<TileType> Tiles
        {
            get
            {
                var tiles = new List<TileType>();

                for (int i = 0; i < TileTypeCount; i++)
                {
                    TileType tile = (TileType)i;

                    for (int j = 0; j < _tileCounts[i]; j++)
                    {
                        tiles.Add(tile);
                    }
                }

                return tiles;
            }
        }
    }
}