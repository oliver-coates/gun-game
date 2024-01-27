using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayAudioRandomly : MonoBehaviour
{
    public float minInterval = 0.5f;
    public float maxInterval = 4f;

    private float interval = 1f;
    private float elapsedTime = 0f;

    public AudioClip[] audioClips;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        interval = UnityEngine.Random.Range(minInterval, maxInterval);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > interval)
        {
            var source = UnityEngine.Random.Range(0, audioClips.Length);
            audioSource.PlayOneShot(audioClips[source]);
            elapsedTime = 0f;
            interval = UnityEngine.Random.Range(minInterval, maxInterval);
        }
    }
}
