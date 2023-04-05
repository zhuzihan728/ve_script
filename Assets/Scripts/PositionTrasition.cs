using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

public class PositionTrasition : MonoBehaviour, IUseable
{
    private Hand follow;
    private NetworkContext context;
    private bool isUsed;
    private Vector3 offset;
    private Collider colliderBox;
    public GameObject instrument;
    public int param; // 3 = Shakuhachi, 1 = Euphorium, 2 = Erhu
    public Hand holdingHand;

    public bool owner;

    public NetworkId NetworkId { get; set; }

    private void Start()
    {
        context = NetworkScene.Register(this);
        colliderBox = GetComponent<Collider>();
    }


    public void Attach(Hand hand)
    {
        isUsed = true;
        follow = hand;
        owner = true;
    }

    public void Detach(Hand hand)
    {
        isUsed = false;
        follow = null;
        owner = false;
    }


    private void Awake()
    {
        owner = false;
    }

    public void UnUse(Hand controller)
    {
        // if(controller != holdingHand){
        //     return;
        // }
        
    }

    public void Use(Hand controller)
    {
        Debug.Log("===================================");
        Debug.Log("the holding hand is " + holdingHand);
        Debug.Log("the input controller is " + controller);
        if (holdingHand == null){
            holdingHand = controller;
        }
        if(isUsed == false){
            // offset = instrument.transform.position - controller.transform.position;
            Attach(controller);
        }
        else if (isUsed == true && controller == holdingHand){
            Detach(controller);   
            holdingHand = null;
        }

    }

    public struct Message
    {
        public TransformMessage transform;
        public int Index;

        public Message(Transform transform, int Index)
        {
            this.transform = new TransformMessage(transform);
            this.Index = Index;
        }
    }
    private void Update()
    {
        if (follow != null)
        {
            instrument.transform.position = follow.transform.position;
            instrument.transform.rotation = follow.transform.rotation;
            context.SendJson(new Message(instrument.transform, param));
        }       
        if(owner)
            {
                context.SendJson(new Message(instrument.transform, param));
            }
        else
        {
        }
    }
     public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
        {
            var msg = message.FromJson<Message>();
            if(msg.Index != param){
                return;
            }
            instrument.transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
            instrument.transform.localRotation = msg.transform.rotation;
            
        }
}
