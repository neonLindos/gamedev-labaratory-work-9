using UnityEngine;
using System.Collections;

public class EnemyFSM : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State currentState;

    public Transform player;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public Transform[] patrolPoints;

    private CharacterController playerController;
    public float knockbackDistance = 3f;
    public float distancePatrolPoint = 1f;

    public AudioSource atackSound;
    private int currentPatrolIndex = 0;


    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerController = playerObj.GetComponent<CharacterController>();
        }
        else
        {
            Debug.LogError("Игрок с тегом 'Player' не найден!");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (distance < chaseDistance)
                    currentState = State.Chase;
                break;

            case State.Chase:
                Chase();
                if (distance < attackDistance)
                    currentState = State.Attack;
                else if (distance > chaseDistance)
                    currentState = State.Patrol;
                break;

            case State.Attack:
                Attack();
                if (distance > attackDistance)
                    currentState = State.Chase;
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        Transform target = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < distancePatrolPoint)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (playerController != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 knockback = dir * knockbackDistance;
            playerController.Move(knockback * Time.deltaTime);
            atackSound.Play();
        }
    }
}
