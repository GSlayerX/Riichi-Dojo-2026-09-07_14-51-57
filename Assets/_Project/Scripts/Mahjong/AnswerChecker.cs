using System.Collections.Generic;
using System.Linq;

namespace RiichiDojo.Mahjong
{
    public class AnswerChecker
    {
        public bool IsCorrect(
            TrainingQuestion question,
            IReadOnlyList<TileType> selectedTiles)
        {
            if (question == null || selectedTiles == null)
            {
                return false;
            }

            return question.Waits.Count == selectedTiles.Count
                   && question.Waits
                       .OrderBy(tile => tile)
                       .SequenceEqual(
                           selectedTiles.OrderBy(tile => tile));
        }
    }
}