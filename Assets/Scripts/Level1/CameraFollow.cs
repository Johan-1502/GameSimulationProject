using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    private void LateUpdate()
    {
        if (target.transform.position.x > 0.1 && !target.GetComponent<Player>().hasStopped())
            transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

    }
}
