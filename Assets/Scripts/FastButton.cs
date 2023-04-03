using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;

public class FastButton : MonoBehaviour, IUseable
{
    private int MAX_RANGE = 5;
    public VisualHitTips VHT;
    public StartStopControl SSC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnUse(Hand controller)
    {
    }

    public void Use(Hand controller)
    {
        // check player's position
        Debug.Log("use fast button");
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            // if(VHT.tempo > 0.5f && SSC.isRecord == false){
            if(VHT.tempo > 0.5f){
                VHT.tempo -= 0.05f;
            }
        }
    }  
}
