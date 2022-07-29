using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnlock : PowerUp
{
    public Button firstWeapon;
    public Button secondWeapon;

    public override void interaction(Collider2D collision)
    {

        if (!firstWeapon.IsInteractable())
        {
            firstWeapon.interactable = true;
        }

        else if (!secondWeapon.IsInteractable())
        {
            secondWeapon.interactable = true;
        }
    }

}
