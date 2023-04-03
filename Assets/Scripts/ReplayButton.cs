using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

public class ReplayButton : MonoBehaviour, IUseable
{
    private int MAX_RANGE = 3;
    public RecordPlayback record_playback;

    // Start is called before the first frame update
    void Start()
    {
        record_playback = FindObjectOfType<RecordPlayback>();
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
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            record_playback.Replay();
        }
    }

}
