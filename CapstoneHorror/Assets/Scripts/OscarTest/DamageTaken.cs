using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageTaken : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    //syncVar means the attribute value must be synchronized among game instances
    [SyncVar]
    private int currentHealth;
    [SerializeField]
    private string enemyTag;
    [SerializeField]
    private bool destroyOnDeath;

    private Vector3 initialPos;

    private void Start()
    {
        this.currentHealth = this.maxHealth;
        this.initialPos = this.transform.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == this.enemyTag)
        {
            this.TakeDamage(3);
            Destroy(collider.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        if (this.isServer)
        {
            this.currentHealth -= amount;

            if (this.currentHealth <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.currentHealth = this.maxHealth;
                RpcRespawn();
            }
        }
    }

    //commands executed in client and not the server, must be called as we want it to apply for players to respawn in initial position on death
    [ClientRpc]
    void RpcRespawn()
    {
        this.transform.position = this.initialPos;
    }
}


