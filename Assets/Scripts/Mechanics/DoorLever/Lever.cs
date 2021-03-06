﻿using UnityEngine;

public class Lever : MonoBehaviour
{
    
        #region Variables

    [Header("Lever Settings")]
    [SerializeField] private Color leverColor = Color.white;
    [SerializeField] private SpriteRenderer leverSprite = null;

    [Header("Lever Animator")]    
    [SerializeField] private Animator leverAnimator;

    [Header("Doors")]
    [SerializeField] private Door[] doorsToActivate = null;

    // script side
    private bool spriteOn = false;

        #endregion

    void Start() {

        // Updates the lever sprite color
        leverSprite.color = leverColor;

        foreach(Door d in doorsToActivate)
        {
            d.AddDoorColor(leverColor);
        }

    }

    public void Toggle() {

        spriteOn = !spriteOn;
        leverAnimator.SetBool("On", spriteOn);

        foreach(Door d in doorsToActivate)
        {
            d.Toggle();
        }

    }

    [ExecuteInEditMode]
    void OnValidate() {
        leverSprite.color = leverColor;
    }

    void OnDrawGizmos() {

        if(doorsToActivate == null || doorsToActivate.Length <= 0) return;

        foreach(Door door in doorsToActivate) {
            if(door == null) continue;

            Vector3 doorPos = door.transform.position;

            Gizmos.color = leverColor;
            Gizmos.DrawLine(this.transform.position, doorPos);
        }

    }


}
