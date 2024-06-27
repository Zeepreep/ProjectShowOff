using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Another instance of this object already exists!");
            Destroy(gameObject);
            return;
        }
    }

    #region Camera Sounds
    [Header("Camera Sounds")]
    public AudioClip cameraShutter;
    public AudioClip photoPrint;
    public AudioClip cameraZoomIn;
    #endregion

    #region Background Sounds
    [Header("Background Sounds")]
    public AudioClip birdChirp;
    public AudioClip birdFlap;
    public AudioClip carSound;
    public AudioClip carSound2;
    public AudioClip woodPlankCreak;
    public AudioClip windThroughLeaves;
    public AudioClip scaffoldingShake;
    public AudioClip catMeow;
    public AudioClip constructionSounds;
    public AudioClip audioLevelTransition;
    #endregion

    #region UI Sounds
    [Header("UI Sounds")]
    
    public AudioClip buttonClick;
    public AudioClip errorSound;
    public AudioClip writingSounds;
    public AudioClip paperShredding;
    public AudioClip pictureHung;
    
    #endregion
    
    #region Voice Over Sounds

    [Header("Voice Over Sounds")] 
    
    public AudioClip tutorialVoiceOver;
    public AudioClip level1VoiceOver;
    public AudioClip level2VoiceOver;
    public AudioClip level3VoiceOver;
    
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

    public void PlayWoodPlankCreak(Transform soundPosition)
    {
        if (woodPlankCreak != null)
        {
            AudioSource.PlayClipAtPoint(woodPlankCreak, soundPosition.position);
        }
        else
        {
            Debug.LogError("woodPlankCreak AudioClip is null.");
        }
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

    public void PlayAudioLevelTransition(Transform soundPosition = null) // New method
    {
        PlaySoundEffectAtPoint(audioLevelTransition, soundPosition);
    }
    #endregion

    #region UI Sounds Methods
    public void PlayButtonClick(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(buttonClick, soundPosition);
    }
    
    public void PlayErrorSound(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(errorSound, soundPosition);
    }
    
    public void PlayWritingSounds(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(writingSounds, soundPosition);
    }
    
    public void PlayPaperShredding(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(paperShredding, soundPosition);
    }
    
    public void PlayPictureHung(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(pictureHung, soundPosition);
    }
    #endregion
    
    #region Voice Over Sounds Methods
    
    public void PlayTutorialVoiceOver(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(tutorialVoiceOver, soundPosition);
    }
    
    public void PlayLevel1VoiceOver(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(level1VoiceOver, soundPosition);
    }
    
    public void PlayLevel2VoiceOver(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(level2VoiceOver, soundPosition);
    }
    
    public void PlayLevel3VoiceOver(Transform soundPosition = null)
    {
        PlaySoundEffectAtPoint(level3VoiceOver, soundPosition);
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

    public void PlayCarEngineSound(GameObject vehicle)
    {
        AudioSource vehicleAudioSource = vehicle.GetComponent<AudioSource>();
        if (vehicleAudioSource == null)
        {
            vehicleAudioSource = vehicle.AddComponent<AudioSource>();
            vehicleAudioSource.volume = 0.01f;
        }

        if (carSound != null)
        {
            vehicleAudioSource.clip = carSound;
            vehicleAudioSource.loop = true;
            vehicleAudioSource.Play();
        }
        else
        {
            Debug.LogError("Car sound clip is not set in the SoundManager.");
        }
    }

    public void StopCarEngineSound(GameObject vehicle)
    {
        AudioSource vehicleAudioSource = vehicle.GetComponent<AudioSource>();
        if (vehicleAudioSource != null && vehicleAudioSource.isPlaying)
        {
            vehicleAudioSource.Stop();
        }
    }
}
