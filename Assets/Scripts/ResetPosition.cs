using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

public class ResetPosition : MonoBehaviour, IUseable
{
    public int MAX_RANGE = 3;
    public Vector3 INIT_POSITION = new Vector3(0f, 0f, 0f);
    public float RotationSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.World);
    }

    public void UnUse(Hand controller)
    {
    }

    public void Use(Hand controller)
    {
        // teleport player onto one of the cloud
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            GameObject.Find("Player").transform.position = INIT_POSITION;
        }
    }
}
