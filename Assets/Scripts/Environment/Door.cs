using Data.Interfaces;
using UnityEngine;

namespace Environment
{
    public class Door: MonoBehaviour, IDestroyerObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public void DestroySelf()
        {
            spriteRenderer.enabled = false;
        }
    }
}