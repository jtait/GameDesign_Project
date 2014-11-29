﻿using UnityEngine;
using System.Collections;

public class ByteShip_0 : GenericEnemy
{
    private const float SHOT_OFFSET = 2f;
    private const float BASE_SHOT_VELOCITY = 2f;

    private float nextShot;

    void FixedUpdate()
    {
        this.Move();
        this.Shoot();
    }

    protected override void Start()
    {
        /* flip the transform over */
        transform.localEulerAngles = new Vector3(0, 180, 0);

        /* override all basic parameters */
        ammunition = Resources.Load<GameObject>("Ammo/EnemyLaser");
        health = 4;
        pointValue = 800 * difficulty;
        fireRate = Mathf.Max((5 / (float)difficulty), 0.3f);
        print(fireRate);
        baseForwardVelocity = 0.7f;
        nextShot = 0f;
    }

    /* override base class Move() function */
    void Move()
    {
        rigidbody.velocity = Vector3.down * baseForwardVelocity;
    }

    void Shoot()
    {
        if (ammunition == null) return;

        if (Time.time > nextShot)
        {
            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y - SHOT_OFFSET, transform.position.z);
            GameObject clone = GameObject.Instantiate(ammunition, launchFrom, Quaternion.identity) as GameObject;
            float shotVelocity = rigidbody.velocity.y - BASE_SHOT_VELOCITY;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, shotVelocity, 0);
            nextShot = Time.time + fireRate;
        }
    }

    protected override void Death()
    {
        base.Death();
    }

}
