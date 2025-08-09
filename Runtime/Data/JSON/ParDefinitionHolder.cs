using System;
using NEP.ScoreLab.Data;
using UnityEngine;

namespace NEP.ScoreLab.SDK
{
    public class ParDefinitionHolder : MonoBehaviour
    {
        public ParData ParData;
        [SerializeField] private ParDefinition _parDefinition;

        private void OnValidate()
        {
            if (_parDefinition == null)
            {
                return;
            }

            ParData = _parDefinition.Data;
        }
    }
}