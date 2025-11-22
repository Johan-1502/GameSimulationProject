using UnityEngine;

public class EndDestroyerLv2 : MonoBehaviour
{
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
