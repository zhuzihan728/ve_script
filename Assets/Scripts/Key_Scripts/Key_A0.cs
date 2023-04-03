using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

public class Key_A0 : MonoBehaviour, IUseable
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
        // body = GetComponent<Rigidbody>();
    }
    
    public void UnUse(Hand controller)
    {
        audioSource.Stop();
    }

    public void Use(Hand controller)
    {
        
        // if (audioSource.isPlaying)
        // {
        //     audioSource.Stop();
        // }
        // else
        // {
        audioSource.Play();
        // }
    }
    // public void OnMouseDown()
    // {
    //     // audioSource.Play();
    // }
    

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

// using System.Collections;
// using System.Collections.Generic;
// using Ubiq.XR;
// using UnityEngine;
// using Ubiq.Messaging;
// public class Key_A0 : MonoBehaviour, IUseable
// {
//     private AudioSource audioSource;
//     private Hand follow;
//     // private Rigidbody body;
//     private NetworkContext context;

//     public bool owner;

//     public NetworkId NetworkId { get; set; }

//     private void Start()
//     {
//         context = NetworkScene.Register(this);
//     }


//     // public void Attach(Hand hand)
//     // {
//     //     follow = hand;
//     //     owner = true;
//     // }

//     // public void Detach(Hand hand)
//     // {
//     //     follow = null;
//     //     owner = false;
//     // }


//     private void Awake()
//     {
//         audioSource = GetComponent<AudioSource>();
//         body = GetComponent<Rigidbody>();
//         owner = false;
//     }

//     public void UnUse(Hand controller)
//     {

//     }

//     public void Use(Hand controller)
//     {
        
//         if (audioSource.isPlaying)
//         {
//             audioSource.Stop();
//         }
//         else
//         {
//             audioSource.Play();
//         }
//     }
//     public struct Message
//     {
//         public TransformMessage transform;
//         public Message(Transform transform)
//         {
//             this.transform = new TransformMessage(transform);
//         }
//     }
//     private void Update()
//     {
//         if (follow != null)
//         {
//             transform.position = follow.transform.position;
//             transform.rotation = follow.transform.rotation;
//             body.isKinematic = true;
//         }
//         if(owner)
//             {
//                 context.SendJson(new Message(transform));
//             }
//         else
//         {
//             body.isKinematic = false;
//         }
//     }
//      public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
//         {
//             var msg = message.FromJson<Message>();
//             transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
//             transform.localRotation = msg.transform.rotation;
            
//         }
// }
