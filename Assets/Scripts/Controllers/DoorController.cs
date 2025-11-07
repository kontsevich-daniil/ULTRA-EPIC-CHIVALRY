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
        if (doorMesh == null)
            doorMesh = transform;

        closedPos = doorMesh.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (!isOpening && AllObjectsDestroyed())
        {
            isOpening = true;
        }
        
        Vector3 target = isOpening ? openPos : closedPos;
        doorMesh.position = Vector3.MoveTowards(doorMesh.position, target, moveSpeed * Time.deltaTime);
    }
    
    private bool AllObjectsDestroyed()
    {
        if (objectsToDestroy == null || objectsToDestroy.Length == 0)
            return true;

        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
                return false;
        }

        return true;
    }
    
    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpening = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpening = false;
    }*/
}