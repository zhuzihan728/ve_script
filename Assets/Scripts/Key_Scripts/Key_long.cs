using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

public class Key_long : MonoBehaviour, IUseable
{
    // private Hand follow;
    // private Rigidbody body;
    private NetworkContext context;
    private AudioSource audioSource;
    public NetworkId NetworkId { get; set; }


    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void UnUse(Hand controller)
    {
    }

    public void Use(Hand controller)
    {
        audioSource.Play();
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
        // audioSource.Play();
        context.SendJson(new Message(transform));
        
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>(); 
    }
}