using Data.Interfaces;
using UnityEngine;

namespace Environment
{
    public class Door: MonoBehaviour, IDestroyerObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider doorCollider;
        public void DestroySelf()
        {
            spriteRenderer.enabled = false;
            doorCollider.enabled = false;
        }
    }
}