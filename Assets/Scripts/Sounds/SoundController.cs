using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sounds
{
    public class SoundController: MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _vfxSource;
        
        [SerializeField] private AudioClip _vfxStop;

        public void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void PauseMusic()
        {
            _musicSource.Pause();
            PlayVfx(_vfxStop);
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