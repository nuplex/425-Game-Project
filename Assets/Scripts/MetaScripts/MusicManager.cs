using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip mainMenu;
	public AudioClip[] clips; 
	private AudioSource source;

	private float waitCounter;
	private int currentWaitTime;
	public int MIN_SILENCE = 60, MAX_SILENCE = 600;
	private int lastChoice = -1; //nothing on start

	//when game starts up
	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start(){
		waitCounter = 0;
		currentWaitTime = 0;
	}

	// Update is called once per frame
	void Update () {
		waitCounter += Time.deltaTime;
		if(waitCounter >= currentWaitTime){
			PlaySong();
			waitCounter = 0;
			currentWaitTime = NextWaitTime();
		}
	}

	void PlaySong(){
		int choice = Random.Range (0, clips.Length);
		if (choice == lastChoice) {
			choice =(choice + 1) % clips.Length;
		}
		source.clip = clips [choice];
		source.Play();
		lastChoice = choice;
	}

	int NextWaitTime(){
		return (int) Random.Range (MIN_SILENCE, MAX_SILENCE);
	}
}
