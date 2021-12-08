using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemspawn : MonoBehaviour
{
    [SerializeField]
    GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PowerupItems()
    {
        Instantiate(item);
    }
}
