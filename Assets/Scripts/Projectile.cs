using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Animal"))
        {
            if (hitEffect) Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            GameManager.Instance.CatchAnimal();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}