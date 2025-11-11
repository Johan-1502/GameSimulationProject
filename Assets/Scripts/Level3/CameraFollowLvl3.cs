using UnityEngine;

public class CameraFollowLvl3 : MonoBehaviour
{
    public GameObject target;

    private void LateUpdate()
    {
        if (target.transform.position.x > 0.1)
            transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
    }
}
