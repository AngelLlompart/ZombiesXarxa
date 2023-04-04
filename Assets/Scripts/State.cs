using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class State 
{
      public enum STATE
    {
        ATTACK,
        HIDE,
        RUNAWAY,
        RAMPAGE
    };

    // 'Events' - where we are in the running of a STATE.
    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    };

    public STATE name; // To store the name of the STATE.
    protected EVENT stage; // To store the stage the EVENT is in.
    protected GameObject npc; // To store the NPC game object.

    protected  GameObject[]
        players; // To store the transform of the player. This will let the guard know where the player is, so it can face the player and know whether it should be shooting or chasing (depending on the distance).

    protected State
        nextState; // This is NOT the enum above, it's the state that gets to run after the one currently running (so if IDLE was then going to PATROL, nextState would be PATROL).

    protected NavMeshAgent agent; // To store the NPC NavMeshAgent component.

    protected GameObject _closePlayer;
    
    public float distance = 6;

    private float angleMax = 60;

    protected GameManager _gameManager = GameObject.FindObjectOfType<GameManager>();

    // Constructor for State
    public State(GameObject _npc, NavMeshAgent _agent, GameObject[] _players)
    {
        npc = _npc;
        agent = _agent;
        stage = EVENT.ENTER;
        players = _players;
    }

    // Phases as you go through the state.
    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    } // Runs first whenever you come into a state and sets the stage to whatever is next, so it will know later on in the process where it's going.

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    } // Once you are in UPDATE, you want to stay in UPDATE until it throws you out.

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    } // Uses EXIT so it knows what to run and clean up after itself.

    // The method that will get run from outside and progress the state through each of the different stages.
    public State Process()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; // Notice that this method returns a 'state'.
        }

        return this; // If we're not returning the nextState, then return the same state.
    }
    
    public void GetClosestPlayer()
    {
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = npc.transform.position;

        foreach (GameObject p in players)
        {
            if (p != null)
            {
                float distance = Vector3.Distance(p.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    _closePlayer = p;
                    minDistance = distance;
                }
            }
        }
    }

    public class Attack : State
    {
        public Attack(GameObject _npc, NavMeshAgent _agent, GameObject[] _players)
            : base(_npc, _agent, _players)
        {
            name = STATE.ATTACK; // Set name to correct state.
            agent.speed = 4; // How fast your character moves
        }
        
        public override void Enter()
        {
            agent.isStopped = false;
            base.Enter();
        }


        public override void Update()
        {
           
                GetClosestPlayer();
                agent.destination = _closePlayer.transform.position;
                if (npc.GetComponent<EnemyManager>().health <= 50 && _gameManager.round >= 4)
                {
                    nextState = new Rampage(npc, agent, players);
                    stage = EVENT.EXIT;
                } 
            
            

            
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
    
    public class Rampage : State
    {
        float timer = 0;
        private float zigZagDelta;
        private float zigZagDistance = 3;
        public Rampage(GameObject _npc, NavMeshAgent _agent, GameObject[] _players)
            : base(_npc, _agent, _players)
        {
            name = STATE.RAMPAGE; // Set name to correct state.
            agent.speed = 8; // How fast your character moves
        }
        
        public override void Enter()
        {
            agent.isStopped = false;
            base.Enter();
        }


        public override void Update()
        {  
            GetClosestPlayer();

            zigZagDelta += Time.deltaTime * 2;

            Vector3 movementPos = _closePlayer.transform.position;
                movementPos += ZigZagStrafe(); // add the offset from the zigzag
  
            agent.SetDestination(movementPos);
        }
        
        Vector3 ZigZagStrafe()
        {
            // using sinus to generate zigzag between -1 and 1 , multiplying with some magnitude
            float t = Mathf.Sin(zigZagDelta) * zigZagDistance;
            // this is in local space
            Vector3 zigZagDisplacementLocal = Vector3.right * t;
            // this is now in world space
            Vector3 zigZagDisplacementWorld = _closePlayer.transform.TransformDirection(zigZagDisplacementLocal);
  
            return zigZagDisplacementWorld;
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
    
    
}
