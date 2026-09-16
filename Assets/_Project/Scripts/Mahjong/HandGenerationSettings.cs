namespace RiichiDojo.Mahjong
{
    public readonly struct HandGenerationSettings
    {
        public int NumberOfSuits { get; }

        public int NoHonorsWeight { get; }

        public int WindsWeight { get; }

        public int DragonsWeight { get; }

        public int WindsAndDragonsWeight { get; }

        public int ChiitoitsuWeight { get; }

        public int KokushiWeight { get; }

        public int ChurenpotoWeight { get; }
        
        public HandGenerationSettings(
            int numberOfSuits,
            int noHonorsWeight,
            int windsWeight,
            int dragonsWeight,
            int windsAndDragonsWeight,
            int chiitoitsuWeight,
            int kokushiWeight,
            int churenpotoWeight)
        {
            NumberOfSuits = numberOfSuits;

            NoHonorsWeight = noHonorsWeight;
            WindsWeight = windsWeight;
            DragonsWeight = dragonsWeight;
            WindsAndDragonsWeight =
                windsAndDragonsWeight;

            ChiitoitsuWeight =
                chiitoitsuWeight;

            KokushiWeight =
                kokushiWeight;

            ChurenpotoWeight =
                churenpotoWeight;
        }
    }
}