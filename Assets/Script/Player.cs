using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    RaycastHit camerahit;
    Ray rayFromCamera;
    GameObject bullet_prefab;
    Transform gunfirepoint;

    Behavior behavior;

    [Header("-About Movement")]
    public float MoveSpeed = 5;

    [Header("-Shooting Ability")]
    public float FirePerSec = 8;
    public float MouseSensitve = 10;

    private float stack_for_fire = 1f;

    public bool isInputable
    {
        set; get;
    }

    public override void Start()
    {
        base.Start();
        bullet_prefab = Resources.Load<GameObject>("Prefab/Bullet");
        gunfirepoint = transform.Find("GunFirePoint");
        behavior = new Behavior(ShootBullet);
        isInputable = true;
    }
    
    void Update()
    {
        if (isInputable)
        {
            MovePlayerByInput();
            LookAtTheMousePoint();
        }
        //LookAtTheTarget();
        behavior();
    }

    void MovePlayerByInput()
    {
        Vector3 move_vector_normal = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        move_vector_normal = move_vector_normal.normalized;
        transform.Translate(move_vector_normal * Time.deltaTime * MoveSpeed, Space.World);
    }
    void LookAtTheMousePoint()
    {
        rayFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 FloorPoint = rayFromCamera.origin - rayFromCamera.direction * rayFromCamera.origin.y / rayFromCamera.direction.y;
        transform.LookAt(FloorPoint);
    }
    void LookAtTheTarget()
    {
        float turnY = Input.GetAxis("Mouse X") * MouseSensitve / 4.0f;
        transform.localRotation *= Quaternion.Euler(0, turnY, 0);
    }
    void ShootBullet()
    {
        stack_for_fire += Time.deltaTime;
        if (Input.GetMouseButton(0) && 1f/FirePerSec < stack_for_fire)
        {
            Instantiate<GameObject>(bullet_prefab, gunfirepoint.position, gunfirepoint.rotation);
            stack_for_fire = 0f;
        }
    }
    protected override void Die()
    {
        dead = true;
        GameObject.Find("Option").GetComponent<Option>().GameOver();
    }
}
