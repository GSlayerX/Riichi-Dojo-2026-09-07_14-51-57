namespace RiichiDojo.Mahjong
{
    public class StandardHandSolver
    {
        private const int TileTypeCount = 34;

        public bool IsWinningHand(MahjongHand hand)
        {
            if (hand.TileCount != 14)
            {
                return false;
            }

            int[] counts = CreateCounts(hand);

            // Перебираем возможную пару.
            for (int i = 0; i < TileTypeCount; i++)
            {
                if (counts[i] < 2)
                {
                    continue;
                }

                // Временно убираем пару.
                counts[i] -= 2;

                if (CanFormMelds(counts, 4))
                {
                    return true;
                }

                // Возвращаем пару обратно.
                counts[i] += 2;
            }

            return false;
        }

        private int[] CreateCounts(MahjongHand hand)
        {
            int[] counts = new int[TileTypeCount];

            for (int i = 0; i < TileTypeCount; i++)
            {
                counts[i] = hand.GetCount((TileType)i);
            }

            return counts;
        }

        private bool CanFormMelds(int[] counts, int meldsRemaining)
        {
            // Все 4 мэлда уже сформированы.
            if (meldsRemaining == 0)
            {
                return true;
            }

            // Находим первый оставшийся тайл.
            int firstTile = -1;

            for (int i = 0; i < TileTypeCount; i++)
            {
                if (counts[i] > 0)
                {
                    firstTile = i;
                    break;
                }
            }

            // Тайлов не осталось, но мэлды ещё нужны.
            if (firstTile == -1)
            {
                return false;
            }

            // Попробуем сформировать триплет.
            if (counts[firstTile] >= 3)
            {
                counts[firstTile] -= 3;

                if (CanFormMelds(counts, meldsRemaining - 1))
                {
                    return true;
                }

                counts[firstTile] += 3;
            }

            // Попробуем сформировать последовательность.
            if (CanFormSequence(counts, firstTile))
            {
                counts[firstTile]--;
                counts[firstTile + 1]--;
                counts[firstTile + 2]--;

                if (CanFormMelds(counts, meldsRemaining - 1))
                {
                    return true;
                }

                counts[firstTile]++;
                counts[firstTile + 1]++;
                counts[firstTile + 2]++;
            }

            return false;
        }

        private bool CanFormSequence(int[] counts, int firstTile)
        {
            // Почести не могут образовывать последовательности.
            if (firstTile >= 27)
            {
                return false;
            }

            // 8 и 9 не могут быть началом последовательности.
            int rank = firstTile % 9;

            if (rank > 6)
            {
                return false;
            }

            return counts[firstTile + 1] > 0
                   && counts[firstTile + 2] > 0;
        }
    }
}