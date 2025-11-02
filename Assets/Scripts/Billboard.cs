using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Камера, на которую должен смотреть спрайт
    private Transform cam;

    void Start()
    {
        // Берём главную камеру
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Разворачиваем спрайт лицом к камере
        transform.LookAt(transform.position + cam.rotation * Vector3.forward,
                         cam.rotation * Vector3.up);
    }
}
