using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnEnemy : NetworkBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float spawnInterval = 3.0f;
    [SerializeField]
    private float enemySpeed = 3.0f;

    //same as OnStart method but calling from server side, and invokeRepeating to call spawn Enemy according to our interval time
    public override void OnStartServer()
    {
        InvokeRepeating("SpawnEnemies", this.spawnInterval, this.spawnInterval);
    }

    //spawn Enemy method will instantiate an enemy in a random position using the network server to replicate it among all instances of the game
    void SpawnEnemies()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-13.0f, 13.0f), this.transform.position.y, this.transform.position.z);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject;
        enemy.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -this.enemySpeed, -this.enemySpeed);
        NetworkServer.Spawn(enemy);
        enemy.transform.position += transform.forward * Time.deltaTime * enemySpeed;
        Destroy(enemy, 15);
    }
}
