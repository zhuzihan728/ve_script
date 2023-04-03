using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

[System.Serializable]
public class KeyPress : MonoBehaviour, IUseable
{
    // network
    private NetworkContext context;
    public NetworkId NetworkId { get; set; }
    
    private int MAX_RANGE = 3;
    private AudioSource audioSource;
    public VisualHitTips VHT;

    public bool isPlaying;
    public int InstrumentIndex;


    private void Start()
    {
        context = NetworkScene.Register(this);
        VHT = FindObjectOfType<VisualHitTips>();
    }

    private void Awake()
    {
        isPlaying = false;
        audioSource = GetComponent<AudioSource>();
    }
    
    public void UnUse(Hand controller)
    {
        audioSource.Stop();
        isPlaying = false;
    }

    public void Use(Hand controller)
    {
        // check player's position
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            isPlaying = true;
            audioSource.Play();
            VHT.OnNode();
        }
    }


    public struct Message
    {
        public int InstrumentIndex;
        public bool isPlaying;

        public Message(int InstrumentIndex, bool isPlaying)
        {
            this.InstrumentIndex = InstrumentIndex;
            this.isPlaying= isPlaying;
        }
    }

    private void Update()
    {   
        context.SendJson(new Message(InstrumentIndex, isPlaying));
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>(); 
        if(msg.isPlaying == true && msg.InstrumentIndex == this.InstrumentIndex){
            audioSource.Play();
            VHT.OnNode();
            Debug.Log("msg received," + gameObject.name + "is playing a sound");
        }
    }
}
