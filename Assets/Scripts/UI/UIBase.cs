using System;
using System.Collections.Generic;
using Controllers;
using Cysharp.Threading.Tasks;
using Installers;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Zenject;
using Image = UnityEngine.UIElements.Image;
using Unit = UniRx.Unit;

namespace UI
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;   
        
        private GameController _gameController;
        private LevelsController _levelsController;

        public VideoPlayer videoPlayer;
        public List<VideoClip> videoClips;
        public FadeController fadeController;
        public Image cutsceneImage;
        public TMP_Text timerText;

        [Inject]
        private void Initialized(GameController gameController, LevelsController levelsController)
        {
            _gameController = gameController;
            _levelsController = levelsController;
        }

        public void Awake()
        {
            _gameController.LevelCompleted.Subscribe(_ =>
            {
                LevelCompleted().Forget();
            }).AddTo(this);
            
            _gameController.LevelStart.Merge(_gameController.LevelRestart).Subscribe(_ =>
            {
                fadeController.FadeIn();
            }).AddTo(this);
            
            _gameController.PlayerDied.Subscribe(_ =>
            {
                fadeController.FadeOut();
            }).AddTo(this);
            
            _gameController.GameInPause.Subscribe(_ =>
            {
                _pausePanel.SetActive(true);
            }).AddTo(this);
            
            _gameController.GameInResume.Subscribe(_ =>
            {
                fadeController.FadeIn();
            }).AddTo(this);
            
            _gameController.Timer
                .Subscribe(timer => timerText.text = timer.ToString())
                .AddTo(this);
        }

        private async UniTaskVoid LevelCompleted()
        {
            await fadeController.FadeOut();
            await PlayVideo();
            _gameController.LevelStart.Execute();
        }

        private IObservable<Unit> PlayVideo()
        {
            return Observable.Create<Unit>(obserwable =>
            {
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.clip = videoClips[_levelsController.CurrentLevelIndex - 1];
                videoPlayer.loopPointReached += OnVideoEnd;
                videoPlayer.Play();

                void OnVideoEnd(VideoPlayer video)
                {
                    video.loopPointReached -= OnVideoEnd;
                    obserwable.OnNext(Unit.Default);
                    obserwable.OnCompleted();
                }

                return Disposable.Create(() =>
                {
                    videoPlayer.loopPointReached -= OnVideoEnd;
                    videoPlayer.Stop();
                    videoPlayer.gameObject.SetActive(false);
                });
            });
        }
    }
}