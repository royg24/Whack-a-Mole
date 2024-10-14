using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderPointerHandler : MonoBehaviour, IPointerUpHandler
{
    public AudioSource audioSource;
    [SerializeField] private AudioClip exampleClip;

    public void Start()
    {
        audioSource = GameSettings.GameSettingsInstance.SoundAudioSource;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress.CompareTag("Sound"))
        {
            audioSource.PlayOneShot(exampleClip);
        }
    }
}