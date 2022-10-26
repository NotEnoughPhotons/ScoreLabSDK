using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.Core
{
    public class Emulator : MonoBehaviour
    {
        public int Score;
        public float Multiplier;

        public List<Data.PackedValue> ActiveValues;
        public List<Data.PackedValue> ClonedValues;

        public List<AssetBundle> bundles;
        public List<GameObject> loadedStuff;
        public List<string> uiNames;

        public JSONScore[] testScores;
        public JSONMult[] testMultipliers;

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

            testScores = DataManager.PackedValues.Scores;
            testMultipliers = DataManager.PackedValues.Multipliers;

            bundles = DataManager.Bundle.Bundles;
            loadedStuff = DataManager.UI.LoadedUIObjects;
            uiNames = DataManager.UI.UINames;

            ScoreTracker.Instance.Update();

            Score = ScoreTracker.Instance.Score;
            Multiplier = ScoreTracker.Instance.Multiplier;

            ActiveValues = ScoreTracker.Instance.ActiveValues;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScoreTracker.Instance.Add(Data.EventType.Score.Kill);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                ScoreTracker.Instance.Add(Data.EventType.Mult.Test);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ScoreTracker.Instance.Add(Data.EventType.Mult.Kill);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                ScoreTracker.Instance.Add(Data.EventType.Mult.Test2);
            }
        }
    }
}

