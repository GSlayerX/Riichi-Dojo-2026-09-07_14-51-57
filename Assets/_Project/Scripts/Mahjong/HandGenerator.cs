using System;
using System.Collections.Generic;
using System.Linq;

namespace RiichiDojo.Mahjong
{
    public class HandGenerator
    {
        private const int HandSize = 13;
        private const int TileTypeCount = 34;

        private readonly Random _random = new Random();
        private readonly WaitCalculator _calculator;

        public HandGenerator()
        {
            _calculator = new WaitCalculator();
        }

        public HandGenerator(WaitCalculator calculator)
        {
            _calculator = calculator;
        }

        public MahjongHand GenerateHand()
        {
            return GenerateHand(1, 9);
        }

        public MahjongHand GenerateHand(HandPattern pattern)
        {
            if (pattern == HandPattern.Chiitoitsu)
            {
                return GenerateRandomChiitoitsuHand();
            }

            if (pattern == HandPattern.Kokushi)
            {
                return GenerateRandomKokushiHand();
            }

            return GenerateHand(1, 9);
        }

        public MahjongHand GenerateHand(
            int minWaits,
            int maxWaits)
        {
            while (true)
            {
                MahjongHand hand;

                if (minWaits >= 6 && maxWaits >= 9)
                {
                    if (_random.NextDouble() < 0.2)
                    {
                        hand = GenerateChurenpotoHand();
                    }
                    else
                    {
                        hand = GenerateRandomStandardTenpaiHand();
                    }
                }
                else
                {
                    hand = GenerateRandomStandardTenpaiHand();
                }

                var result = _calculator.CalculateWaits(hand);

                if (!result.Patterns.Contains(HandPattern.Standard))
                {
                    continue;
                }

                if (result.Waits.Count >= minWaits &&
                    result.Waits.Count <= maxWaits)
                {
                    return hand;
                }
            }
        }

        public TrainingQuestion GenerateQuestion()
        {
            var hand = GenerateHand();

            var result = _calculator.CalculateWaits(hand);

            return new TrainingQuestion(
                hand,
                result.Waits,
                result.Patterns);
        }

        public TrainingQuestion GenerateQuestion(
            int minWaits,
            int maxWaits)
        {
            var hand = GenerateHand(minWaits, maxWaits);

            var result = _calculator.CalculateWaits(hand);

            return new TrainingQuestion(
                hand,
                result.Waits,
                result.Patterns);
        }

        public TrainingQuestion GenerateQuestion(HandPattern pattern)
        {
            var hand = GenerateHand(pattern);

            var result = _calculator.CalculateWaits(hand);

            return new TrainingQuestion(
                hand,
                result.Waits,
                result.Patterns);
        }

        private MahjongHand GenerateRandomStandardTenpaiHand()
        {
            while (true)
            {
                var winningHand = GenerateRandomWinningStandardHand();

                int tileIndex = _random.Next(winningHand.TileCount);

                TileType removedTile =
                    winningHand.Tiles[tileIndex];

                winningHand.RemoveTile(removedTile);

                if (winningHand.TileCount == HandSize)
                {
                    return winningHand;
                }
            }
        }

        private MahjongHand GenerateRandomWinningStandardHand()
        {
            while (true)
            {
                var hand = new MahjongHand();

                var usedCounts = new int[TileTypeCount];

                TileType pair = GetRandomTileWithAvailableCopies(
                    usedCounts,
                    2);

                AddTile(hand, usedCounts, pair);
                AddTile(hand, usedCounts, pair);

                for (int i = 0; i < 4; i++)
                {
                    if (_random.NextDouble() < 0.5)
                    {
                        TileType triplet =
                            GetRandomTileWithAvailableCopies(
                                usedCounts,
                                3);

                        AddTile(hand, usedCounts, triplet);
                        AddTile(hand, usedCounts, triplet);
                        AddTile(hand, usedCounts, triplet);
                    }
                    else
                    {
                        if (!TryAddRandomSequence(
                                hand,
                                usedCounts))
                        {
                            i--;
                        }
                    }
                }

                if (hand.TileCount == 14)
                {
                    return hand;
                }
            }
        }

