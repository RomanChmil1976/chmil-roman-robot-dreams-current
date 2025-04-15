using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
       //Destroy(gameObject, 0.4f);
       Destroy(gameObject);
    }
}