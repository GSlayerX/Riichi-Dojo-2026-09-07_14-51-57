using NUnit.Framework;
using RiichiDojo.Mahjong;

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
        public void AddSameTile_FiveTimes_ThrowsException()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);

            Assert.Throws<System.InvalidOperationException>(
                () => hand.AddTile(TileType.Man5));
        }
        
        [Test]
        public void RemoveTile_WhenTileIsNotInHand_ThrowsException()
        {
            var hand = new MahjongHand();

            Assert.Throws<System.InvalidOperationException>(
                () => hand.RemoveTile(TileType.Man5));
        }
        
        [Test]
        public void CalculateWaits_EmptyHand_ReturnsEmptyList()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            var waits = calculator.CalculateWaits(hand);

            Assert.IsEmpty(waits);
        }
        
        [Test]
        public void IsWinningHand_StandardHand_ReturnsTrue()
        {
            var hand = new MahjongHand();
            var solver = new StandardHandSolver();

            // 123 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            // 123 Pinzu
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            // 123 Souzu
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            // 789 Souzu
            hand.AddTile(TileType.Sou7);
            hand.AddTile(TileType.Sou8);
            hand.AddTile(TileType.Sou9);

            // Pair of 5 Manzu
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);

            Assert.IsTrue(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void IsWinningHand_InvalidHand_ReturnsFalse()
        {
            var hand = new MahjongHand();
            var solver = new StandardHandSolver();

            // 1 2 3 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            // 1 2 3 Pinzu
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            // 1 2 3 Souzu
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            // 4 5 6 Souzu
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou6);

            // 1 2 Manzu — не хватает пары/мэлда
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);

            Assert.IsFalse(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void IsWinningHand_AllTriplets_ReturnsTrue()
        {
            var hand = new MahjongHand();
            var solver = new StandardHandSolver();

            // 111 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            // 222 Manzu
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            // 333 Pinzu
            hand.AddTile(TileType.Pin3);
            hand.AddTile(TileType.Pin3);
            hand.AddTile(TileType.Pin3);

            // 444 Souzu
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);

            // Pair of White Dragons
            hand.AddTile(TileType.White);
            hand.AddTile(TileType.White);

            Assert.IsTrue(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void IsWinningHand_HonorsCannotFormSequence_ReturnsFalse()
        {
            var hand = new MahjongHand();
            var solver = new StandardHandSolver();

            // East, South, West — НЕ последовательность
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.South);
            hand.AddTile(TileType.West);

            // 123 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            // 123 Pinzu
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            // 456 Souzu
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou6);

            // Pair of Red Dragons
            hand.AddTile(TileType.Red);
            hand.AddTile(TileType.Red);

            Assert.IsFalse(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void IsWinningHand_RequiresChoosingCorrectPair_ReturnsTrue()
        {
            var hand = new MahjongHand();
            var solver = new StandardHandSolver();

            // 111 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            // 123 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            // 456 Pinzu
            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin6);

            // 789 Souzu
            hand.AddTile(TileType.Sou7);
            hand.AddTile(TileType.Sou8);
            hand.AddTile(TileType.Sou9);

            // Pair of Red Dragons
            hand.AddTile(TileType.Red);
            hand.AddTile(TileType.Red);

            Assert.IsTrue(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void CalculateWaits_SimpleHand_ReturnsExpectedTile()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            // 123 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            // 123 Pinzu
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            // 123 Souzu
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            // 789 Souzu
            hand.AddTile(TileType.Sou7);
            hand.AddTile(TileType.Sou8);
            hand.AddTile(TileType.Sou9);

            // Single 5 Manzu
            hand.AddTile(TileType.Man5);

            var waits = calculator.CalculateWaits(hand);

            Assert.AreEqual(1, waits.Count);
            Assert.Contains(TileType.Man5, (System.Collections.ICollection)waits);
        }
        
        [Test]
        public void CalculateWaits_TwoSidedWait_ReturnsTwoTiles()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            // 123 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            // 123 Pinzu
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            // 123 Souzu
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            // 45 Souzu
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou5);

            // Pair of 5 Manzu
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);

            var waits = calculator.CalculateWaits(hand);

            Assert.AreEqual(2, waits.Count);
            Assert.That(waits, Does.Contain(TileType.Sou3));
            Assert.That(waits, Does.Contain(TileType.Sou6));
        }

        [Test]
        public void CalculateWaits_EdgeWait_ReturnsExpectedTile()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            // 123 Pinzu
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            // 123 Souzu
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            // 456 Souzu
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou6);

            // 12 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);

            // Pair of 5 Manzu
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);

            var waits = calculator.CalculateWaits(hand);

            Assert.AreEqual(1, waits.Count);
            Assert.That(waits, Does.Contain(TileType.Man3));
        }

    }
}