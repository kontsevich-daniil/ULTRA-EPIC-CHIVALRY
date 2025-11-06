using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Controllers
{
    public class LifeCycleController : MonoBehaviour
    {
        private GameController _gameController;
        
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        private float _transitionDuration = 0.5f;

        private bool _IsPaused = false;

        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                if (_IsPaused)
                    Resume().Forget();
                else
                    Pause().Forget();
            }
        }

        private async UniTask Pause()
        {
            if (_IsPaused) 
                return;
            
            _IsPaused = true;
            _gameController.GameInPause.Execute();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            float start = Time.timeScale;
            float elapsed = 0f;

            while (elapsed < _transitionDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / _transitionDuration;
                Time.timeScale = Mathf.Lerp(start, 0f, t);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            Time.timeScale = 0f;
        }

        private async UniTask Resume()
        {
            if (!_IsPaused) 
                return;
            
            _IsPaused = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float elapsed = 0f;
            float start = Time.timeScale;

            while (elapsed < _transitionDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / _transitionDuration;
                Time.timeScale = Mathf.Lerp(start, 1f, t);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            _gameController.GameInResume.Execute();
            Time.timeScale = 1f;
        }
    }
}