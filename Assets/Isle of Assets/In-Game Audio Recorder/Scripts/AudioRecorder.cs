using System;
using System.Text;
using System.IO;
using UnityEngine;

namespace InGameAudioRecorder
{
	[HelpURL("https://assetstore.unity.com/packages/slug/228338")]
	[RequireComponent(typeof(AudioListener))]
	public class AudioRecorder : MonoBehaviour
	{
		public static AudioRecorder Instance { get; private set; }

		public static string filePath, fileName;
		public static bool IsRecording { get; private set; }
		public static bool IsSaveAudioOnApplicationQuit = true;
		public const int OutputSampleRate = 12000; // 48000

		private const int headerSize = 44;

		private FileStream fileStream;

		/// <summary>
		/// Starts recording audio
		/// </summary>
		public void StartRecording()
		{
			if (IsRecording)
			{
				Debug.LogError("The recording cannot be started because it is already running");
				return;
			}
			IsRecording = true;
			if (string.IsNullOrEmpty(filePath))
            {
				Debug.LogError("You have not chosen a place to save the file!");
				return;
            }
			if (string.IsNullOrEmpty(fileName))
			{
				Debug.LogError("You didn't choose a file name!");
				return;
			}
			CreateAudioFile();

			Debug.Log("Recording started");
		}

		/// <summary>
		/// Stops and saves recorded audio
		/// </summary>
		public void StopRecording()
		{
			if (!IsRecording)
            {
				Debug.LogError("The recording cannot be stopped because it has not started yet");
				return;
            }
			IsRecording = false;
			SaveAudioFile();

			Debug.Log("Recording stopped. The file is saved in the directory \"" + filePath + "\"");
		}

		/// <summary>
		/// Creates an audio file at the specified path and fills it with empty bytes for now
		/// </summary>
		private void CreateAudioFile()
        {
			string allFilePath = filePath + "\\" + fileName + ".wav";
			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);
			else if (File.Exists(allFilePath))
				File.Delete(allFilePath);
			fileStream = new FileStream(allFilePath, FileMode.Create);
			for (int i = 0; i < headerSize; i++)
				fileStream.WriteByte(new byte());
		}

		/// <summary>
		/// The algorithm for saving audio in the WAVE format
		/// </summary>
		private void SaveAudioFile()
		{
			fileStream.Flush();//
			fileStream.Seek(0, SeekOrigin.Begin);
			fileStream.Write(Encoding.UTF8.GetBytes("RIFF"), 0, 4);
			fileStream.Write(BitConverter.GetBytes(fileStream.Length - 8), 0, 4);
			fileStream.Write(Encoding.UTF8.GetBytes("WAVE"), 0, 4);
			fileStream.Write(Encoding.UTF8.GetBytes("fmt "), 0, 4);
			fileStream.Write(BitConverter.GetBytes(16), 0, 4);
			fileStream.Write(BitConverter.GetBytes(1), 0, 2);
			fileStream.Write(BitConverter.GetBytes(2), 0, 2);
			fileStream.Write(BitConverter.GetBytes(OutputSampleRate), 0, 4);
			fileStream.Write(BitConverter.GetBytes(OutputSampleRate * 4), 0, 4);
			fileStream.Write(BitConverter.GetBytes(4), 0, 2);
			fileStream.Write(BitConverter.GetBytes(16), 0, 2);
			fileStream.Write(Encoding.UTF8.GetBytes("data"), 0, 4);
			fileStream.Write(BitConverter.GetBytes(fileStream.Length - headerSize), 0, 4);
			fileStream.Close();
		}

		/// <summary>
		/// This is called by unity when bunch of audio samples gets accumulates
		/// </summary>
		/// <param name="data"></param>
		/// <param name="channels"></param>
		// private void OnAudioFilterRead(float[] data, int channels)
		// {
		// 	if (!IsRecording)
		// 		return;
		// 	byte[] bytesData = new byte[data.Length * 2];
		// 	for (int i = 0; i < data.Length; i++)
		// 		BitConverter.GetBytes((short)(data[i] * 32767)).CopyTo(bytesData, i * 2);
		// 	fileStream.Write(bytesData, 0, bytesData.Length);
		// }
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (!IsRecording)
				return;

			// create buffer to hold the converted audio samples
			short[] shortData = new short[data.Length];

			// convert the float audio samples to short
			for (int i = 0; i < data.Length; i++)
				shortData[i] = (short)(data[i] * 32767);

			// convert the short audio samples to bytes and write to file
			byte[] bytesData = new byte[shortData.Length * 2];
			Buffer.BlockCopy(shortData, 0, bytesData, 0, bytesData.Length);
			fileStream.Flush();
			fileStream.Write(bytesData, 0, bytesData.Length);
		}


		/// <summary>
		/// Automatic file saving when exiting the application
		/// </summary>
		private void OnApplicationQuit()
        {
			if (!IsSaveAudioOnApplicationQuit || !IsRecording)
				return;
			StopRecording();
        }

		/// <summary>
		/// Preparing for this instance to work
		/// </summary>
		private void Start()
		{
			if (Instance == null)
				Instance = this;
			else
				Destroy(Instance.gameObject);
			AudioSettings.outputSampleRate = OutputSampleRate;

			// play sounds that are marked as PlayOnAwake
		// 	AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		// 	for (int i = 0; i < allAudioSources.Length; i++)
		// 		if (allAudioSources[i].playOnAwake)
		// 			allAudioSources[i].Play();
		}
	}
}