using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

[System.Serializable]
public class Ghost : MonoBehaviour
{
    private int MAX_RANGE = 5;

    private NetworkContext context;
    public NetworkId NetworkId { get; set; }

    // public GameObject visual;
    public GameObject[] DS;
    
    private void Start()
    {
        context = NetworkScene.Register(this);
        // visual = GameObject.Find("ShowUpMenu");
        // visual.SetActive(false);
        DS= GameObject.FindGameObjectsWithTag("Ghost");

    }

    private void Update()
    {   

        foreach (GameObject D in DS)
        {   
            if(Vector3.Distance(GameObject.Find("Player").transform.position, D.transform.position) < MAX_RANGE){
                GetComponent<Renderer>().enabled = true;
                // D.SetActive(true);
            }
            else{
                GetComponent<Renderer>().enabled = false;
                // D.SetActive(false);
            }
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {


    }
}
