using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Core
{
    public class Emulator : MonoBehaviour
    {
        public int Score;
        public float Multiplier;
        public List<Data.PackedMultiplier> ActiveMultipliers;

        public List<AssetBundle> bundles;
        public List<GameObject> loadedStuff;
        public List<string> uiNames;

        public bool testCondition;
        public static bool _testCondition;

        public Func<bool> TestFunc;

        private void Awake()
        {
            Data.DataManager.Init();
            new ScoreTracker();
            TestFunc = new Func<bool>(() => _testCondition);
        }

        private void Update()
        {
            _testCondition = testCondition;

            bundles = Data.DataManager.Bundle.Bundles;
            loadedStuff = Data.DataManager.UI.LoadedUIObjects;
            uiNames = Data.DataManager.UI.UINames;

            ScoreTracker.Instance.Update();

            Score = ScoreTracker.Instance.Score;
            Multiplier = ScoreTracker.Instance.Multiplier;
            ActiveMultipliers = ScoreTracker.Instance.ActiveMultipliers;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScoreTracker.Instance.Add(new Data.PackedScore("Test", 100));
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                ScoreTracker.Instance.Add(new Data.PackedMultiplier("Test", 100, API.GameConditions.IsPlayerInAir));
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ScoreTracker.Instance.Add(new Data.PackedMultiplier("Test", 100, 5f));
            }
        }
    }
}

