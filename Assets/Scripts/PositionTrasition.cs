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
    }

    public void Use(Hand controller)
    {
        if(isUsed == false){
            // offset = instrument.transform.position - controller.transform.position;
            Attach(controller);
        }
        else{
            Detach(controller);   
        }

    }

    public struct Message
    {
        public TransformMessage transform;
        public Message(Transform transform)
        {
            this.transform = new TransformMessage(transform);
        }
    }
    private void Update()
    {
        if (follow != null)
        {
            instrument.transform.position = follow.transform.position;
            instrument.transform.rotation = follow.transform.rotation;
            context.SendJson(new Message(transform));
        }       
        if(owner)
            {
                context.SendJson(new Message(transform));
            }
        else
        {
        }
    }
     public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
        {
            var msg = message.FromJson<Message>();
            instrument.transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
            instrument.transform.localRotation = msg.transform.rotation;
            
        }
}
