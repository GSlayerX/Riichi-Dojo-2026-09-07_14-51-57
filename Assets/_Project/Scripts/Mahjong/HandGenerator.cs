using System;

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
                result.Waits);
        }
    }
}