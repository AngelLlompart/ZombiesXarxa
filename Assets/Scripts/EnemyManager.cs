using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private GameObject _player;

    private NavMeshAgent _navMesh;

    [SerializeField] private Animator enemyAnimator;

    private float damageDone = 20;

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
    public PhotonView _photonView;
    private GameObject _closestPlayer;

    State currentState;

    private GameObject[] _playersInScene;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        //_playersInScene = GameObject.FindGameObjectsWithTag("Player");
        _navMesh = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        currentState = new State.Attack(this.gameObject, _navMesh, _playersInScene); // Create our first state.
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying && Time.timeScale != 0)
        {
            _audioSource.clip = audioClips[Random.Range(0, 3)];
            _audioSource.PlayDelayed(1);
        }

        /*if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        {
            return;
        }*/
        _playersInScene = GameObject.FindGameObjectsWithTag("Player");
        
        GetClosestPlayer();
        currentState = currentState.Process();
        if (_closestPlayer != null)
        {
            //_navMesh.destination = _player.transform.position;
            healthBar.transform.LookAt(_closestPlayer.transform);
        }
        
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
                _closestPlayer.GetComponent<PlayerManager>().Hit(damageDone);
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
        if (PhotonNetwork.InRoom)
        {
            _photonView.RPC("TakeDamage", RpcTarget.All, dmg, _photonView.ViewID);
        }
        else
        {
            TakeDamage(dmg, _photonView.ViewID);
        }
    }

    [PunRPC]
    public void TakeDamage(float dmg, int viewID)
    {
        if (_photonView.ViewID == viewID)
        {
            health -= dmg;
            healthBar.value = health;
            if (health <= 0)
            {
                enemyAnimator.SetTrigger("isDead");
                Destroy(gameObject, 10f);
                Destroy(GetComponent<NavMeshAgent>());
                Destroy(GetComponent<EnemyManager>());
                Destroy(GetComponent<CapsuleCollider>());


                if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && _photonView.IsMine))
                {
                    gameManager.enemiesAlive--;
                }
            } 
        }
    }

    private void GetClosestPlayer()
    {
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject p in _playersInScene)
        {
            if (p != null)
            {
                float distance = Vector3.Distance(p.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    _player = p;
                    minDistance = distance;
                }
            }
        }

        _closestPlayer = _player;
    }
}
