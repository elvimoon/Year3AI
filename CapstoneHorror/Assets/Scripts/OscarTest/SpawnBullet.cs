using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnBullet : NetworkBehaviour
{
    public GameObject bulletType; //The prefab for the bullet

    [SerializeField]
    private float bulletSpeed;

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            CmdShoot(); ///The instantiate code must be called server-side, so a wrapper to make it a command is used
        }
    }

    [Command] ///Commands are always called server-side; a client will send a call to the server to call the command
    void CmdShoot()
    {
        GameObject bullet = Instantiate(bulletType, this.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * -bulletSpeed;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 3.0f);
    }
}
