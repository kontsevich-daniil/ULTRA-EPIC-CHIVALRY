using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Камера или объект игрока")]
    public Transform playerCamera;

    [Header("Центр взгляда (пустой объект)")]
    public Transform lookCenter;

    [Header("Ограничить вращение только по оси Y (для 2D спрайтов)")]
    public bool lockYRotation = true;

    void LateUpdate()
    {
        if (playerCamera == null || lookCenter == null)
            return;

        // Направление от спрайта к точке взгляда игрока
        Vector3 direction = lookCenter.position - transform.position;

        if (lockYRotation)
            direction.y = 0f; // фиксируем вертикальный поворот

        // Если направление не нулевое — поворачиваем
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}