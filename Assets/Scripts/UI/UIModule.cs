using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.UI
{
    public class UIModule : MonoBehaviour
    {
        protected bool _canDecay { get; private set; }
        protected float _decayTime { get; private set; }
        protected float _postDecayTime { get; private set; }

        protected float _tDecay { get; private set; }
        protected float _tPostDecay { get; private set; }

        public virtual void OnModuleEnable() { }

        public virtual void OnModuleDisable() { }

        public virtual void OnUpdate() { }

        public void CanDecay(bool decay)
        {
            this._canDecay = decay;
        }

        public void SetDecayTime(float decayTime)
        {
            this._decayTime = decayTime;
            this._tDecay = this._decayTime;
        }

        public void SetPostDecayTime(float postDecayTime)
        {
            this._postDecayTime = postDecayTime;
            this._tPostDecay = this._postDecayTime;
        }

        protected void UpdateDecayTimers()
        {
            if (!_canDecay)
            {
                return;
            }

            if(_tDecay < 0f)
            {
                _tPostDecay -= Time.deltaTime;

                print($"Post-Decay: {_tPostDecay}");

                if (_tPostDecay < 0f)
                {
                    _tDecay = _decayTime;
                    _tPostDecay = _postDecayTime;
                    gameObject.SetActive(false);
                    return;
                }

                return;
            }

            _tDecay -= Time.deltaTime;
            print($"Decay: {_tDecay}");
        }

        private void OnEnable() => OnModuleEnable();
        private void OnDisable() => OnModuleEnable();
        private void Update() => OnUpdate();
    }
}