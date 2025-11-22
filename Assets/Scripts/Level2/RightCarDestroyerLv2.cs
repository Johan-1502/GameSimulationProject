using UnityEngine;

public class RightCarDestroyerLv2 : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x+4.47f, transform.position.y, transform.position.z);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Car"))
        {
            Car car = collision.GetComponent<Car>();
            if (car.getVelocityDirection() == 1)
                Destroy(collision.gameObject);
        }
    }
}
