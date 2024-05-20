using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     public float Lifetime = 0.1f; // Adjust this value for the maximum lifetime of the bullet

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBullet();
    }

    void Start()
    {
        // Schedule the destruction of the bullet after its maximum lifetime
        Destroy(gameObject, Lifetime);
    }

    void DestroyBullet()
    {
        // Destroy the bullet game object
        Destroy(gameObject);
    }
}