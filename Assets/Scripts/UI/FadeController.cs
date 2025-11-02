using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeController: MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private readonly Subject<Unit> _fadeSubject = new();
        private IObservable<Unit> Fade => _fadeSubject;
        public IObservable<Unit> FadeOut()
        {
            animator.Play("FadeOut");
            return Fade.Take(1);
        }

        public IObservable<Unit> FadeIn()
        {
            animator.Play("FadeIn");
            return Fade.Take(1);
        }

        private void OnAnimationComplete()
        {
            _fadeSubject.OnNext(Unit.Default);
        }
    }
}