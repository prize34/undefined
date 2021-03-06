﻿using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PortaBoss_LaserRock : MonoBehaviour
{

    enum LaserRockState {

        Following,
        Shooting,
        Away,

    }

        #region Variables

    [SerializeField] private PortalBoss_Master master;
    [SerializeField] private LaserRockState state;

    [Header("Move")]
    [SerializeField] private float moveVelocity = 1f;
    [SerializeField] private float floatHeight = 30f;

    [Header("Shoot")]
    public Animator laserAnimator = null;
    [SerializeField] private float laserHeight = 2f;
    [SerializeField] private float laserThreshold;
    [SerializeField] private Attack myAttack;
    
    [Space]
    public bool activated = false;
    // Script side variables

    private GameObject player;
    private LineRenderer laserTrail;

    private Vector3 lastPlayerPos;
    private Vector3 lastLaserPos;
    private bool followPlayer;

    private float curShootTimer = 0f;
    private bool pausedTimer = false;

        #endregion

    void LaserStopFollow() {

        this.followPlayer = false;

    }

    void Start() {

        this.player = GameManager.Player.gameObject;
        GameManager.Checkpoint.onLoad += () => {this.player = GameManager.Player.gameObject;};
        
        this.laserTrail = GetComponent<LineRenderer>();
        this.laserTrail.positionCount = 2;
        this.followPlayer = true;

    }

    void FixedUpdate() {

        if(!activated) return;
        if(this.state == LaserRockState.Away) { AwayBehaviour(); return; }

        LaserFollowBehaviour();
        TimerBehaviour();

        switch(this.state)
        {
            case LaserRockState.Following:
                FollowingBehaviour();
                break;
        }

    }

    void FollowingBehaviour() {

        Vector2 gotoVector = Vector2.zero;

        gotoVector = Vector2.Lerp(
            this.transform.position,
            this.player.transform.position + new Vector3(0, floatHeight, 0),
            Time.fixedDeltaTime * this.moveVelocity
            );

        this.transform.position = gotoVector;

    }

    void LaserFollowBehaviour() {

        Vector3 direction = player.transform.position - this.transform.position;
        direction.Normalize();

        Vector3 laserEndPoint = this.transform.position + direction * 300;
        lastLaserPos = this.followPlayer ? laserEndPoint : lastLaserPos;

        Vector3[] positions = new Vector3[] { 
            this.transform.position, 
            lastLaserPos
            //this.followPlayer ? this.player.transform.position + new Vector3(0, laserHeight, 0) : this.lastPlayerPos
            };

        lastPlayerPos = this.followPlayer ? player.transform.position : lastPlayerPos;

        this.laserTrail.SetPositions(positions);

    }

    void ShotBehaviour() {

        if(!activated) return;

        GameManager.Camera.Shake(5, 20, 0.25f, Vector2.one);

        float distance = Vector2.Distance(player.transform.position, this.lastPlayerPos);

        if(distance < this.laserHeight + this.laserThreshold)
        {
            this.myAttack.DirectAttack(player);
        }

    }

    public void SetAway() {
        ChangeState(LaserRockState.Away);
    }

    public void Reset() {
        ChangeState(LaserRockState.Following, true);
    }

    void AwayBehaviour() {

        Vector3 awayPos = this.player.transform.position + new Vector3(0, 60, 0);
        this.transform.position = Vector3.Lerp(this.transform.position, awayPos, Time.fixedDeltaTime / 5);

    }

    void TimerBehaviour() {

        if(pausedTimer) return;

        if(curShootTimer >= master.LaserActualDelay)
        {
            this.laserAnimator.SetTrigger("Shot");
            this.laserAnimator.SetFloat("PrepSpeed", 0.25f / this.master.LaserActualPrepSpeed);

            this.curShootTimer = 0f;
            this.pausedTimer = true;
        }
        else
        {
            this.curShootTimer += Time.fixedDeltaTime;
        }

    }

    void ChangeState(LaserRockState newState)
    {
        ChangeState(newState, false);
    }

    void ChangeState(LaserRockState newState, bool force = false) {

        if(this.state == LaserRockState.Away && !force) return;

        switch(newState)
        {
            case LaserRockState.Shooting:
                this.followPlayer = false;
                break;
            
            case LaserRockState.Following:
                this.pausedTimer = false;
                this.followPlayer = true;
                break;
        }

        this.state = newState;

    }

}
