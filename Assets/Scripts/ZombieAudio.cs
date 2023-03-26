using UnityEngine;

public class ZombieAudio: MonoBehaviour
{
    public AudioClip[] audioClips;
    public float loopDelay = 10.0f;

    private AudioSource audioSource;
    private float timeSinceLastLoop;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timeSinceLastLoop = loopDelay;
    }

    void Update()
    {
        timeSinceLastLoop += Time.deltaTime;

        if (timeSinceLastLoop >= loopDelay)
        {
            int randomClipIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomClipIndex];
            audioSource.Play();
            timeSinceLastLoop = 0.0f;
        }
    }
}
