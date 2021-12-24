using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpeedupItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float distance;

    private Vector3 playerpoint;
    // Start is called before the first frame update
    void Start()
    {
        //開始時にプレイヤーを検索
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //プレイヤーを追尾
        playerpoint = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerpoint, Time.deltaTime * 5);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            //プレイヤーにぶつかると成長させるメソッドを呼び出し消滅
            other.transform.parent.gameObject.GetComponent<Player>().Fire_speedUp();
            Destroy(this.gameObject);
        }
    }
}
