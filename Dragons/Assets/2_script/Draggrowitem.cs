using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggrowitem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float distance;

    private Vector3 playerpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        playerpoint = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerpoint, Time.deltaTime*5);
    }
    private void OnTriggerEnter(Collider other)
    {
    
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent.gameObject.GetComponent<Player>().Draggrow();
            Destroy(this.gameObject);
        }
    }
}
