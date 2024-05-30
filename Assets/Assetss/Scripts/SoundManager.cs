using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Another instance of this object already exists!");
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    #region Camera Sounds

    [Header("Camera Sounds")] public AudioClip cameraShutter;

    public AudioClip photoPrint;

    public AudioClip cameraZoomIn;

    #endregion

    #region Background Sounds

    [Header("Background Sounds")] public AudioClip birdChirp;

    public AudioClip birdFlap;

    public AudioClip carSound;

    public AudioClip carSound2;

    public AudioClip woodPlankCreak;

    public AudioClip windThroughLeaves;

    public AudioClip scaffoldingShake;

    public AudioClip catMeow;

    public AudioClip constructionSounds;

    #endregion


    #region Camera Sounds Methods

    public void PlayCameraShutter(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(cameraShutter, soundPosition);
    }

    public void PlayPhotoPrint(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(photoPrint, soundPosition);
    }

    public void PlayCameraZoomIn(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(cameraZoomIn, soundPosition);
    }

    #endregion

    #region Background Sounds Methods

    public void PlayBirdChirp(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(birdChirp, soundPosition);
    }

    public void PlayBirdFlap(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(birdFlap, soundPosition);
    }

    public void PlayCarSound(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(carSound, soundPosition);
    }

    public void PlayCarSound2(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(carSound2, soundPosition);
    }

    public void PlayWoodPlankCreak(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(woodPlankCreak, soundPosition);
    }

    public void PlayWindThroughLeaves(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(windThroughLeaves, soundPosition);
    }

    public void PlayScaffoldingShake(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(scaffoldingShake, soundPosition);
    }

    public void PlayCatMeow(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(catMeow, soundPosition);
    }

    public void PlayConstructionSounds(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(constructionSounds, soundPosition);
    }

    #endregion


    private void PlaySoundEffectAtPoint(AudioClip clip, Transform soundPosition)
    {
        if (clip != null)
        {
            if (soundPosition == null)
            {
                Debug.LogWarning("Sound position is null. Unable to play sound effect at point.");
                
                if (Camera.main == null)
                {
                    Debug.LogWarning("Main camera not found. Unable to play sound effect at point.");
                }
                else
                {
                    AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
                }
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, soundPosition.position);
            }
        }
        else
        {
            Debug.LogWarning("AudioClip is null. Unable to play sound effect.");
        }
    }
}