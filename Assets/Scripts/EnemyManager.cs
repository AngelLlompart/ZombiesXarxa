using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private GameObject _player;

    private NavMeshAgent _navMesh;

    [SerializeField] private Animator enemyAnimator;

    private float damage = 20;

    public float health = 100;

    public GameManager gameManager;

    [SerializeField] private Slider healthBar;
    
    // Animacio i millora del xoc
    public bool playerInReach;
    public float attackDelayTimer;
    public float howMuchEarlierStartAttackAnimation;
    public float delayBetweenAttacks;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] audioClips; 

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        _player = GameObject.FindGameObjectWithTag("Player");
        _navMesh = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying && Time.timeScale != 0)
        {
            _audioSource.clip = audioClips[Random.Range(0, 3)];
            _audioSource.PlayDelayed(1);
        }
        
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
            //_player.GetComponent<PlayerManager>().Hit(damage);
            playerInReach = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playerInReach)
        {
            attackDelayTimer += Time.deltaTime;
            if(attackDelayTimer >= delayBetweenAttacks - howMuchEarlierStartAttackAnimation && attackDelayTimer <= delayBetweenAttacks)
            {
                enemyAnimator.SetTrigger("isAttacking");
            }
            if(attackDelayTimer >= delayBetweenAttacks)
            {
                _player.GetComponent<PlayerManager>().Hit(damage);
                attackDelayTimer = 0;
            }
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInReach = false;
            attackDelayTimer = 0;
        }
    }


    public void Hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            gameManager.enemiesAlive--;
            enemyAnimator.SetTrigger("isDead");
            Destroy(gameObject, 10f);
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<EnemyManager>());
            Destroy(GetComponent<CapsuleCollider>());

        }

        healthBar.value = health;
        Debug.Log(health);
    }
}
