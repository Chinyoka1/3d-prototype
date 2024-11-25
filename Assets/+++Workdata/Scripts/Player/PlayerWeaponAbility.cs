using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponAbility : MonoBehaviour
{
    public enum WeaponState{ Unarmed, TwoHandSword, OneHandSword}
    public WeaponState weaponState;
    
    private PlayerController playerController;
    private InputAction sheatheAction;

    public GameObject bigSwordBack;
    public GameObject bigSwordHand;

    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        StartCoroutine(DelayReference());
    }

    private void OnDisable()
    {
        sheatheAction.performed -= WeaponSheatheUnsheathe;
    }
    
    IEnumerator DelayReference()
    {
        yield return null;
        sheatheAction = playerController.inputActions.Player.Sheathe;
        sheatheAction.performed += WeaponSheatheUnsheathe;
    }

    public void WeaponSheatheUnsheathe(InputAction.CallbackContext ctx)
    {
        switch (weaponState)
        {
            case WeaponState.Unarmed:
                //... which weapon?
                playerController.UnsheatheWeaponAnimation(1);
                break;
            
            case WeaponState.OneHandSword:

                break;
            
            case WeaponState.TwoHandSword:
                playerController.SheatheWeaponAnimation(0);
                break;
        }
        

    }

    public void WeaponSwitch()
    {
        switch (weaponState)
        {
            case WeaponState.Unarmed:
                bigSwordBack.SetActive(false);
                bigSwordHand.SetActive(true);
                weaponState = WeaponState.TwoHandSword;

                break;
            
            case WeaponState.OneHandSword:

                break;
            
            case WeaponState.TwoHandSword:
                bigSwordBack.SetActive(true);
                bigSwordHand.SetActive(false);
                weaponState = WeaponState.Unarmed;

                break;
        }
    }

}
