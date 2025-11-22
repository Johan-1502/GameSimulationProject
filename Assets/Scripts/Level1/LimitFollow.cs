using UnityEngine;

public class SuperiorLimitFollow : MonoBehaviour
{
    public Transform target;
    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bottle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
