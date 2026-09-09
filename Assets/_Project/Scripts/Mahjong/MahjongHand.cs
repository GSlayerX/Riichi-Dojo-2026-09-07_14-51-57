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
        
        public IReadOnlyList<TileType> GetAvailableSelectionTiles()
        {
            var tiles = new List<TileType>();

            bool hasMan = false;
            bool hasPin = false;
            bool hasSou = false;
            bool hasHonors = false;

            for (int i = 0; i < TileTypeCount; i++)
            {
                if (_tileCounts[i] <= 0)
                {
                    continue;
                }

                TileType tile = (TileType)i;

                if (tile >= TileType.Man1 && tile <= TileType.Man9)
                {
                    hasMan = true;
                }
                else if (tile >= TileType.Pin1 && tile <= TileType.Pin9)
                {
                    hasPin = true;
                }
                else if (tile >= TileType.Sou1 && tile <= TileType.Sou9)
                {
                    hasSou = true;
                }
                else
                {
                    hasHonors = true;
                }
            }

            if (hasMan)
            {
                for (int i = (int)TileType.Man1;
                     i <= (int)TileType.Man9;
                     i++)
                {
                    tiles.Add((TileType)i);
                }
            }

            if (hasPin)
            {
                for (int i = (int)TileType.Pin1;
                     i <= (int)TileType.Pin9;
                     i++)
                {
                    tiles.Add((TileType)i);
                }
            }

            if (hasSou)
            {
                for (int i = (int)TileType.Sou1;
                     i <= (int)TileType.Sou9;
                     i++)
                {
                    tiles.Add((TileType)i);
                }
            }

            if (hasHonors)
            {
                for (int i = (int)TileType.East;
                     i <= (int)TileType.Red;
                     i++)
                {
                    tiles.Add((TileType)i);
                }
            }

            return tiles;
        }
    }
}