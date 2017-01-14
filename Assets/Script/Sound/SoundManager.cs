using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (AudioSource))]
public class SoundManager : MonoBehaviour {

	internal static SoundManager _instance;

	private AudioSource audioSource;

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();

		StartCoroutine (PlayAndWait (SoundConfig.FOREST_PATH, 1.0f, 310.0f, 311.0f, 0.0f));
		StartCoroutine (PlayAndWait (SoundConfig.WAVE_PATH, 0.8f, 30.0f, 60.0f, 10.0f));
	}

	internal void PlayEffect(string path, float vol) {
		AudioClip clip = (AudioClip)Resources.Load (path);
		if (clip != null) {
			audioSource.PlayOneShot(clip, vol);
		}
	}

	public void PlayClick() {
		SoundManager._instance.PlayEffect (SoundConfig.BUTTON_1, 1.0f);
	}

	private IEnumerator PlayAndWait(string path, float vol, float minDuration, float maxDuration, float delay) {
		yield return new WaitForSeconds(delay);
		while (true) {
			PlayEffect(path, vol);
			yield return new WaitForSeconds(Random.Range(minDuration, maxDuration));
		}
	}

}
