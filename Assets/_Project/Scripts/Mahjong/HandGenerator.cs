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

        public MahjongHand GenerateHand(
            HandGenerationSettings settings)
        {
            return GenerateRandomStandardTenpaiHand(settings);
        }
        
        public HandGenerator(WaitCalculator calculator)
        {
            _calculator = calculator;
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

            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0);

            return GenerateHand(settings);
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

        public TrainingQuestion GenerateQuestion(
            HandGenerationSettings settings)
        {
            var hand = GenerateHand(settings);

            var result = _calculator.CalculateWaits(hand);

            return new TrainingQuestion(
                hand,
                result.Waits,
                result.Patterns);
        }

        private MahjongHand GenerateRandomStandardTenpaiHand(
            HandGenerationSettings settings)
        {
            while (true)
            {
                var selectedSuits =
                    SelectSuits(settings.NumberOfSuits);

                HonorConfiguration honorConfiguration =
                    SelectHonorConfiguration(settings);

                MahjongHand winningHand =
                    GenerateRandomWinningStandardHand(
                        selectedSuits,
                        honorConfiguration);

                int tileIndex =
                    _random.Next(winningHand.TileCount);

                TileType removedTile =
                    winningHand.Tiles[tileIndex];

                winningHand.RemoveTile(removedTile);

                if (winningHand.TileCount != HandSize)
                {
                    continue;
                }

                if (!MatchesGenerationSettings(
                        winningHand,
                        selectedSuits,
                        honorConfiguration))
                {
                    continue;
                }

                var result =
                    _calculator.CalculateWaits(winningHand);

                if (!result.Patterns.Contains(
                        HandPattern.Standard))
                {
                    continue;
                }

                return winningHand;
            }
        }
        
        private MahjongHand GenerateRandomWinningStandardHand(
            List<int> selectedSuits,
            HonorConfiguration honorConfiguration)
        {
            while (true)
            {
                var hand = new MahjongHand();
                var usedCounts = new int[TileTypeCount];

                TileType pair =
                    GetRandomPairTile(
                        selectedSuits,
                        honorConfiguration,
                        usedCounts);

                AddTile(hand, usedCounts, pair);
                AddTile(hand, usedCounts, pair);

                for (int i = 0; i < 4; i++)
                {
                    bool added;

                    if (_random.NextDouble() < 0.5)
                    {
                        added = TryAddRandomTriplet(
                            hand,
                            usedCounts,
                            selectedSuits,
                            honorConfiguration);
                    }
                    else
                    {
                        added = TryAddRandomSequence(
                            hand,
                            usedCounts,
                            selectedSuits);
                    }

                    if (!added)
                    {
                        i--;
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
            int[] usedCounts,
            List<int> selectedSuits)
        {
            if (selectedSuits.Count == 0)
            {
                return false;
            }

            int suit =
                selectedSuits[
                    _random.Next(selectedSuits.Count)];

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
        
        private List<int> SelectSuits(int numberOfSuits)
        {
            var suits = new List<int>
            {
                0,
                1,
                2
            };

            for (int i = suits.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);

                (suits[i], suits[j]) =
                    (suits[j], suits[i]);
            }

            return suits
                .Take(numberOfSuits)
                .ToList();
        }
        
        private HonorConfiguration SelectHonorConfiguration(
            HandGenerationSettings settings)
        {
            int totalWeight =
                settings.NoHonorsWeight +
                settings.WindsWeight +
                settings.DragonsWeight +
                settings.WindsAndDragonsWeight;

            if (totalWeight <= 0)
            {
                throw new InvalidOperationException(
                    "Total honor weight must be greater than zero.");
            }

            int roll = _random.Next(totalWeight);

            if (roll < settings.NoHonorsWeight)
            {
                return HonorConfiguration.None;
            }

            roll -= settings.NoHonorsWeight;

            if (roll < settings.WindsWeight)
            {
                return HonorConfiguration.Winds;
            }

            roll -= settings.WindsWeight;

            if (roll < settings.DragonsWeight)
            {
                return HonorConfiguration.Dragons;
            }

            return HonorConfiguration.WindsAndDragons;
        }
        
        private bool MatchesGenerationSettings(
            MahjongHand hand,
            List<int> selectedSuits,
            HonorConfiguration honorConfiguration)
        {
            var presentSuits = new HashSet<int>();

            bool hasWind = false;
            bool hasDragon = false;

            foreach (TileType tile in hand.Tiles)
            {
                int index = (int)tile;

                if (index < 27)
                {
                    int suit = index / 9;
                    presentSuits.Add(suit);
                    continue;
                }

                if (tile >= TileType.East &&
                    tile <= TileType.North)
                {
                    hasWind = true;
                }

                if (tile >= TileType.White &&
                    tile <= TileType.Red)
                {
                    hasDragon = true;
                }
            }

            if (presentSuits.Count != selectedSuits.Count)
            {
                return false;
            }

            foreach (int suit in selectedSuits)
            {
                if (!presentSuits.Contains(suit))
                {
                    return false;
                }
            }

            switch (honorConfiguration)
            {
                case HonorConfiguration.None:
                    return !hasWind && !hasDragon;

                case HonorConfiguration.Winds:
                    return hasWind && !hasDragon;

                case HonorConfiguration.Dragons:
                    return !hasWind && hasDragon;

                case HonorConfiguration.WindsAndDragons:
                    return hasWind && hasDragon;

                default:
                    return false;
            }
        }
        
        private TileType GetRandomPairTile(
            List<int> selectedSuits,
            HonorConfiguration honorConfiguration,
            int[] usedCounts)
        {
            var candidates = new List<TileType>();

            foreach (int suit in selectedSuits)
            {
                int first = suit * 9;

                for (int rank = 0; rank < 9; rank++)
                {
                    TileType tile =
                        (TileType)(first + rank);

                    if (usedCounts[(int)tile] <= 2)
                    {
                        candidates.Add(tile);
                    }
                }
            }

            if (honorConfiguration == HonorConfiguration.Winds ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                candidates.AddRange(GetWindTiles());
            }

            if (honorConfiguration == HonorConfiguration.Dragons ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                candidates.AddRange(GetDragonTiles());
            }

            return candidates[
                _random.Next(candidates.Count)];
        }
        
        private bool TryAddRandomTriplet(
            MahjongHand hand,
            int[] usedCounts,
            List<int> selectedSuits,
            HonorConfiguration honorConfiguration)
        {
            var candidates = new List<TileType>();

            foreach (int suit in selectedSuits)
            {
                int first = suit * 9;

                for (int rank = 0; rank < 9; rank++)
                {
                    int tileIndex = first + rank;
                    TileType candidate = (TileType)tileIndex;

                    if (usedCounts[tileIndex] <= 1)
                    {
                        candidates.Add(candidate);
                    }
                }
            }

            if (honorConfiguration == HonorConfiguration.Winds ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                foreach (TileType wind in GetWindTiles())
                {
                    if (usedCounts[(int)wind] <= 1)
                    {
                        candidates.Add(wind);
                    }
                }
            }

            if (honorConfiguration == HonorConfiguration.Dragons ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                foreach (TileType dragon in GetDragonTiles())
                {
                    if (usedCounts[(int)dragon] <= 1)
                    {
                        candidates.Add(dragon);
                    }
                }
            }

            if (candidates.Count == 0)
            {
                return false;
            }

            TileType selectedTile =
                candidates[_random.Next(candidates.Count)];

            AddTile(hand, usedCounts, selectedTile);
            AddTile(hand, usedCounts, selectedTile);
            AddTile(hand, usedCounts, selectedTile);

            return true;
        }
        
        private static TileType[] GetWindTiles()
        {
            return new[]
            {
                TileType.East,
                TileType.South,
                TileType.West,
                TileType.North
            };
        }

        private static TileType[] GetDragonTiles()
        {
            return new[]
            {
                TileType.White,
                TileType.Green,
                TileType.Red
            };
        }
    }
}