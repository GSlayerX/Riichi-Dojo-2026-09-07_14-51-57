using NUnit.Framework;
using RiichiDojo.Mahjong;

namespace RiichiDojo.Mahjong.Tests
{
    public class HandGeneratorTests
    {
        [Test]
        public void GenerateHand_ThreeSuits_UsesExactlyThreeSuits()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    CountNumberedSuits(hand),
                    Is.EqualTo(3));
            }
        }

        private int CountNumberedSuits(
            MahjongHand hand)
        {
            bool hasMan = false;
            bool hasPin = false;
            bool hasSou = false;

            foreach (TileType tile in hand.Tiles)
            {
                int index = (int)tile;

                if (index < 9)
                {
                    hasMan = true;
                }
                else if (index < 18)
                {
                    hasPin = true;
                }
                else if (index < 27)
                {
                    hasSou = true;
                }
            }

            int count = 0;

            if (hasMan)
            {
                count++;
            }

            if (hasPin)
            {
                count++;
            }

            if (hasSou)
            {
                count++;
            }

            return count;
        }
        
        [Test]
        public void GenerateHand_TwoSuits_UsesExactlyTwoSuits()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 2,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    CountNumberedSuits(hand),
                    Is.EqualTo(2));
            }
        }

        [Test]
        public void GenerateHand_OneSuit_UsesExactlyOneSuit()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 1,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    CountNumberedSuits(hand),
                    Is.EqualTo(1));
            }
        }
        
        [Test]
        public void GenerateHand_NoHonors_ContainsNoHonors()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    ContainsWind(hand),
                    Is.False);

                Assert.That(
                    ContainsDragon(hand),
                    Is.False);
            }
        }

        [Test]
        public void GenerateHand_Winds_ContainsWindButNoDragons()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 0,
                    windsWeight: 100,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    ContainsWind(hand),
                    Is.True);

                Assert.That(
                    ContainsDragon(hand),
                    Is.False);
            }
        }

        [Test]
        public void GenerateHand_Dragons_ContainsDragonButNoWinds()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 0,
                    windsWeight: 0,
                    dragonsWeight: 100,
                    windsAndDragonsWeight: 0);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    ContainsWind(hand),
                    Is.False);

                Assert.That(
                    ContainsDragon(hand),
                    Is.True);
            }
        }

        [Test]
        public void GenerateHand_WindsAndDragons_ContainsBoth()
        {
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 0,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 100);

            var generator = new HandGenerator();

            for (int i = 0; i < 100; i++)
            {
                MahjongHand hand =
                    generator.GenerateHand(settings);

                Assert.That(
                    ContainsWind(hand),
                    Is.True);

                Assert.That(
                    ContainsDragon(hand),
                    Is.True);
            }
        }

        private bool ContainsWind(MahjongHand hand)
        {
            foreach (TileType tile in hand.Tiles)
            {
                if (tile >= TileType.East &&
                    tile <= TileType.North)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsDragon(MahjongHand hand)
        {
            foreach (TileType tile in hand.Tiles)
            {
                if (tile >= TileType.White &&
                    tile <= TileType.Red)
                {
                    return true;
                }
            }

            return false;
        }
    }
}