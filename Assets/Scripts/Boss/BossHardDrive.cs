﻿using UnityEngine;
using System.Collections;

public class BossHardDrive : GenericBoss {

    private const float LIMIT_DISTANCE_FROM_CENTER = 8;
    private const float BASE_MOVE_SPEED = 25f;
    private const int BASE_HEALTH = 50; // the base health of the drive
    private const float BASE_PAUSE_BEFORE_SHOOT_DURATION = 1f;
    private const float MIN_TIME_UNTIL_SHOT = 7;
    private const float MAX_TIME_UNTIL_SHOT = 11;
    private const float CENTER_OFFSET = 2f; // the offset so the disk shoots from the edge instead of the center of the transform
    private const float DOWN_OFFSET = 2.5f;
    private const float LEFT_OFFSET = 1f;

    /* movement */
    private float leftLimit;
    private float rightLimit;
    private float moveSpeed;

    /* shooting */
    private float nextShot;
    private GameObject disk;
    private float pauseTime;

    protected override void Awake()
    {
        base.Awake();
        disk = Resources.Load<GameObject>("Ammo/boss_p_disk");
    }

	protected override void Start ()
    {
        health = BASE_HEALTH * gParams.difficulty / 2; // multiply boss health by difficulty level
        moveSpeed = BASE_MOVE_SPEED * 0.25f * difficulty;
        leftLimit = transform.position.x - LIMIT_DISTANCE_FROM_CENTER;
        rightLimit = transform.position.x + LIMIT_DISTANCE_FROM_CENTER;
        pauseTime = BASE_PAUSE_BEFORE_SHOOT_DURATION / difficulty;
        activateDistance = 10f;
	}

    protected override void Update()
    {
        DeathCheck();
        if (!bossActive && playerTransform.position.y > transform.position.y - activateDistance)
        {
            bossActive = true;
            rigidbody.velocity = new Vector3(moveSpeed, 0, 0);
            nextShot = Time.time + MAX_TIME_UNTIL_SHOT;
        }
        else if (bossActive && playerTransform.position.y < transform.position.y - activateDistance)
        {
            bossActive = false;
            rigidbody.velocity = Vector3.zero;
        }
    }
	
	void FixedUpdate()
    {
        if (bossActive)
        {
            BackAndForth();

            if (Time.time > nextShot)
            {
                StartCoroutine(PauseAndShoot(pauseTime, playerTransform.position));
                nextShot = Time.time + Random.Range(MIN_TIME_UNTIL_SHOT, MAX_TIME_UNTIL_SHOT);
            }
        }

    }

    /* move back and forth */
    private void BackAndForth()
    {
        if (transform.position.x > (rightLimit))
        {
            rigidbody.velocity = new Vector3(-moveSpeed, 0, 0);
        }
        else if (transform.position.x < leftLimit)
        {
            rigidbody.velocity = new Vector3(moveSpeed, 0, 0);
        }
    }

    /* pause and shoot */
    IEnumerator PauseAndShoot(float duration, Vector3 playerPosition)
    {
        Vector3 savedVelocity = rigidbody.velocity;
        rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(duration);
        Shoot(playerPosition);
        yield return new WaitForSeconds(duration/2);
        rigidbody.velocity = savedVelocity;
    }

    private void Shoot(Vector3 targetPosition)
    {
        // generate a new object to fire, instantiate with velocity, power, etc.
        Vector3 launchFrom = transform.position + transform.forward * CENTER_OFFSET + Vector3.forward * DOWN_OFFSET + Vector3.left * LEFT_OFFSET;
        GameObject clone = GameObject.Instantiate(disk, launchFrom, Quaternion.identity) as GameObject;
        clone.GetComponent<GenericAmmo>().shotVelocity = (targetPosition - transform.position) * clone.GetComponent<GenericAmmo>().baseSpeed * 0.5f * difficulty;
    }

}
