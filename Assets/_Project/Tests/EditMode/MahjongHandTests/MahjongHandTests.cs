using NUnit.Framework;
using RiichiDojo.Mahjong;
using System.Collections.Generic;
using System.Linq;

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

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.IsEmpty(result.Waits);
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

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Man9);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(2, result.Waits.Count);
            Assert.That(result.Waits, Does.Contain(TileType.Sou1));
            Assert.That(result.Waits, Does.Contain(TileType.Sou4));
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

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(2, result.Waits.Count);
            Assert.That(result.Waits, Does.Contain(TileType.Sou3));
            Assert.That(result.Waits, Does.Contain(TileType.Sou6));
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

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(1, result.Waits.Count);
            Assert.That(result.Waits, Does.Contain(TileType.Man3));
        }
        
        [Test]
        public void CalculateWaits_HandIsNot13Tiles_ReturnsEmptyList()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            // Только 12 тайлов.
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.Sou7);
            hand.AddTile(TileType.Sou8);
            hand.AddTile(TileType.Sou9);

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.IsEmpty(result.Waits);
        }
        
        [Test]
        public void CalculateWaits_NoWinningTile_ReturnsEmptyList()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

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

            // 12 Souzu
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);

            // 34 Manzu
            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.IsEmpty(result.Waits);
        }
        /*
        [Test]
        public void IsWinningHand_Chiitoitsu_ReturnsTrue()
        {
            var hand = new MahjongHand();
            var solver = new ChiitoitsuSolver();

            // 11 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            // 22 Manzu
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            // 33 Pinzu
            hand.AddTile(TileType.Pin3);
            hand.AddTile(TileType.Pin3);

            // 44 Pinzu
            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin4);

            // 55 Souzu
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou5);

            // 66 Souzu
            hand.AddTile(TileType.Sou6);
            hand.AddTile(TileType.Sou6);

            // East East
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.East);

            Assert.IsTrue(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void IsWinningHand_Chiitoitsu_FourOfAKindIsNotTwoPairs()
        {
            var hand = new MahjongHand();
            var solver = new ChiitoitsuSolver();

            // 11 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            // 22 Manzu
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            // 33 Pinzu
            hand.AddTile(TileType.Pin3);
            hand.AddTile(TileType.Pin3);

            // 44 Pinzu
            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin4);

            // Four 5 Souzu
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou5);

            // 66 Souzu
            hand.AddTile(TileType.Sou6);
            hand.AddTile(TileType.Sou6);

            // East East
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.East);

            Assert.IsFalse(solver.IsWinningHand(hand));
        }
        */
        [Test]
        public void CalculateWaits_Chiitoitsu_ReturnsExpectedTile()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            // 11 Manzu
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            // 22 Manzu
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            // 33 Pinzu
            hand.AddTile(TileType.Pin3);
            hand.AddTile(TileType.Pin3);

            // 44 Pinzu
            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin4);

            // 55 Souzu
            hand.AddTile(TileType.Sou5);
            hand.AddTile(TileType.Sou5);

            // 66 Souzu
            hand.AddTile(TileType.Sou6);
            hand.AddTile(TileType.Sou6);

            // Single East
            hand.AddTile(TileType.East);

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(1, result.Waits.Count);
            Assert.That(result.Waits, Does.Contain(TileType.East));
        }
        /*
        [Test]
        public void IsWinningHand_Kokushi_ReturnsTrue()
        {
            var hand = new MahjongHand();
            var solver = new KokushiSolver();

            // 1 Manzu
            hand.AddTile(TileType.Man1);

            // 9 Manzu
            hand.AddTile(TileType.Man9);

            // 1 Pinzu
            hand.AddTile(TileType.Pin1);

            // 9 Pinzu
            hand.AddTile(TileType.Pin9);

            // 1 Souzu
            hand.AddTile(TileType.Sou1);

            // 9 Souzu
            hand.AddTile(TileType.Sou9);

            // Honors
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.South);
            hand.AddTile(TileType.West);
            hand.AddTile(TileType.North);
            hand.AddTile(TileType.White);
            hand.AddTile(TileType.Green);
            hand.AddTile(TileType.Red);

            // Duplicate terminal/honor
            hand.AddTile(TileType.Man1);

            Assert.IsTrue(solver.IsWinningHand(hand));
        }
        */
        [Test]
        public void CalculateWaits_Kokushi_ReturnsExpectedTile()
        {
            var hand = new MahjongHand();
            var calculator = new WaitCalculator();

            // 1 Manzu
            hand.AddTile(TileType.Man1);

            // 9 Manzu
            hand.AddTile(TileType.Man9);

            // 1 Pinzu
            hand.AddTile(TileType.Pin1);

            // 9 Pinzu
            hand.AddTile(TileType.Pin9);

            // 1 Souzu
            hand.AddTile(TileType.Sou1);

            // 9 Souzu
            hand.AddTile(TileType.Sou9);

            // Honors
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.South);
            hand.AddTile(TileType.West);
            hand.AddTile(TileType.North);
            hand.AddTile(TileType.White);
            hand.AddTile(TileType.Green);

            // Second 1 Manzu
            hand.AddTile(TileType.Man1);

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(1, result.Waits.Count);
            Assert.That(result.Waits, Does.Contain(TileType.Red));
        }

        [Test]
        public void CalculateWaits_KokushiThirteenSidedWait_ReturnsAllThirteenTiles()
        {
            var hand = new MahjongHand();

            // 13 уникальных терминальных и почётных тайлов.
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou9);
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.South);
            hand.AddTile(TileType.West);
            hand.AddTile(TileType.North);
            hand.AddTile(TileType.White);
            hand.AddTile(TileType.Green);
            hand.AddTile(TileType.Red);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(13, result.Waits.Count);

            Assert.That(result.Waits, Does.Contain(TileType.Man1));
            Assert.That(result.Waits, Does.Contain(TileType.Man9));
            Assert.That(result.Waits, Does.Contain(TileType.Pin1));
            Assert.That(result.Waits, Does.Contain(TileType.Pin9));
            Assert.That(result.Waits, Does.Contain(TileType.Sou1));
            Assert.That(result.Waits, Does.Contain(TileType.Sou9));
            Assert.That(result.Waits, Does.Contain(TileType.East));
            Assert.That(result.Waits, Does.Contain(TileType.South));
            Assert.That(result.Waits, Does.Contain(TileType.West));
            Assert.That(result.Waits, Does.Contain(TileType.North));
            Assert.That(result.Waits, Does.Contain(TileType.White));
            Assert.That(result.Waits, Does.Contain(TileType.Green));
            Assert.That(result.Waits, Does.Contain(TileType.Red));
        }
        
        [Test]
        public void IsWinningHand_KokushiMissingTile_ReturnsFalse()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou9);
            hand.AddTile(TileType.East);
            hand.AddTile(TileType.South);
            hand.AddTile(TileType.West);
            hand.AddTile(TileType.North);
            hand.AddTile(TileType.White);
            hand.AddTile(TileType.Green);

            // Вместо Red добавляем лишний терминальный тайл.
            hand.AddTile(TileType.Man9);

            var solver = new KokushiSolver();

            Assert.IsFalse(solver.IsWinningHand(hand));
        }
        
        [Test]
        public void WaitResult_StoresHandAndWaits()
        {
            var hand = new MahjongHand();
            hand.AddTile(TileType.Man1);

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var result = new WaitResult(hand, waits);

            Assert.AreSame(hand, result.Hand);
            Assert.AreEqual(2, result.Waits.Count);
            Assert.That(result.Waits, Does.Contain(TileType.Man2));
            Assert.That(result.Waits, Does.Contain(TileType.Man3));
        }
        
        [Test]
        public void Tiles_ReturnsAllTiles()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Sou5);

            var tiles = hand.Tiles;

            Assert.AreEqual(3, tiles.Count);
            Assert.That(tiles, Does.Contain(TileType.Man1));
            Assert.That(tiles, Does.Contain(TileType.Sou5));
        }
        
        [Test]
        public void Tiles_PreservesTileCopies()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.East);

            var tiles = hand.Tiles;

            Assert.AreEqual(4, tiles.Count);
            Assert.AreEqual(3, tiles.Count(t => t == TileType.Man5));
            Assert.AreEqual(1, tiles.Count(t => t == TileType.East));
        }
    }
}