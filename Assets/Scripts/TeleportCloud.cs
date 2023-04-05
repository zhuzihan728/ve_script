using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;

public class TeleportCloud : MonoBehaviour, IUseable
{
    // network
    private NetworkContext context;
    public NetworkId NetworkId { get; set; }

    public int MAX_RANGE = 4;
    private Rigidbody Eevee;
    public float UpSpeed = 3f;
    public Vector3 CLOUD_POSITION_1 = new Vector3(10f, 33f, -10f);
    public Vector3 CLOUD_POSITION_2 = new Vector3(-14f, 25f, 12f);
    
    // skybox: index 0-3
    public Material skyboxDefault;
    public Material skyboxDawn;
    public Material skyboxBright;
    public Material skyboxTwilight;

    public float time;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        Eevee = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timer = time % 3f;
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            if (timer < 0.05f){
                Debug.Log("Eevee is jumping");
                GetComponent<Rigidbody>().velocity = new Vector3(0f, UpSpeed, 0f);
            }
        }
    }

    public void UnUse(Hand controller)
    {
    }

    public void Use(Hand controller)
    {
        // teleport player onto one of the cloud
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= MAX_RANGE){
            skyboxChange();
            if(Random.Range(0, 2) == 0){
                GameObject.Find("Player").transform.position = CLOUD_POSITION_1;
            }
            else{
                GameObject.Find("Player").transform.position = CLOUD_POSITION_2;
            }
        }
    }

    public struct Message
    {
        public int skyboxIndex;

        public Message(int skyboxIndex)
        {
            this.skyboxIndex = skyboxIndex;
        }
    }

    private void skyboxChange(){
        int randomNumber = Random.Range(0, 4);
        context.SendJson(new Message(randomNumber));
        if(randomNumber == 0){
            RenderSettings.skybox = skyboxDefault;
        }
        if(randomNumber == 1){
            RenderSettings.skybox = skyboxDawn;
        }
        if(randomNumber == 2){
            RenderSettings.skybox = skyboxBright;
        }
        if(randomNumber == 3){
            RenderSettings.skybox = skyboxTwilight;
        }
    }
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>(); 
        
        if(msg.skyboxIndex == 0){
            RenderSettings.skybox = skyboxDefault;
        }
        if(msg.skyboxIndex == 1){
            RenderSettings.skybox = skyboxDawn;
        }
        if(msg.skyboxIndex == 2){
            RenderSettings.skybox = skyboxBright;
        }
        if(msg.skyboxIndex == 3){
            RenderSettings.skybox = skyboxTwilight;
        }
    }

}
