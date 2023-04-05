using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;

public class SlowButton : MonoBehaviour, IUseable
{
    private int MAX_RANGE = 5;
    public VisualHitTips VHT;
    public ChangeColor CC;
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
        Debug.Log("use slow button");
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            VHT.OnSlow();
            CC.ChangeColorButton();
        }
        
    }  
}
