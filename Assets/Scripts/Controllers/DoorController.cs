using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorMesh;                  // Ссылка на саму дверь (мэш)
    public Vector3 openOffset = new Vector3(0, -3f, 0); // Насколько опускается дверь
    public float moveSpeed = 3f;                // Скорость открытия/закрытия

    [Header("Required Objects")]
    public GameObject[] objectsToDestroy;       // Объекты, которые нужно уничтожить для открытия двери

    private Vector3 closedPos;                  // Позиция в закрытом состоянии
    private Vector3 openPos;                    // Позиция в открытом состоянии
    private bool isOpening = false;             // Флаг состояния двери

    void Start()
    {
        // Сохраняем начальное положение двери
        if (doorMesh == null)
            doorMesh = transform;

        closedPos = doorMesh.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        // Плавное движение двери
        Vector3 target = isOpening && AllObjectsDestroyed() ? openPos : closedPos;
        doorMesh.position = Vector3.MoveTowards(doorMesh.position, target, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Проверяет, уничтожены ли все назначенные объекты
    /// </summary>
    private bool AllObjectsDestroyed()
    {
        if (objectsToDestroy == null || objectsToDestroy.Length == 0)
            return true; // если не назначено ни одного объекта — дверь сразу открывается

        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
                return false; // хотя бы один объект ещё жив
        }

        return true; // все уничтожены
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