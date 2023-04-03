using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using MText;
using System.Collections.Generic;
public class RecordPlayback : MonoBehaviour
{
    [SerializeField] Modular3DText saveText = null;

    // public AudioClip[] RecordedClip = new AudioClip[2];
    public List<AudioClip> RecordedClip = new List<AudioClip>();
    public AudioSource audioSource;
    public Text Adress; 
    private string fileName;
    private byte[] data;

    private void Start()
    {
        saveText.enabled = true;
        saveText.UpdateText(Application.streamingAssetsPath.ToString());
    }

    public void Replay()
    {
        StartCoroutine(PlayIe(0));
        if (RecordedClip == null)
        {
            Debug.Log("No recording available");
            return;
        }
        audioSource.clip = RecordedClip.ToArray()[^1];
        audioSource.Play();
        Debug.Log("Playing recording");

    }


    public static string[] GetAudiosByPath()
    {

        string path = Application.streamingAssetsPath;

        string[] audioClipspath = null;
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*");
            audioClipspath = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                audioClipspath[i] = files[i].FullName;
            }

        }

        return audioClipspath;
    }
    private IEnumerator PlayIe(int num)
    {
        string[] path = GetAudiosByPath();

        if (path!=null&&path.Length > 0)
        {
            UnityWebRequest _unityWebRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + GetAudiosByPath()[num], AudioType.WAV);
            yield return _unityWebRequest.SendWebRequest();
            AudioClip _audioClip = DownloadHandlerAudioClip.GetContent(_unityWebRequest);
            // RecordedClip[0]= _audioClip;
            RecordedClip.Add(_audioClip);
        }

     
    }

}
