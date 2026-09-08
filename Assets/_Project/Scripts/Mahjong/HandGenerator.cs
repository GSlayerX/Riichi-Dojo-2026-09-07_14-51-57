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
            while (true)
            {
                var hand = GenerateRandomHand();

                var result = _calculator.CalculateWaits(hand);

                if (result.Waits.Count > 0)
                {
                    return hand;
                }
            }
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
            
            while (true)
            {
                var hand = GenerateRandomHand();

                var result = _calculator.CalculateWaits(hand);

                if (result.Patterns.Contains(pattern))
                {
                    return hand;
                }
            }
        }
        
        private MahjongHand GenerateRandomHand()
        {
            var hand = new MahjongHand();

            while (hand.TileCount < HandSize)
            {
                TileType tile = (TileType)_random.Next(TileTypeCount);

                if (hand.GetCount(tile) >= 4)
                {
                    continue;
                }

                hand.AddTile(tile);
            }

            return hand;
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
        
        public TrainingQuestion GenerateQuestion(HandPattern pattern)
        {
            var hand = GenerateHand(pattern);

            var result = _calculator.CalculateWaits(hand);

            return new TrainingQuestion(
                hand,
                result.Waits,
                result.Patterns);
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

            var selectedTiles = new System.Collections.Generic.HashSet<TileType>();

            while (selectedTiles.Count < 6)
            {
                TileType tile = (TileType)_random.Next(TileTypeCount);

                if (selectedTiles.Add(tile))
                {
                    hand.AddTile(tile);
                    hand.AddTile(tile);
                }
            }

            TileType singleTile;

            do
            {
                singleTile = (TileType)_random.Next(TileTypeCount);
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

        public MahjongHand GenerateRandomKokushiHand(KokushiWaitType waitType)
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
                terminalHonors[_random.Next(terminalHonors.Length)];

            TileType removedTile;

            do
            {
                removedTile =
                    terminalHonors[_random.Next(terminalHonors.Length)];
            }
            while (removedTile == duplicateTile);

            hand.RemoveTile(removedTile);
            hand.AddTile(duplicateTile);

            return hand;
        }
    }
}