        private bool TryAddRandomSequence(
            MahjongHand hand,
            int[] usedCounts)
        {
            int suit = _random.Next(3);
            int startRank = _random.Next(7);

            int first = suit * 9 + startRank;
            int second = first + 1;
            int third = first + 2;

            if (usedCounts[first] >= 4 ||
                usedCounts[second] >= 4 ||
                usedCounts[third] >= 4)
            {
                return false;
            }

            AddTile(hand, usedCounts, (TileType)first);
            AddTile(hand, usedCounts, (TileType)second);
            AddTile(hand, usedCounts, (TileType)third);

            return true;
        }

        private TileType GetRandomTileWithAvailableCopies(
            int[] usedCounts,
            int requiredCopies)
        {
            while (true)
            {
                TileType tile =
                    (TileType)_random.Next(TileTypeCount);

                if (usedCounts[(int)tile] +
                    requiredCopies <= 4)
                {
                    return tile;
                }
            }
        }

        private void AddTile(
            MahjongHand hand,
            int[] usedCounts,
            TileType tile)
        {
            hand.AddTile(tile);
            usedCounts[(int)tile]++;
        }

        private MahjongHand GenerateChurenpotoHand()
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

            return hand;
        }

        private MahjongHand GenerateChiitoitsuHand()
        {
            var hand = new MahjongHand();

            hand.AddTile(TileType.Man1);
            hand.AddTile(TileType.Man1);

            hand.AddTile(TileType.Man2);
            hand.AddTile(TileType.Man2);

            hand.AddTile(TileType.Pin3);
            hand.AddTile(TileType.Pin3);

            hand.AddTile(TileType.Pin5);
            hand.AddTile(TileType.Pin5);

            hand.AddTile(TileType.Sou4);
            hand.AddTile(TileType.Sou4);

            hand.AddTile(TileType.East);
            hand.AddTile(TileType.East);

            hand.AddTile(TileType.Red);

            return hand;
        }

        private MahjongHand GenerateRandomChiitoitsuHand()
        {
            var hand = new MahjongHand();

            var selectedTiles =
                new HashSet<TileType>();

            while (selectedTiles.Count < 6)
            {
                TileType tile =
                    (TileType)_random.Next(TileTypeCount);

                if (selectedTiles.Add(tile))
                {
                    hand.AddTile(tile);
                    hand.AddTile(tile);
                }
            }

            TileType singleTile;

            do
            {
                singleTile =
                    (TileType)_random.Next(TileTypeCount);
            }
            while (selectedTiles.Contains(singleTile));

            hand.AddTile(singleTile);

            return hand;
        }

        public MahjongHand GenerateRandomKokushiHand()
        {
            KokushiWaitType waitType =
                _random.Next(2) == 0
                    ? KokushiWaitType.ThirteenSided
                    : KokushiWaitType.SingleTile;

            return GenerateRandomKokushiHand(waitType);
        }

        public MahjongHand GenerateRandomKokushiHand(
            KokushiWaitType waitType)
        {
            var hand = new MahjongHand();

            var terminalHonors = new[]
            {
                TileType.Man1,
                TileType.Man9,
                TileType.Pin1,
                TileType.Pin9,
                TileType.Sou1,
                TileType.Sou9,
                TileType.East,
                TileType.South,
                TileType.West,
                TileType.North,
                TileType.White,
                TileType.Green,
                TileType.Red
            };

            foreach (TileType tile in terminalHonors)
            {
                hand.AddTile(tile);
            }

            if (waitType == KokushiWaitType.ThirteenSided)
            {
                return hand;
            }

            TileType duplicateTile =
                terminalHonors[
                    _random.Next(terminalHonors.Length)];

            TileType removedTile;

            do
            {
                removedTile =
                    terminalHonors[
                        _random.Next(terminalHonors.Length)];
            }
            while (removedTile == duplicateTile);

            hand.RemoveTile(removedTile);
            hand.AddTile(duplicateTile);

            return hand;
        }
    }
}