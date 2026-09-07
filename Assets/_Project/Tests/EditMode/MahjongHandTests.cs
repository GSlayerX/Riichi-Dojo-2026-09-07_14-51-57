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
    }
}