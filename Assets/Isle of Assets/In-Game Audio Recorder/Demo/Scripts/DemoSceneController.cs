using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace InGameAudioRecorder
{
    [HelpURL("https://assetstore.unity.com/packages/slug/228338")]
    [RequireComponent(typeof(AudioSource))]
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] clips;

        [SerializeField]
        private Sprite[] musicSprites;

        [SerializeField]
        private GameObject lightObj;

        [SerializeField]
        private AudioSource backgroundMusic;

        [SerializeField]
        private Image musicImage;

        [SerializeField]
        private Text recordingTimeText;

        private AudioSource audioSource;
        private Coroutine lightFlickering;

        private float recordingTime;
        private int musicIndex = 1;

        /// <summary>
        /// Starts recording audio
        /// </summary>
        public void StartRecording()
        {
            AudioRecorder.Instance.StartRecording();
            if (recordingTimeText.color.a < 1f)
                recordingTimeText.color = new Color(recordingTimeText.color.r, recordingTimeText.color.g, recordingTimeText.color.b, 1f);
            if (lightFlickering == null)
                lightFlickering = StartCoroutine(LightFlickering());
        }

        /// <summary>
        /// Stops and saves recorded audio
        /// </summary>
        public void StopRecording()
        {
            AudioRecorder.Instance.StopRecording();
            if (lightFlickering != null)
            {
                StopCoroutine(lightFlickering);
                lightObj.SetActive(false);
                lightFlickering = null;
            }
            recordingTime = 0f;
        }

        /// <summary>
        /// Turning music on/off
        /// </summary>
        public void MusicButton()
        {
            musicIndex = musicIndex == 0 ? 1 : 0;
            musicImage.sprite = musicSprites[musicIndex];
            backgroundMusic.volume = musicIndex;
        }

        /// <summary>
        /// Plays sound by index
        /// </summary>
        /// <param name="clipIndex"></param>
        public void PlaySound(int clipIndex)
        {
            audioSource.clip = clips[clipIndex];
            audioSource.Play();
        }

        /// <summary>
        /// The flickering effect of the recording light
        /// </summary>
        /// <returns></returns>
        private IEnumerator LightFlickering()
        {
            WaitForSeconds delay = new WaitForSeconds(0.75f);
            while(true)
            {
                lightObj.SetActive(!lightObj.activeSelf);
                yield return delay;
            }
        }

        /// <summary>
        /// Preparing for this instance to work
        /// </summary>
        private void Start()
        {
            AudioRecorder.filePath = Application.streamingAssetsPath;
            AudioRecorder.fileName = "Sound";

            audioSource = GetComponent<AudioSource>();
            lightObj.SetActive(false);
            musicImage.sprite = musicSprites[1];
        }

        /// <summary>
        /// Timer operation
        /// </summary>
        private void Update()
        {
            if (!AudioRecorder.IsRecording)
                return;
            recordingTime += Time.deltaTime;
            int milliseconds = (int)((recordingTime - (int)recordingTime) * 1000);
            int seconds = (int)(recordingTime % 60);
            int minutes = (int)recordingTime / 60 % 60;
            int hours = (int)recordingTime / 3600;
            if (hours == 100)
            {
                StopRecording();
                return;
            }
            recordingTimeText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("000");
        }
    }
}