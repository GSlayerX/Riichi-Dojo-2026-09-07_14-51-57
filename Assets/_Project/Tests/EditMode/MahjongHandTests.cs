using NUnit.Framework;

namespace RiichiDojo.Mahjong.Tests
{
    public class MahjongHandTests
    {
        [Test]
        public void AddTile_IncreasesTileCount()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man5);

            Assert.AreEqual(1, hand.GetCount(TileType.Man5));
            Assert.AreEqual(1, hand.TileCount);
        }

        [Test]
        public void AddSameTile_Twice_IncreasesCountToTwo()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);

            Assert.AreEqual(2, hand.GetCount(TileType.Man5));
            Assert.AreEqual(2, hand.TileCount);
        }

        [Test]
        public void RemoveTile_DecreasesTileCount()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);

            hand.RemoveTile(TileType.Man5);

            Assert.AreEqual(1, hand.GetCount(TileType.Man5));
            Assert.AreEqual(1, hand.TileCount);
        }
        
        [Test]
        public void MahjongHand_Tiles_AreReturnedInTileTypeOrder()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Red);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Pin2);

            var tiles = hand.Tiles;

            Assert.That(
                tiles,
                Is.EqualTo(new[]
                {
                    TileType.Man1,
                    TileType.Man3,
                    TileType.Pin2,
                    TileType.Red
                }));
        }
        
        [Test]
        public void MahjongHand_GetAvailableSelectionTiles_ManAndSou_ReturnsBothSuits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Sou7);

            var tiles = hand.GetAvailableSelectionTiles();

            Assert.That(
                tiles,
                Is.EqualTo(new[]
                {
                    TileType.Man1,
                    TileType.Man2,
                    TileType.Man3,
                    TileType.Man4,
                    TileType.Man5,
                    TileType.Man6,
                    TileType.Man7,
                    TileType.Man8,
                    TileType.Man9,

                    TileType.Sou1,
                    TileType.Sou2,
                    TileType.Sou3,
                    TileType.Sou4,
                    TileType.Sou5,
                    TileType.Sou6,
                    TileType.Sou7,
                    TileType.Sou8,
                    TileType.Sou9
                }));
        }
        
        [Test]
        public void MahjongHand_GetAvailableSelectionTiles_Honors_ReturnsAllHonors()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.White);

            var tiles = hand.GetAvailableSelectionTiles();

            Assert.That(
                tiles,
                Is.EqualTo(new[]
                {
                    TileType.East,
                    TileType.South,
                    TileType.West,
                    TileType.North,
                    TileType.White,
                    TileType.Green,
                    TileType.Red
                }));
        }
        
        [Test]
        public void MahjongHand_GetAvailableSelectionTiles_OnlyMan_ReturnsOnlyMan()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man5);

            var tiles = hand.GetAvailableSelectionTiles();

            Assert.That(
                tiles,
                Is.EqualTo(new[]
                {
                    TileType.Man1,
                    TileType.Man2,
                    TileType.Man3,
                    TileType.Man4,
                    TileType.Man5,
                    TileType.Man6,
                    TileType.Man7,
                    TileType.Man8,
                    TileType.Man9
                }));
        }
        
        [Test]
        public void MahjongHand_GetAvailableSelectionTiles_AllCategories_ReturnsAllTiles()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.East);

            var tiles = hand.GetAvailableSelectionTiles();

            Assert.That(
                tiles,
                Is.EqualTo(new[]
                {
                    TileType.Man1,
                    TileType.Man2,
                    TileType.Man3,
                    TileType.Man4,
                    TileType.Man5,
                    TileType.Man6,
                    TileType.Man7,
                    TileType.Man8,
                    TileType.Man9,

                    TileType.Pin1,
                    TileType.Pin2,
                    TileType.Pin3,
                    TileType.Pin4,
                    TileType.Pin5,
                    TileType.Pin6,
                    TileType.Pin7,
                    TileType.Pin8,
                    TileType.Pin9,

                    TileType.Sou1,
                    TileType.Sou2,
                    TileType.Sou3,
                    TileType.Sou4,
                    TileType.Sou5,
                    TileType.Sou6,
                    TileType.Sou7,
                    TileType.Sou8,
                    TileType.Sou9,

                    TileType.East,
                    TileType.South,
                    TileType.West,
                    TileType.North,
                    TileType.White,
                    TileType.Green,
                    TileType.Red
                }));
        }
    }
}