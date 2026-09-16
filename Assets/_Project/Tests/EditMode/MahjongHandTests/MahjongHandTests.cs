using NUnit.Framework;
using RiichiDojo.Mahjong;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            var result = new WaitResult(
                hand,
                waits,
                new List<HandPattern>());

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
        
        [Test]
        public void HandGenerator_Generates13TileHand()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var hand = generator.GenerateHand(settings);

            Assert.AreEqual(13, hand.TileCount);
        }
        
        [Test]
        public void HandGenerator_NeverGeneratesMoreThanFourCopiesOfTile()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var hand = generator.GenerateHand(settings);

            for (int i = 0; i < 34; i++)
            {
                var tile = (TileType)i;

                Assert.LessOrEqual(
                    hand.GetCount(tile),
                    4);
            }
        }
        
        [Test]
        public void HandGenerator_GeneratesDifferentHands()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var hand1 = generator.GenerateHand(settings);
            var hand2 = generator.GenerateHand(settings);

            Assert.IsFalse(
                hand1.Tiles.SequenceEqual(hand2.Tiles));
        }
        
        [Test]
        public void HandGenerator_GeneratedHand_CanBeUsedByWaitCalculator()
        {
            var generator = new HandGenerator();
            var calculator = new WaitCalculator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var hand = generator.GenerateHand(settings);

            var result = calculator.CalculateWaits(hand);

            Assert.AreSame(hand, result.Hand);
            Assert.IsNotNull(result.Waits);
        }
        
        [Test]
        public void HandGenerator_GeneratesHandWithAtLeastOneWait()
        {
            var generator = new HandGenerator();
            var calculator = new WaitCalculator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var hand = generator.GenerateHand(settings);
            var result = calculator.CalculateWaits(hand);

            Assert.IsNotEmpty(result.Waits);
        }
        
        [Test]
        public void HandGenerator_GeneratedHand_HasValidWaits()
        {
            var generator = new HandGenerator();
            var calculator = new WaitCalculator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var hand = generator.GenerateHand(settings);
            var result = calculator.CalculateWaits(hand);

            Assert.GreaterOrEqual(result.Waits.Count, 1);
            Assert.LessOrEqual(result.Waits.Count, 34);
        }
        
        [Test]
        public void TrainingQuestion_StoresHandAndWaits()
        {
            var hand = new MahjongHand();
            hand.AddTile(TileType.Man1);

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(hand, waits, patterns);

            Assert.AreSame(hand, question.Hand);
            Assert.AreEqual(2, question.Waits.Count);
            Assert.That(question.Waits, Does.Contain(TileType.Man2));
            Assert.That(question.Waits, Does.Contain(TileType.Man3));
            Assert.AreEqual(1, question.Patterns.Count);
            Assert.That(question.Patterns, Does.Contain(HandPattern.Standard));
        }
        
        [Test]
        public void HandGenerator_GeneratesTrainingQuestion()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var question =
                generator.GenerateQuestion(settings);

            Assert.IsNotNull(question);
            Assert.IsNotNull(question.Hand);
            Assert.IsNotNull(question.Waits);
            Assert.AreEqual(13, question.Hand.TileCount);
            Assert.GreaterOrEqual(question.Waits.Count, 1);
        }
        
        [Test]
        public void HandGenerator_GeneratesDifferentQuestions()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var question1 =
                generator.GenerateQuestion(settings);

            var question2 =
                generator.GenerateQuestion(settings);

            Assert.IsFalse(
                question1.Hand.Tiles.SequenceEqual(
                    question2.Hand.Tiles));
        }
        
        [Test]
        public void HandGenerator_GeneratedQuestion_HasSameHandAndWaits()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var question =
                generator.GenerateQuestion(settings);

            var calculator = new WaitCalculator();
            var result = calculator.CalculateWaits(question.Hand);

            Assert.AreEqual(
                result.Waits.Count,
                question.Waits.Count);

            foreach (var wait in result.Waits)
            {
                Assert.That(
                    question.Waits,
                    Does.Contain(wait));
            }

            Assert.That(
                question.Patterns.Count,
                Is.GreaterThan(0));
        }
        
        [Test]
        public void HandGenerator_GenerateQuestion_HasAtLeastOneWait()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            var question =
                generator.GenerateQuestion(settings);

            Assert.IsNotEmpty(question.Waits);
        }
        
        [Test]
        public void HandGenerator_AcceptsWaitCalculator()
        {
            var calculator = new WaitCalculator();

            var generator = new HandGenerator(calculator);

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var question =
                generator.GenerateQuestion(settings);

            Assert.IsNotNull(question);
            Assert.IsNotEmpty(question.Waits);
        }
        
        [Test]
        public void CalculateWaits_StandardHand_ReturnsStandardPattern()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);

            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin5);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.That(result.Patterns, Does.Contain(HandPattern.Standard));
        }
        
        [Test]
        public void CalculateWaits_Chiitoitsu_ReturnsChiitoitsuPattern()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin1);

            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin2);

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou1);

            hand.AddTile(TileType.Sou2);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.That(result.Patterns, Does.Contain(HandPattern.Chiitoitsu));
        }
        
        [Test]
        public void CalculateWaits_Kokushi_ReturnsKokushiPattern()
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

            hand.AddTile(TileType.Man1);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.That(result.Patterns, Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void CalculateWaits_DoesNotDuplicatePatterns()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.East);
            hand.AddTile(TileType.East);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.AreEqual(
                result.Patterns.Count,
                new HashSet<HandPattern>(result.Patterns).Count);
        }
        
        [Test]
        public void CalculateWaits_PatternsContainOnlyUniqueValues()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin1);

            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin2);

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou1);

            hand.AddTile(TileType.Sou2);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.That(result.Patterns, Is.Not.Null);
            Assert.That(result.Patterns.Count, Is.GreaterThan(0));
        }
        
        [Test]
        public void CalculateWaits_HandIsNot13Tiles_ReturnsEmptyPatterns()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.That(result.Waits, Is.Empty);
            Assert.That(result.Patterns, Is.Empty);
        }
        
        [Test]
        public void CalculateWaits_TwoSidedWait_ReturnsOneStandardPattern()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);

            hand.AddTile(TileType.Pin1);
            hand.AddTile(TileType.Pin2);
            hand.AddTile(TileType.Pin3);

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);

            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin5);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.AreEqual(1, result.Patterns.Count);
            Assert.That(result.Patterns, Does.Contain(HandPattern.Standard));
        }
        
        [Test]
        public void HandGenerator_GeneratesQuestion_WithStandardPattern()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var question =
                generator.GenerateQuestion(settings);

            Assert.That(
                question.Waits.Count,
                Is.GreaterThan(0));
        }

        [Test]
        public void HandGenerator_GeneratedQuestion_HasPattern()
        {
            var generator = new HandGenerator();

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var question =
                generator.GenerateQuestion(settings);

            Assert.That(
                question.Patterns.Count,
                Is.GreaterThan(0));
        }

        [Test]
        public void HandGenerator_CanGenerateHandWithRequestedPattern()
        {
            var generator = new HandGenerator();

            var hand = generator.GenerateHand(HandPattern.Chiitoitsu);
            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.That(
                result.Patterns,
                Does.Contain(HandPattern.Chiitoitsu));
        }
        
        [Test]
        public void HandGenerator_GeneratesDifferentChiitoitsuHands()
        {
            var generator = new HandGenerator();

            var firstHand = generator.GenerateHand(HandPattern.Chiitoitsu);
            var secondHand = generator.GenerateHand(HandPattern.Chiitoitsu);

            Assert.That(
                firstHand.Tiles,
                Is.Not.EqualTo(secondHand.Tiles));
        }
        
        [Test]
        public void HandGenerator_GeneratesKokushiHandWithExpectedWait()
        {
            var generator = new HandGenerator();

            var hand = generator.GenerateRandomKokushiHand();
            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.That(
                hand.TileCount,
                Is.EqualTo(13));

            Assert.That(
                result.Patterns,
                Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void CalculateWaits_KokushiPairWait_ReturnsOneTile()
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
            hand.AddTile(TileType.Green);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.That(result.Waits.Count, Is.EqualTo(1));
            Assert.That(result.Waits, Does.Contain(TileType.Red));
            Assert.That(result.Patterns, Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void HandGenerator_CanGenerateKokushiHand()
        {
            var generator = new HandGenerator();

            var hand = generator.GenerateHand(HandPattern.Kokushi);
            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.That(
                result.Patterns,
                Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void HandGenerator_GeneratesKokushiThirteenSidedHand()
        {
            var generator = new HandGenerator();

            var hand = generator.GenerateRandomKokushiHand(KokushiWaitType.ThirteenSided);
            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.That(hand.TileCount, Is.EqualTo(13));
            Assert.That(result.Waits.Count, Is.EqualTo(13));
            Assert.That(result.Patterns, Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void HandGenerator_GeneratesKokushiSingleTileWaitHand()
        {
            var generator = new HandGenerator();

            var hand = generator.GenerateRandomKokushiHand(
                KokushiWaitType.SingleTile);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.That(hand.TileCount, Is.EqualTo(13));
            Assert.That(result.Waits.Count, Is.EqualTo(1));
            Assert.That(result.Patterns, Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void HandGenerator_GeneratesQuestionWithRequestedPattern()
        {
            var generator = new HandGenerator();

            var question = generator.GenerateQuestion(HandPattern.Chiitoitsu);

            Assert.That(
                question.Patterns,
                Does.Contain(HandPattern.Chiitoitsu));
        }
        
        [Test]
        public void HandGenerator_GeneratesQuestionWithRequestedKokushiPattern()
        {
            var generator = new HandGenerator();

            var question = generator.GenerateQuestion(HandPattern.Kokushi);

            Assert.That(
                question.Patterns,
                Does.Contain(HandPattern.Kokushi));
        }
        
        [Test]
        public void HandGenerator_GeneratesQuestionWithRequestedStandardPattern()
        {
            var generator = new HandGenerator();

            var question = generator.GenerateQuestion(HandPattern.Standard);

            Assert.That(
                question.Patterns,
                Does.Contain(HandPattern.Standard));
        }
        
        [Test]
        public void AnswerChecker_CorrectAnswer_ReturnsTrue()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.True);
        }
        
        [Test]
        public void AnswerChecker_CorrectAnswerInDifferentOrder_ReturnsTrue()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Man3,
                TileType.Man2
            };

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.True);
        }
        
        [Test]
        public void AnswerChecker_IncorrectAnswer_ReturnsFalse()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Man2,
                TileType.Man4
            };

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.False);
        }
        
        [Test]
        public void AnswerChecker_IncompleteAnswer_ReturnsFalse()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Man2
            };

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.False);
        }
        
        [Test]
        public void AnswerChecker_AnswerWithExtraTile_ReturnsFalse()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3,
                TileType.Man4
            };

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.False);
        }
        
        [Test]
        public void AnswerChecker_EmptyAnswer_ReturnsFalse()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>();

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.False);
        }
        
        [Test]
        public void AnswerChecker_EmptyWaitsAndEmptyAnswer_ReturnsTrue()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>();
            var patterns = new List<HandPattern>();

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>();

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.True);
        }
        
        [Test]
        public void AnswerChecker_NullSelectedTiles_ReturnsFalse()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Man2,
                TileType.Man3
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Standard
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            Assert.That(
                checker.IsCorrect(question, null),
                Is.False);
        }
        
        [Test]
        public void AnswerChecker_NullQuestion_ReturnsFalse()
        {
            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Man2
            };

            Assert.That(
                checker.IsCorrect(null, selectedTiles),
                Is.False);
        }
        
        [Test]
        public void AnswerChecker_CorrectSingleTileAnswer_ReturnsTrue()
        {
            var hand = new MahjongHand();

            var waits = new List<TileType>
            {
                TileType.Red
            };

            var patterns = new List<HandPattern>
            {
                HandPattern.Kokushi
            };

            var question = new TrainingQuestion(
                hand,
                waits,
                patterns);

            var checker = new AnswerChecker();

            var selectedTiles = new List<TileType>
            {
                TileType.Red
            };

            Assert.That(
                checker.IsCorrect(question, selectedTiles),
                Is.True);
        }
        
        [Test]
        public void HandGenerator_GeneratesValidChiitoitsuStructure()
        {
            var generator = new HandGenerator();

            var hand = generator.GenerateHand(HandPattern.Chiitoitsu);

            Assert.AreEqual(13, hand.TileCount);

            int pairCount = 0;
            int singleCount = 0;

            for (int i = 0; i < 34; i++)
            {
                var tile = (TileType)i;
                int count = hand.GetCount(tile);

                Assert.LessOrEqual(count, 2);

                if (count == 2)
                {
                    pairCount++;
                }
                else if (count == 1)
                {
                    singleCount++;
                }
            }

            Assert.AreEqual(6, pairCount);
            Assert.AreEqual(1, singleCount);
        }
        
        [Test]
        public void WaitCalculator_Churenpoto_ReturnsNineSidedWait()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man7);
            hand.AddTile(TileType.Man8);

            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Man9);

            var calculator = new WaitCalculator();

            var result = calculator.CalculateWaits(hand);

            Assert.AreEqual(9, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man1);
            CollectionAssert.Contains(result.Waits, TileType.Man2);
            CollectionAssert.Contains(result.Waits, TileType.Man3);
            CollectionAssert.Contains(result.Waits, TileType.Man4);
            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man6);
            CollectionAssert.Contains(result.Waits, TileType.Man7);
            CollectionAssert.Contains(result.Waits, TileType.Man8);
            CollectionAssert.Contains(result.Waits, TileType.Man9);

            CollectionAssert.Contains(result.Patterns, HandPattern.Standard);
        }
        
        [Test]
        public void WaitCalculator_OneSidedTemplate_ReturnsOneWait()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man8);
            hand.AddTile(TileType.Man9);

            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin6);
            hand.AddTile(TileType.Pin7);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Pin9);

            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(1, result.Waits.Count);
            CollectionAssert.Contains(result.Waits, TileType.Man7);
        }
        
        [Test]
        public void WaitCalculator_TwoSidedTemplate_ReturnsTwoWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man7);
            hand.AddTile(TileType.Man8);

            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin6);
            hand.AddTile(TileType.Pin7);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Pin9);

            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(2, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man6);
            CollectionAssert.Contains(result.Waits, TileType.Man9);
        }
        
        [Test]
        public void WaitCalculator_ThreeSidedTemplate_ReturnsThreeWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man7);
            hand.AddTile(TileType.Man8);
            hand.AddTile(TileType.Man9);

            hand.AddTile(TileType.Pin4);
            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin6);
            hand.AddTile(TileType.Pin7);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Pin9);

            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(3, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Pin4);
            CollectionAssert.Contains(result.Waits, TileType.Pin7);
            CollectionAssert.Contains(result.Waits, TileType.Pin8);
        }
        
        [Test]
        public void WaitCalculator_FourSidedTemplate_ReturnsFourWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man7);

            hand.AddTile(TileType.Pin7);
            hand.AddTile(TileType.Pin8);
            hand.AddTile(TileType.Pin9);

            hand.AddTile(TileType.Sou3);
            hand.AddTile(TileType.Sou3);
            hand.AddTile(TileType.Sou3);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(4, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man2);
            CollectionAssert.Contains(result.Waits, TileType.Man3);
            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man8);
        }
        
        [Test]
        public void WaitCalculator_FiveSidedTemplate_ReturnsFiveWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man4);

            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man6);

            hand.AddTile(TileType.Man7);

            hand.AddTile(TileType.Man8);
            hand.AddTile(TileType.Man8);
            hand.AddTile(TileType.Man8);

            hand.AddTile(TileType.Red);
            hand.AddTile(TileType.Red);
            hand.AddTile(TileType.Red);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(5, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man6);
            CollectionAssert.Contains(result.Waits, TileType.Man7);
            CollectionAssert.Contains(result.Waits, TileType.Man8);
            CollectionAssert.Contains(result.Waits, TileType.Man9);
        }
        
        [Test]
        public void WaitCalculator_SixSidedTemplate_ReturnsSixWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man6);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(6, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man1);
            CollectionAssert.Contains(result.Waits, TileType.Man2);
            CollectionAssert.Contains(result.Waits, TileType.Man3);
            CollectionAssert.Contains(result.Waits, TileType.Man4);
            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man6);
        }
        
        [Test]
        public void WaitCalculator_SevenSidedTemplate_ReturnsSevenWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);

            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man6);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(7, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man1);
            CollectionAssert.Contains(result.Waits, TileType.Man2);
            CollectionAssert.Contains(result.Waits, TileType.Man3);
            CollectionAssert.Contains(result.Waits, TileType.Man4);
            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man6);
            CollectionAssert.Contains(result.Waits, TileType.Man7);
        }
        
        [Test]
        public void WaitCalculator_EightSidedTemplate_ReturnsEightWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Sou1);
            hand.AddTile(TileType.Sou2);
            hand.AddTile(TileType.Sou3);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man7);

            hand.AddTile(TileType.Man7);
            hand.AddTile(TileType.Man7);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(8, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man1);
            CollectionAssert.Contains(result.Waits, TileType.Man2);
            CollectionAssert.Contains(result.Waits, TileType.Man3);
            CollectionAssert.Contains(result.Waits, TileType.Man4);
            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man6);
            CollectionAssert.Contains(result.Waits, TileType.Man7);
            CollectionAssert.Contains(result.Waits, TileType.Man8);
        }
        
        [Test]
        public void WaitCalculator_NineSidedTemplate_ReturnsNineWaits()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man3);
            hand.AddTile(TileType.Man4);
            hand.AddTile(TileType.Man5);
            hand.AddTile(TileType.Man6);
            hand.AddTile(TileType.Man7);
            hand.AddTile(TileType.Man8);
            hand.AddTile(TileType.Man9);

            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Man9);

            var result = new WaitCalculator().CalculateWaits(hand);

            Assert.AreEqual(9, result.Waits.Count);

            CollectionAssert.Contains(result.Waits, TileType.Man1);
            CollectionAssert.Contains(result.Waits, TileType.Man2);
            CollectionAssert.Contains(result.Waits, TileType.Man3);
            CollectionAssert.Contains(result.Waits, TileType.Man4);
            CollectionAssert.Contains(result.Waits, TileType.Man5);
            CollectionAssert.Contains(result.Waits, TileType.Man6);
            CollectionAssert.Contains(result.Waits, TileType.Man7);
            CollectionAssert.Contains(result.Waits, TileType.Man8);
            CollectionAssert.Contains(result.Waits, TileType.Man9);
        }
    }
}