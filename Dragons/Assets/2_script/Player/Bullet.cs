using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        transform.position = transform.position + transform.forward * Data.Data.PlayerBulletSpeed;
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
