using UnityEngine;

public class PlankSoundTrigger : MonoBehaviour
{
    private AudioSource plankAudioSource;
    private bool isPlaying;

    private void Start()
    {
        plankAudioSource = gameObject.AddComponent<AudioSource>();
        plankAudioSource.clip = SoundManager.Instance.woodPlankCreak;
        plankAudioSource.loop = true;
        isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlaying)
        {
            plankAudioSource.Play();
            isPlaying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlaying)
        {
            plankAudioSource.Stop();
            isPlaying = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !plankAudioSource.isPlaying)
        {
            plankAudioSource.Play();
        }
    }
}
