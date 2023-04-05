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
        
        // audioSource.Stop();
        isPlaying = false;
        context.SendJson(new Message(InstrumentIndex, isPlaying));
        StartCoroutine("PlaySound");
    }

    public void Use(Hand controller)
    {
        // check player's position
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            isPlaying = true;
            context.SendJson(new Message(InstrumentIndex, isPlaying));
            audioSource.Play();
            VHT.OnNode();
           
        }
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(.5f);
        audioSource.Stop();
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
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>(); 
        if(msg.isPlaying == true && msg.InstrumentIndex == this.InstrumentIndex){
            isPlaying = true;
            audioSource.Play();
            VHT.OnNode();
        }
        else if(msg.isPlaying == false && msg.InstrumentIndex == this.InstrumentIndex){
            isPlaying = false;
            StartCoroutine("PlaySound");
        }
    }
}
