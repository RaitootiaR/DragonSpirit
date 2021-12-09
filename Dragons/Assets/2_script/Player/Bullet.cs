using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed;
    private void Start()
    {
        BulletSpeed = Data.Data.PlayerBulletSpeed;
    }
    void Update()
    {
        transform.position = transform.position + transform.forward * BulletSpeed;
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(Data.GroundTag))
        {
            gameObject.SetActive(false);
        }
    }
}
