using System;
using Controllers;
using Cysharp.Threading.Tasks;
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
        private GameController _gameController;

        public VideoPlayer videoPlayer;
        public FadeController fadeController;
        public Image cutsceneImage;
        public TMP_Text timerText;

        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
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
            
            _gameController.Timer
                .Subscribe(timer => timerText.text = timer.ToString())
                .AddTo(this);
        }

        private async UniTaskVoid LevelCompleted()
        {
            await fadeController.FadeOut();
            //await PlayVideo();
            _gameController.LevelStart.Execute();
        }

        private IObservable<Unit> PlayVideo()
        {
            return Observable.Create<Unit>(obserwable =>
            {
                videoPlayer.loopPointReached += OnVideoEnd;
                videoPlayer.Play();

                void OnVideoEnd(VideoPlayer video)
                {
                    video.loopPointReached -= OnVideoEnd;
                    obserwable.OnCompleted();
                }

                return Disposable.Create(() =>
                {
                    videoPlayer.loopPointReached -= OnVideoEnd;
                    videoPlayer.Stop();
                });
            });
        }
    }
}