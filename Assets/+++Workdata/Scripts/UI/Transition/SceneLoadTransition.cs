using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTransition : MonoBehaviour
{
    public void StartTransition(string sceneName, string inAnimation, string outAnimation, string transitionName)
    {
        //DataPersistanceManager.instance.SaveGame();
        StartCoroutine(InitiateTeleport(sceneName, inAnimation, outAnimation, transitionName));
    }

    private IEnumerator InitiateTeleport(string sceneName, string inAnimation, string outAnimation, string transitionName)
    {
        Animator anim = GameObject.FindGameObjectsWithTag(transitionName)[0].GetComponent<Animator>();
        anim.Play(inAnimation);
        yield return new WaitForSeconds(2.2f);
        anim.Play("idle_active");
        FindObjectOfType<GameController>().gameMode = GameController.GameMode.LoadScene;
        SceneManager.LoadScene(sceneName);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);
        anim.Play(outAnimation);
        yield return new WaitForSeconds(2.2f);
        anim.Play("idle_inactive");
    }
}