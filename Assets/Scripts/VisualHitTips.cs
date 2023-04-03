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
    }
    public void OnPlay()
    {
        highestScore = 0;
        time = 0;
        isRunning = true;
        scoreText.enabled = true;
        timeText.enabled = true;
        canHit = false;
        Resetscore();
    }
    public void OnStop()
    {
        isRunning = false;
        if (score > highestScore)
        {
            highestScore = score;
            highScoreText.UpdateText(highestScore.ToString());
        }
        context.SendJson(new Message(highestScore));
    }
    // Update is called once per frame
    void Update()
    {
        TempoText.UpdateText(tempo.ToString("f2"));
        min = tempo - range;
        max = tempo + range;
        if (isRunning)
        {
            time += Time.deltaTime;
            timeText.UpdateText(time.ToString("f2"));
            timer = time % tempo;
            if (time < 0.5f)
                return;
            if (timer >= min|| timer< range)
            {
                if (!isPlaying)
                {
                    particleSystem.Play();
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
                    list.Add(time);
                    score++;
                    scoreText.UpdateText(score.ToString());
                }
        }
    }

    public struct Message
    {
        public int highestScore;

        public Message(int highestScore)
        {
            this.highestScore = highestScore;
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>(); 
        if(msg.highestScore > highestScore){
            highestScore = msg.highestScore;
            highScoreText.UpdateText(highestScore.ToString());
        }
    }

}

