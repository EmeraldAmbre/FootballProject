using UnityEngine;
using AudioManager.Public;
using FootballProject.Utilities;

namespace FootballProject.Audio {
    public abstract class AudioMaster<T> : SceneObjectSingleton<T> where T : Object {
        [SerializeField] protected AudioCollection audioCollection;
        public AudioManager.Public.AudioManager audioManager;

        protected bool isInitialized = false;
        public bool IsInitialized => isInitialized;

        protected override async void OnEnable  () {
            audioManager = new AudioManager.Public.AudioManager();
            await audioManager.LoadCollection(audioCollection);
            isInitialized = true;
        }

        protected virtual void OnDisable () {
            if (!isInitialized) {
                return;
            }

            audioManager.UnloadCollection();
        }
    }
}

