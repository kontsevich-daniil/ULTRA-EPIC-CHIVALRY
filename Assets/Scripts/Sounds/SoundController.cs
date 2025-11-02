using System.Collections.Generic;
using Controllers;
using Cysharp.Threading.Tasks;
using Installers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sounds
{
    public class SoundController: MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private List<AudioClip> _musicsClip;
        
        [SerializeField] private AudioSource _vfxSource;
        [SerializeField] private AudioClip _vfxStop;

        private GameController _gameController;
        private LevelsController _levelsController;

        [Inject]
        private void Initialized(GameController gameController, LevelsController levelsController)
        {
            _gameController = gameController;
            _levelsController = levelsController;
            
            _gameController.LevelCompleted
                .Merge(_gameController.PlayerDied).
                Subscribe(_ =>
                {
                    StopMusic();
                })
                .AddTo(this);

            _gameController.LevelStart
                .Merge(_gameController.LevelRestart)
                .Subscribe(_ =>
                {
                    PlayMusic(_musicsClip[_levelsController.CurrentLevelIndex]);
                })
                .AddTo(this);

            PlayMusic(_musicsClip[_levelsController.CurrentLevelIndex]);
        }

        private void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void PauseMusic()
        {
            _musicSource.Pause();
            PlayVfx(_vfxStop);
        }

        private void StopMusic()
        {
            _musicSource.Stop();
        }

        public async void ResumeMusic()
        {
            PlayVfx(_vfxStop);
            await UniTask.WaitUntil(() => !_vfxSource.isPlaying);
            _musicSource.UnPause();
        }
        
        public void PlayVfx(AudioClip clip)
        {
            _vfxSource.PlayOneShot(clip);
        }
    }
}