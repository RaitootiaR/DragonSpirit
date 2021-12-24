using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	// Start is called before the first frame update
	
	private void OnTriggerStay(Collider other)
	{

		if (other.tag=="Player")
		{
			Debug.Log("当たったよ");
			other.gameObject.transform.root.gameObject.GetComponent<Player>().Damaged();
		}
	}

}
