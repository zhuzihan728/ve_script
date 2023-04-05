using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Messaging;
using MText;
// using Ubiq.Messaging;

public class VisualHitTips : MonoBehaviour
{
    // network
    private NetworkContext context;
    public NetworkId NetworkId { get; set; }

    [SerializeField] Modular3DText timeText = null;
    [SerializeField] Modular3DText scoreText = null;
    [SerializeField] Modular3DText highScoreText = null;
    [SerializeField] Modular3DText TempoText = null;
    
    public GameObject CircularWall;
    public Vector3 CircularWallInitialPosition;
    public Vector3 CircularWallFinalPosition;
    public float DownOffset = 20f;
    public GameObject CircularRing;
    public ChangeColor Wall0;
    public ChangeColor Wall1;
    public ChangeColor Wall2;
    public ChangeColor Wall3;
    public ChangeColor Wall4;
    public ChangeColor Wall5;

    public StartStopControl SSC;

    public float min = 0.9f, max = 1.1f;
    public static float range = 0.1f;
    public static float time;
    float timer;
    public bool isRunning;
    public bool isPlaying;
    public List<float> list = new List<float>();
    public List<float> listClick = new List<float>();
    bool canHit;
    public int score;
    public ParticleSystem particleSystem;
    public float tempo = 1f;
    public int highestScore = 0;
    public AudioSource matchedAudio;
    
    public void Resetscore()
    {
        score=0;
        scoreText.UpdateText(score.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        scoreText.enabled = false;
        timeText.enabled = false;
        highestScore = 0;
        CircularWallFinalPosition = CircularWall.transform.position;
        CircularWallInitialPosition = new Vector3(CircularWallFinalPosition.x, CircularWallFinalPosition.y - DownOffset, CircularWallFinalPosition.z);
        CircularWall.SetActive(false);
        CircularRing.SetActive(false);
    }
    public void OnPlay()
    {
        time = 0;
        isRunning = true;
        scoreText.enabled = true;
        timeText.enabled = true;
        canHit = false;
        CircularRing.SetActive(true);
        CircularWall.SetActive(true);
        Resetscore();
    }

    public void OnSlow(){
        if(tempo < 3.0f && SSC.RecordStart == false){
            tempo += 0.05f;
            context.SendJson(new Message(highestScore, tempo));
        }
    }

    public void OnFast(){
            if(tempo > 0.5f && SSC.RecordStart == false){
                tempo -= 0.05f;
                context.SendJson(new Message(highestScore, tempo));
            }
    }

    public void OnStop()
    {
        CircularRing.SetActive(false);
        CircularWall.SetActive(false);
        isRunning = false;
        if (score > highestScore)
        {
            highestScore = score;
            highScoreText.UpdateText(highestScore.ToString());
        }
        context.SendJson(new Message(highestScore, 0f));
    }
    // Update is called once per frame
    void Update()
    {
        TempoText.UpdateText((1/tempo).ToString("f2"));
        min = tempo - range;
        max = tempo + range;
        if (isRunning)
        {

            time += Time.deltaTime;
            timeText.UpdateText(time.ToString("f2"));
            timer = time % tempo;

            // visual tips wall
            CircularWall.transform.position = Vector3.Lerp(CircularWallInitialPosition, CircularWallFinalPosition, timer / tempo);
            if (time < 0.5f)
                return;
            if (timer >= min|| timer< range)
            {
                if (!isPlaying)
                {
                    
                    // particleSystem.Play();
                    canHit = true;
                    isPlaying = true;
                }
            }
            else
            {
                isPlaying = false;
            }
        }
    }
    public void OnNode()
    {
        if (canHit)
        {
            canHit = false;
            listClick.Add(time);
            if (time > 0.5f)
                if (timer >= min || timer <= max-1)
                {
                    Wall0.ChangeColorButton();
                    Wall1.ChangeColorButton();
                    Wall2.ChangeColorButton();
                    Wall3.ChangeColorButton();
                    Wall4.ChangeColorButton();
                    Wall5.ChangeColorButton();
                    matchedAudio.Play();
                    list.Add(time);
                    score++;
                    scoreText.UpdateText(score.ToString());
                }
        }
    }

    public struct Message
    {
        public int highestScore;
        public float tempo;

        public Message(int highestScore, float tempo)
        {
            this.highestScore = highestScore;
            this.tempo = tempo;
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        
        var msg = message.FromJson<Message>();
        
        if (SSC.RecordStart == true)
        {
            return;
        }
        if (msg.tempo != 0f)
        {
            Debug.Log("=================================="); 
            Debug.Log(msg.tempo); 
            tempo = msg.tempo;
        }
        
        if(msg.highestScore > highestScore){
            highestScore = msg.highestScore;
            highScoreText.UpdateText(highestScore.ToString());
        }
    }

}

