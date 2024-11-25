using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip doorOpenSound, doorCloseSound, doorMovementSound;
    public Animator anim;
    public bool isOpen;
    public float delayTimeOpen, delayTimeClose;
    public Collider[] colliders;

    public void ToggleDoor()
    {
        if (isOpen)
        {
            StartCoroutine(InitiateCloseDoor());
        }
        else
        {
            StartCoroutine(InitiateOpenDoor());
        }

        isOpen = !isOpen;
    }

    IEnumerator InitiateOpenDoor()
    {
        if (doorOpenSound)
            audioSource.PlayOneShot(doorOpenSound);
        yield return new WaitForSeconds(delayTimeOpen);
        if (doorMovementSound)
            audioSource.PlayOneShot(doorMovementSound);
        anim.Play("OpenDoor");
        
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
    
    IEnumerator InitiateCloseDoor()
    {
        if (doorMovementSound)
            audioSource.PlayOneShot(doorMovementSound);
        anim.Play("CloseDoor");
        yield return new WaitForSeconds(delayTimeClose);
        if (doorCloseSound)
            audioSource.PlayOneShot(doorCloseSound);
        
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
