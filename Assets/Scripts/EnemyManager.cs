using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private GameObject _player;

    private NavMeshAgent _navMesh;

    [SerializeField] private Animator enemyAnimator;

    private float damage = 20;

    public float health = 100;

    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _navMesh.destination = _player.transform.position;

        if (_navMesh.velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.GetComponent<PlayerManager>().Hit(damage);
        }
    }

    public void Hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            gameManager.enemiesAlive--;
            Destroy(gameObject);
        }
        Debug.Log(health);
    }
}
