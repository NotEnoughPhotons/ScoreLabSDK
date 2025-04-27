using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

using System.Collections.Generic;

namespace NEP.ScoreLab.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private List<GameObject> _pooledObjects;

        private void Awake()
        {
            _pooledObjects = new List<GameObject>();

            GameObject list = new GameObject("Pooled Audio");
            list.transform.parent = transform;

            for(int i = 0; i < 64; i++)
            {
                GameObject pooledAudio = new GameObject("Poolee Audio");
                pooledAudio.transform.parent = list.transform;

                AudioSource source = pooledAudio.AddComponent<AudioSource>();
                source.playOnAwake = true;
                source.volume = 5f;

                pooledAudio.AddComponent<PooledAudio>();
                pooledAudio.SetActive(false);
                _pooledObjects.Add(pooledAudio);
            }
        }

        private void OnEnable()
        {
            API.Value.OnValueAdded += OnValueReceived;
            API.Value.OnValueTierReached += OnValueReceived;
        }

        private void OnDisable()
        {
            API.Value.OnValueAdded -= OnValueReceived;
            API.Value.OnValueTierReached -= OnValueReceived;
        }

        private void OnValueReceived(PackedValue value)
        {
            if (!Settings.UseAnnouncer)
            {
                return;
            }
            
            if(value.EventAudio != null)
            {
                Play(value.EventAudio);
            }
        }

        public void Play(AudioClip clip)
        {

        }

        private GameObject GetFirstInactive()
        {
            for(int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            return null;
        }
    }
}
