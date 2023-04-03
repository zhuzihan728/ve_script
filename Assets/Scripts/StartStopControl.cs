using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;
using InGameAudioRecorder;
using MText;


public class StartStopControl : MonoBehaviour, IUseable
{
    [SerializeField] Modular3DText startText = null;


    private int MAX_RANGE = 5;
    public AudioRecorder AR;
    public VisualHitTips VHT;
    public GameObject ReplayButton;


    // networking components
    public bool RecordStart = false;
    public bool RecordStop = true;
    // public bool isRecording = false;
    public bool firstRecord = false;
    private NetworkContext context;
    public NetworkId NetworkId { get; set; }


    // Start is called before the first frame update
    void Start()
    {        
        context = NetworkScene.Register(this);
        AR = FindObjectOfType<AudioRecorder>();
        startText.enabled = true;
        ReplayButton = GameObject.Find("ReplayButton");
        ReplayButton.SetActive(false);
    }


    private void Awake()
    {
    }


    void Update()
    {
    }


   
    public void UnUse(Hand controller)
    {
    }


    public void Use(Hand controller)
    {


        // Debug.Log("use StartStopControl");
        // check player's position
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            if (RecordStart == false)
            {
                RecordStart = true;
                RecordStop = false;
                context.SendJson(new Message(RecordStart, RecordStop, true));
                Debug.Log("start recording");
                AR.StartRecording();
                VHT.OnPlay();
                startText.UpdateText("Stop");
            }
            else
            {
                if(firstRecord == false){
                    firstRecord = true;
                    ReplayButton.SetActive(true);
                }
               
                RecordStart = false;
                RecordStop = true;
                context.SendJson(new Message(RecordStart, RecordStop, true));
                Debug.Log("stop recording");    
                AR.StopRecording();
                VHT.OnStop();
                startText.UpdateText("Start");
            }
        }
    }
    public struct Message
    {
        public bool RecordStart;
        public bool RecordStop;
        public bool shouldProcess;


        public Message(bool RecordStart, bool RecordStop, bool shouldProcess = false)
        {
            this.RecordStart = RecordStart;
            this.RecordStop = RecordStop;
            this.shouldProcess = shouldProcess;
        }
    }


    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
       
        var msg = message.FromJson<Message>();
        if (!msg.shouldProcess)
        {
            return;
        }
        Debug.Log("=========================================received message" + " " + msg.RecordStart + " " + msg.RecordStop);
        bool received_stop = msg.RecordStop;
        bool received_start = msg.RecordStart;
        if (received_start != RecordStart)
        {
            RecordStart = received_start;
            RecordStop = received_stop;
            if (RecordStart == true)
            {
                AR.StartRecording();
                VHT.OnPlay();
                startText.UpdateText("Stop");
            }
            else
            {
                if(firstRecord == false){
                    firstRecord = true;
                    ReplayButton.SetActive(true);
                }
                AR.StopRecording();
                VHT.OnStop();
                startText.UpdateText("Start");
            }
        }
       
    }
}
