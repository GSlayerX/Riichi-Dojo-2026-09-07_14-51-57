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

        public MahjongHand GenerateHand(
            HandGenerationSettings settings)
        {
            HandGenerationType generationType =
                SelectHandGenerationType(settings);

            switch (generationType)
            {
                case HandGenerationType.Standard:
                    return GenerateRandomStandardTenpaiHand(settings);

                case HandGenerationType.Chiitoitsu:
                    return GenerateRandomChiitoitsuHand(settings);

                case HandGenerationType.Kokushi:
                    return GenerateRandomKokushiHand();

                case HandGenerationType.Churenpoto:
                    return GenerateChurenpotoHand();

                default:
                    throw new InvalidOperationException(
                        $"Unsupported hand generation type: {generationType}");
            }
        }

        public MahjongHand GenerateHand(HandPattern pattern)
        {
            if (pattern == HandPattern.Chiitoitsu)
            {
                return GenerateRandomChiitoitsuHand(
                    new HandGenerationSettings(
                        numberOfSuits: 3,
                        noHonorsWeight: 100,
                        windsWeight: 0,
                        dragonsWeight: 0,
                        windsAndDragonsWeight: 0,
                        chiitoitsuWeight: 0,
                        kokushiWeight: 0,
                        churenpotoWeight: 0));
            }

            if (pattern == HandPattern.Kokushi)
            {
                return GenerateRandomKokushiHand();
            }

            if (pattern == HandPattern.Churenpoto)
            {
                return GenerateChurenpotoHand();
            }
            
            var settings =
                new HandGenerationSettings(
                    numberOfSuits: 3,
                    noHonorsWeight: 100,
                    windsWeight: 0,
                    dragonsWeight: 0,
                    windsAndDragonsWeight: 0,
                    chiitoitsuWeight: 0,
                    kokushiWeight: 0,
                    churenpotoWeight: 0);

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

                int attempts = 0;
                bool failed = false;

                for (int i = 0; i < 4; i++)
                {
                    attempts++;
                    // Защита от бесконечного зацикливания:
                    // Если за 30 шагов сеты не сошлись, сбрасываем руку и генерируем заново
                    if (attempts > 30)
                    {
                        failed = true;
                        break;
                    }

                    bool added;

                    if (_random.NextDouble() < 0.5)
                    {
                        added = TryAddRandomTriplet(
                            hand,
                            usedCounts,
                            selectedSuits,
                            honorConfiguration);

                        if (!added)
                        {
                            added = TryAddRandomSequence(
                                hand,
                                usedCounts,
                                selectedSuits);
                        }
                    }
                    else
                    {
                        added = TryAddRandomSequence(
                            hand,
                            usedCounts,
                            selectedSuits);

                        if (!added)
                        {
                            added = TryAddRandomTriplet(
                                hand,
                                usedCounts,
                                selectedSuits,
                                honorConfiguration);
                        }
                    }

                    if (!added)
                    {
                        i--;
                    }
                }

                if (!failed && hand.TileCount == 14)
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

            // Перемешиваем масти для равномерного выбора
            var shuffledSuits = new List<int>(selectedSuits);
            for (int i = shuffledSuits.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (shuffledSuits[i], shuffledSuits[j]) = (shuffledSuits[j], shuffledSuits[i]);
            }

            foreach (int suit in shuffledSuits)
            {
                var validRanks = new List<int>();

                // Находим все ранги от 0 до 6, у которых есть доступные тайлы
                for (int startRank = 0; startRank < 7; startRank++)
                {
                    int first = suit * 9 + startRank;
                    if (usedCounts[first] < 4 &&
                        usedCounts[first + 1] < 4 &&
                        usedCounts[first + 2] < 4)
                    {
                        validRanks.Add(startRank);
                    }
                }

                if (validRanks.Count > 0)
                {
                    int startRank = validRanks[_random.Next(validRanks.Count)];
                    int first = suit * 9 + startRank;

                    AddTile(hand, usedCounts, (TileType)first);
                    AddTile(hand, usedCounts, (TileType)(first + 1));
                    AddTile(hand, usedCounts, (TileType)(first + 2));

                    return true;
                }
            }

            return false;
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

            // Случайный выбор одной из трёх числовых мастей (Ман, Пин или Со)
            int suit = _random.Next(3);
            int firstTile = suit * 9;

            // 1-1-1
            hand.AddTile((TileType)firstTile);
            hand.AddTile((TileType)firstTile);
            hand.AddTile((TileType)firstTile);

            // 2-3-4-5-6-7-8
            for (int rank = 1; rank <= 7; rank++)
            {
                hand.AddTile((TileType)(firstTile + rank));
            }

            // 9-9-9
            int lastTile = firstTile + 8;
            hand.AddTile((TileType)lastTile);
            hand.AddTile((TileType)lastTile);
            hand.AddTile((TileType)lastTile);

            return hand;
        }

        private MahjongHand GenerateRandomChiitoitsuHand(
            HandGenerationSettings settings)
        {
            var hand = new MahjongHand();

            var selectedSuits =
                SelectSuits(settings.NumberOfSuits);

            HonorConfiguration honorConfiguration =
                HonorConfiguration.None;

            if (settings.NoHonorsWeight +
                settings.WindsWeight +
                settings.DragonsWeight +
                settings.WindsAndDragonsWeight > 0)
            {
                honorConfiguration =
                    SelectHonorConfiguration(settings);
            }

            var availableTiles =
                new List<TileType>();

            foreach (int suit in selectedSuits)
            {
                int firstTile = suit * 9;

                for (int rank = 0; rank < 9; rank++)
                {
                    availableTiles.Add(
                        (TileType)(firstTile + rank));
                }
            }

            if (honorConfiguration == HonorConfiguration.Winds ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                availableTiles.Add(TileType.East);
                availableTiles.Add(TileType.South);
                availableTiles.Add(TileType.West);
                availableTiles.Add(TileType.North);
            }

            if (honorConfiguration == HonorConfiguration.Dragons ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                availableTiles.Add(TileType.White);
                availableTiles.Add(TileType.Green);
                availableTiles.Add(TileType.Red);
            }

            var selectedTiles =
                new List<TileType>();

            foreach (int suit in selectedSuits)
            {
                int firstTile = suit * 9;
                int rank = _random.Next(9);

                selectedTiles.Add(
                    (TileType)(firstTile + rank));
            }
            
            if (honorConfiguration == HonorConfiguration.Winds ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                var winds = new[]
                {
                    TileType.East,
                    TileType.South,
                    TileType.West,
                    TileType.North
                };

                selectedTiles.Add(
                    winds[_random.Next(winds.Length)]);
            }

            if (honorConfiguration == HonorConfiguration.Dragons ||
                honorConfiguration == HonorConfiguration.WindsAndDragons)
            {
                var dragons = new[]
                {
                    TileType.White,
                    TileType.Green,
                    TileType.Red
                };

                selectedTiles.Add(
                    dragons[_random.Next(dragons.Length)]);
            }

            while (selectedTiles.Count < 7)
            {
                TileType tile =
                    availableTiles[
                        _random.Next(availableTiles.Count)];

                if (!selectedTiles.Contains(tile))
                {
                    selectedTiles.Add(tile);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                hand.AddTile(selectedTiles[i]);
                hand.AddTile(selectedTiles[i]);
            }

            hand.AddTile(selectedTiles[6]);

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
        
        private HandGenerationType SelectHandGenerationType(
            HandGenerationSettings settings)
        {
            int specialWeight =
                settings.ChiitoitsuWeight +
                settings.KokushiWeight +
                settings.ChurenpotoWeight;

            // Безопасный расчет веса стандартной руки без краша при сумме > 100
            int standardWeight = Math.Max(0, 100 - specialWeight);
            int totalWeight = standardWeight + specialWeight;

            if (totalWeight <= 0)
            {
                return HandGenerationType.Standard;
            }

            int roll = _random.Next(totalWeight);

            if (roll < standardWeight)
            {
                return HandGenerationType.Standard;
            }

            roll -= standardWeight;

            if (roll < settings.ChiitoitsuWeight)
            {
                return HandGenerationType.Chiitoitsu;
            }

            roll -= settings.ChiitoitsuWeight;

            if (roll < settings.KokushiWeight)
            {
                return HandGenerationType.Kokushi;
            }

            return HandGenerationType.Churenpoto;
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