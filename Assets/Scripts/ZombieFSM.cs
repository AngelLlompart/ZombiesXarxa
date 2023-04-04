using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFSM : MonoBehaviour
{

    // An array of GameObjects to store all the agents
    private GameObject[] _players;
    private NavMeshAgent zombie;
    State currentState;


    void Start()
    {

        // Grab everything with the 'ai' tag
        zombie = this.GetComponent<NavMeshAgent>();
        _players = GameObject.FindGameObjectsWithTag("Player");
        currentState = new State.Attack(this.gameObject, zombie, _players); // Create our first state.
    }

    // Update is called once per frame
    void Update()
    {
        // Trobam a l'enemic
        currentState = currentState.Process(); // Calls Process method to ensure correct state is set.
    }
}

