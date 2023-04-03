using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGameAudioRecorder;
using System.IO;
public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        AudioRecorder.fileName = "sound";
        AudioRecorder.filePath = Application.streamingAssetsPath;
        Debug.Log("Application: "+ Application.streamingAssetsPath);

        // check if a file with the same name exists and delete it
        string filePath = Path.Combine(AudioRecorder.filePath, AudioRecorder.fileName + ".wav");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Deleted existing file: " + filePath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
