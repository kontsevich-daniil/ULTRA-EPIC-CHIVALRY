using Data.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Environment
{
    public class Door: MonoBehaviour, IDestroyerObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider doorCollider;
        [SerializeField] private NavMeshObstacle navMeshObstacle;
        public void DestroySelf()
        {
            spriteRenderer.enabled = false;
            doorCollider.enabled = false;
            navMeshObstacle.enabled = false;
        }
    }
}