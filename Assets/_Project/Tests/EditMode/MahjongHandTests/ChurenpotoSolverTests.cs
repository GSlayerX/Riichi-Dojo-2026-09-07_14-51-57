using NUnit.Framework;
using RiichiDojo.Mahjong;

namespace RiichiDojo.Mahjong.Tests
{
    public class ChurenpotoSolverTests
    {
        [Test]
        public void IsWinningHand_ValidChurenpoto_ReturnsTrue()
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

            hand.AddTile(TileType.Man5);

            var solver = new ChurenpotoSolver();

            Assert.That(
                solver.IsWinningHand(hand),
                Is.True);
        }
        
        [Test]
        public void IsWinningHand_MixedSuits_ReturnsFalse()
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

            hand.AddTile(TileType.Pin9);
            hand.AddTile(TileType.Man5);

            var solver = new ChurenpotoSolver();

            Assert.That(
                solver.IsWinningHand(hand),
                Is.False);
        }
        
        [Test]
        public void IsWinningHand_ThirteenTiles_ReturnsFalse()
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

            var solver = new ChurenpotoSolver();

            Assert.That(
                solver.IsWinningHand(hand),
                Is.False);
        }
        
        [Test]
        public void CalculateWaits_ChurenpotoNineSidedWait_ReturnsAllNineTiles()
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

            WaitResult result =
                calculator.CalculateWaits(hand);

            Assert.That(
                result.Waits.Count,
                Is.EqualTo(9));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man1));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man2));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man3));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man4));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man5));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man6));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man7));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man8));

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man9));

            Assert.That(
                result.Patterns,
                Does.Contain(HandPattern.Churenpoto));
        }
        
        [Test]
        public void CalculateWaits_ChurenpotoSingleWait_ReturnsMan7()
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
            hand.AddTile(TileType.Man6);

            hand.AddTile(TileType.Man8);

            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Man9);
            hand.AddTile(TileType.Man9);

            var calculator = new WaitCalculator();

            WaitResult result =
                calculator.CalculateWaits(hand);

            Assert.That(
                result.Waits,
                Does.Contain(TileType.Man7));

            Assert.That(
                result.Patterns,
                Does.Contain(HandPattern.Churenpoto));
        }
        
        [Test]
        public void GenerateHand_ChurenpotoWeight100_GeneratesChurenpoto()
        {
            var settings = new HandGenerationSettings(
                numberOfSuits: 1,
                noHonorsWeight: 100,
                windsWeight: 0,
                dragonsWeight: 0,
                windsAndDragonsWeight: 0,
                chiitoitsuWeight: 0,
                kokushiWeight: 0,
                churenpotoWeight: 100);

            var generator = new HandGenerator();
            var calculator = new WaitCalculator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                WaitResult result =
                    calculator.CalculateWaits(hand);

                Assert.That(
                    result.Patterns,
                    Does.Contain(HandPattern.Churenpoto));
            }
        }
    }
}