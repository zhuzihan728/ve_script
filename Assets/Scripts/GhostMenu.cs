using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
// using Ubiq.Messaging;

[System.Serializable]
public class GhostMenu : MonoBehaviour
{
private int MAX_RANGE = 3;

    // private NetworkContext context;
    // public NetworkId NetworkId { get; set; }
    public bool isVisible;
    public int MenuIndex;
    
    // public struct Message
    // {
    //     public int MenuIndex;
    //     public bool isVisible;

    //     public Message(int MenuIndex, bool isVisible)
    //     {
    //         this.MenuIndex = MenuIndex;
    //         this.isVisible= isVisible;
    //     }
    // }

    private void Start()
    {
        // context = NetworkScene.Register(this);
    }

    private void Update()
    {   
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < MAX_RANGE){
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            isVisible = true;
        }
        else{
            transform.localScale = new Vector3(0, 0, 0);
            isVisible = false;
        }
        // context.SendJson(new Message(MenuIndex, isVisible));
    }

    // public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    // {
    //     var msg = message.FromJson<Message>(); 
    //     if(msg.MenuIndex == this.MenuIndex){
    //         this.isVisible = msg.isVisible;
    //         Debug.Log("msg received," + gameObject.name + "is set to" + this.isVisible);
    //         if (this.isVisible){
    //             transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    //         }
    //         else{
    //             transform.localScale = new Vector3(0, 0, 0);
    //         }
    //     }
    // }
}