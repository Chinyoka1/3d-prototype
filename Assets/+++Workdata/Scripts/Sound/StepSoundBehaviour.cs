using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 0.2f;
    [SerializeField] private Vector3 raycastPosition;

    [Header("Walk")]
    [SerializeField] private AudioClip[] defaultStepSounds;
    [SerializeField] private AudioClip[] stoneStepSounds;
    [SerializeField] private AudioClip[] grassStepSounds;
    [SerializeField] private AudioClip[] mudStepSounds;
    [SerializeField] private AudioClip[] woodStepSounds;
    
    [Header("Run")]
    [SerializeField] private AudioClip[] defaultRunSounds;
    [SerializeField] private AudioClip[] stoneRunSounds;
    [SerializeField] private AudioClip[] grassRunSounds;
    [SerializeField] private AudioClip[] mudRunSounds;
    [SerializeField] private AudioClip[] woodRunSounds;
    
    [Header("Land")]
    [SerializeField] private AudioClip[] defaultLandSounds;
    [SerializeField] private AudioClip[] stoneLandSounds;
    [SerializeField] private AudioClip[] grassLandSounds;
    [SerializeField] private AudioClip[] mudLandSounds;
    [SerializeField] private AudioClip[] woodLandSounds;

    public void PlayStepSound()
    {
        Ray ray = new Ray(transform.position + raycastPosition, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayer))
        {
            string groundTag = hit.collider.tag;

            switch (groundTag)
            {
                case "Grass":
                    PlayRandomSound(grassStepSounds);
                    break;
                
                case "Stone":
                    PlayRandomSound(stoneStepSounds);
                    break;
                
                case "Mud":
                    PlayRandomSound(mudStepSounds);
                    break;
                
                case "Wood":
                    PlayRandomSound(woodStepSounds);
                    break;
                
                default: 
                    PlayRandomSound(defaultStepSounds);
                    break;
            }
        }
    }
    
    public void PlayRunSound()
    {
        Ray ray = new Ray(transform.position + raycastPosition, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayer))
        {
            string groundTag = hit.collider.tag;

            switch (groundTag)
            {
                case "Grass":
                    PlayRandomSound(grassRunSounds);
                    break;
                
                case "Stone":
                    PlayRandomSound(stoneRunSounds);
                    break;
                
                case "Mud":
                    PlayRandomSound(mudRunSounds);
                    break;
                
                case "Wood":
                    PlayRandomSound(woodRunSounds);
                    break;
                
                default: 
                    PlayRandomSound(defaultRunSounds);
                    break;
            }
        }
    }
    
    public void PlayLandSound()
    {
        Ray ray = new Ray(transform.position + raycastPosition, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayer))
        {
            string groundTag = hit.collider.tag;

            switch (groundTag)
            {
                case "Grass":
                    PlayRandomSound(grassLandSounds);
                    break;
                
                case "Stone":
                    PlayRandomSound(stoneLandSounds);
                    break;
                
                case "Mud":
                    PlayRandomSound(mudLandSounds);
                    break;
                
                case "Wood":
                    PlayRandomSound(woodLandSounds);
                    break;
                
                default: 
                    PlayRandomSound(defaultLandSounds);
                    break;
            }
        }
    }

    private void PlayRandomSound(AudioClip[] audioClips)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[randomIndex]);
    }
}