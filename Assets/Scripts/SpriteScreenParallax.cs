﻿using UnityEngine;

public class SpriteScreenParallax : MonoBehaviour
{
    
        #region Variables

    [Header("Parallax config")]
    [SerializeField] private Vector2 effect = Vector2.zero;

    private Transform cam;
    private Vector2 startPos;

        #endregion

    void Start() {
        startPos = (Vector2)transform.position;
    }

    void LateUpdate() {

        Vector2 pos = GameManager.Camera.transform.position * -effect;

        transform.position = new Vector2(
            startPos.x + pos.x,
            startPos.y + pos.y
        );

    }

}
