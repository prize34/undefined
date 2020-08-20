﻿using UnityEngine;

public class PortalBoss_Master : MonoBehaviour
{

        #region Variables

    [Header("Height Settings")]
    [SerializeField] private float maxHeight = 0f;

    [Header("Big Rock")]
    [SerializeField] private AnimationCurve bigRockDelayCurve;
    [SerializeField] private float bigRockDelayStart;
    [SerializeField] private float bigRockDelayEnd;

    [Space]
    [SerializeField] private AnimationCurve bigRockFloatVelocityCurve;
    [SerializeField] private float bigRockFloatVelocityStart;
    [SerializeField] private float bigRockFloatVelocityEnd;

    [Header("Laser Rock")]
    [SerializeField] private AnimationCurve laserRockDelayCurve;
    [SerializeField] private float laserRockDelayStart;
    [SerializeField] private float laserRockDelayEnd;
    
    [Space]
    [SerializeField] private AnimationCurve laserRockPrepSpeedCurve;
    [SerializeField] private float laserRockPrepSpeedStart;
    [SerializeField] private float laserRockPrepSpeedEnd;

    // Script side variables
    private float startHeight = 0f;
    private Transform player;
    private float heightPercentage = 0f;

    private float bigRockActualDelay = 0f;
    private float bigRockActualFloatVelocity = 0f;

    private float laserRockActualDelay = 0f;
    private float laserRockActualPrepSpeed = 0f;

    // Public acess variables
    public float BigRockActualDelay { get { return bigRockActualDelay; } }
    public float BigRockActualVelocity { get { return bigRockActualFloatVelocity; } }
    public float BigRockActualPercantage { get { return this.bigRockDelayCurve.Evaluate(this.heightPercentage); } }

    public float LaserActualDelay { get { return laserRockActualDelay;} }
    public float LaserActualPrepSpeed { get { return laserRockActualPrepSpeed; } }
    public float LaserActualPercentage { get { return this.laserRockDelayCurve.Evaluate(this.heightPercentage); } }

        #endregion


    void Start() {

        this.startHeight = this.transform.position.y;
        this.player = GameManager.Player.gameObject.transform;

        CalculateBigRockDelay();
        CalculateBigRockVelocity();

        CalculateLaserRockDelay();
        CalculateLaserRockPrepSpeed();
    }

    void Update() {

        this.heightPercentage = Mathf.Clamp((this.player.position.y - this.startHeight) / this.maxHeight, 0, 1);

        CalculateBigRockDelay();
        CalculateBigRockVelocity();

        CalculateLaserRockDelay();
        CalculateLaserRockPrepSpeed();
    }

    void CalculateLaserRockDelay() {

        laserRockActualDelay = Mathf.Lerp(
            this.laserRockDelayStart,
            this.laserRockDelayEnd,
            this.laserRockDelayCurve.Evaluate(this.heightPercentage)
        );

    }

    void CalculateLaserRockPrepSpeed() {

        laserRockActualPrepSpeed = Mathf.Lerp(
            this.laserRockPrepSpeedStart,
            this.laserRockPrepSpeedEnd,
            this.laserRockPrepSpeedCurve.Evaluate(this.heightPercentage)
        );

    }

    void CalculateBigRockVelocity() {

        bigRockActualFloatVelocity = Mathf.Lerp(
            this.bigRockFloatVelocityStart,
            this.bigRockFloatVelocityEnd,
            this.bigRockDelayCurve.Evaluate(this.heightPercentage)
        );

    }

    void CalculateBigRockDelay() {

        bigRockActualDelay = Mathf.Lerp(
            this.bigRockDelayStart, 
            this.bigRockDelayEnd,
            this.bigRockDelayCurve.Evaluate(this.heightPercentage)
            );

    }

}
