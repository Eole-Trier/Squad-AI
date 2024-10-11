using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Duration = 2f;
    void Start()
    {
        Destroy(gameObject, Duration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Health healthComponent = collision.gameObject.GetComponentInParent<Health>();
        if (healthComponent == null)
            healthComponent = collision.gameObject.GetComponent<Health>();
        healthComponent?.AddDamage(1);

        Destroy(gameObject);
    }
}
