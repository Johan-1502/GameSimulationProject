using UnityEngine;

public class DestroyCar : MonoBehaviour
{
    public Transform target;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    private void LateUpdate()
    {
        if (target.transform.position.x > 2.34 && !target.GetComponent<Player>().hasStopped())
            transform.position = new Vector3(target.position.x-4.47f, transform.position.y, transform.position.z);
        else
            transform.position = initialPosition;
        {
            
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Car"))
        {
            Car car = collision.GetComponent<Car>();
            if (car.getVelocityDirection() != 1)
                Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("Homeless"))
        {
            Destroy(collision.gameObject);
        }
    }
}
