using System;
using System.Collections.Generic;

using NEP.ScoreLab.Data;
using NEP.ScoreLab.UI;

namespace NEP.ScoreLab.Core
{
    public static class API
    {
        public static class Score
        {
            public static Action<PackedScore> OnScoreAdded;
            public static Action<PackedScore> OnScoreAccumulated;
            public static Action<PackedScore> OnScoreRemoved;
            public static Action<PackedScore> OnScoreTierReached;

            public static Action<int> OnScoreUpdated;
            public static Action<int> OnScoreDifferenceUpdated;
            public static Action<int> OnLastScoreUpdated;
        }

        public static class Multiplier
        {
            public static Action<PackedMultiplier> OnMultiplierAdded;
            public static Action<PackedMultiplier> OnMultiplierAccumulated;
            public static Action<PackedMultiplier> OnMultiplierRemoved;
            public static Action<PackedMultiplier> OnMultiplierTierReached;

            public static Action<PackedMultiplier> OnMultiplierTimeBegin;
            public static Action<PackedMultiplier> OnMultiplierTimeExpired;
        }

        public static class HighScore
        {
            public static Action<PackedHighScore> OnHighScoreReached;
            public static Action<PackedHighScore> OnHighScoreUpdated;
            public static Action<PackedHighScore> OnHighScoreLoaded;
            public static Action<PackedHighScore> OnHighScoreSaved;
        }

        public static class GameConditions
        {
            private static Dictionary<string, Func<bool>> _conditionTable = new Dictionary<string, Func<bool>>()
            {
                { "IsPlayerMoving", new Func<bool>(() => true) },
                { "IsPlayerInAir", new Func<bool>(() => Emulator._testCondition) }
            };

            public static Func<bool> GetCondition(string cond)
            {
                if (string.IsNullOrEmpty(cond))
                {
                    UnityEngine.Debug.LogWarning($"{cond} doesn't exist in the condition table.");
                    return null;
                }
                else if(!_conditionTable.ContainsKey(cond))
                {
                    UnityEngine.Debug.LogWarning($"{cond} doesn't exist in the condition table.");
                    return null;
                }
                return _conditionTable[cond];
            }
        }

        public static class UI
        {
            public static Action<UIModule> OnModuleEnabled;
            public static Action<UIModule> OnModuleDisabled;

            public static Action<UIModule> OnModuleDecayed;
            public static Action<UIModule> OnModulePostDecayed;
        }

#if UNITY_EDITOR
        public static class Editor
        {
            public static Action OnEditorModuleShow;
        }
#endif
    }
}

