using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [Header("Settings")]
    public float bobFrequency = 6f;      // Частота покачивания (скорость шагов)
    public float bobAmplitude = 1.5f;    // Амплитуда (в градусах)
    public float smooth = 8f;            // Скорость сглаживания возврата к покою

    [Header("References")]
    public PlayerMovement playerController;

    private Quaternion startRot;
    private float timer;

    void Start()
    {
        startRot = transform.localRotation;
    }

    void Update()
    {
        if (playerController != null && playerController.isMoving)
        {
            timer += Time.deltaTime * bobFrequency;

            // Угол покачивания вокруг оси Z (наклон головы)
            float roll = Mathf.Sin(timer) * bobAmplitude; 

            // Можно добавить лёгкое покачивание вверх-вниз (pitch) если хочешь
            //float pitch = Mathf.Cos(timer * 2f) * bobAmplitude * 0.3f;

            // Целевой поворот
            Quaternion targetRot = startRot * Quaternion.Euler(0, 0f, roll * 3f);

            // Плавное движение к целевому повороту
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * smooth);
        }
        else
        {
            // Возврат в исходное положение
            timer = 0f;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(startRot.x, startRot.y, 0), Time.deltaTime * smooth);
        }
    }
}