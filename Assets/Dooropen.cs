using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorMesh;         // Ссылка на саму дверь (мэш)
    public Vector3 openOffset = new Vector3(0, 3f, 0); // Насколько поднимается дверь
    public float moveSpeed = 3f;       // Скорость открытия/закрытия

    private Vector3 closedPos;         // Позиция в закрытом состоянии
    private Vector3 openPos;           // Позиция в открытом состоянии
    private bool isOpening = false;    // Флаг состояния двери

    void Start()
    {
        // Сохраняем начальное положение двери
        if (doorMesh == null)
            doorMesh = transform; // если забыли указать вручную

        closedPos = doorMesh.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        // Плавное перемещение двери между состояниями
        Vector3 target = isOpening ? openPos : closedPos;
        doorMesh.position = Vector3.MoveTowards(doorMesh.position, target, moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpening = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpening = false;
    }
}
