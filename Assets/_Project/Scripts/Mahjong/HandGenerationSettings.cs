namespace RiichiDojo.Mahjong
{
    public readonly struct HandGenerationSettings
    {
        public int NumberOfSuits { get; }

        public int NoHonorsWeight { get; }

        public int WindsWeight { get; }

        public int DragonsWeight { get; }

        public int WindsAndDragonsWeight { get; }

        public HandGenerationSettings(
            int numberOfSuits,
            int noHonorsWeight,
            int windsWeight,
            int dragonsWeight,
            int windsAndDragonsWeight)
        {
            NumberOfSuits = numberOfSuits;
            NoHonorsWeight = noHonorsWeight;
            WindsWeight = windsWeight;
            DragonsWeight = dragonsWeight;
            WindsAndDragonsWeight =
                windsAndDragonsWeight;
        }
    }
}