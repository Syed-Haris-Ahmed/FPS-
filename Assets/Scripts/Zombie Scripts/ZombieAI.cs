using UnityEngine;
using System.Collections;
using TMPro;

public class ZombieAI : MonoBehaviour
{
    private Transform player;             // The player's position.
    public float moveSpeed = 5f;         // Enemy's movement speed.
    public float damage = 10f;           // Amount of damage dealt to the player.
    public float attackInterval = 2f;    // Time interval between attacks.
    public float detectionRadius = 10f;  // Radius within which the zombie detects the player.
    public float rotationSpeed = 5f;     // Speed at which the zombie rotates towards the player.

    private float nextAttackTime;        // Time when the enemy can attack again.
    private bool isAttacking = false;    // Flag to check if enemy is attacking.
    private CharacterController controller;
    private Animator animator;

    private bool isRunning;
    private bool isIdle;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");

    void Start()
    {
        Debug.Log("Starting ExampleScript...");

        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Debug.Log("Player GameObject found.");
            // Get the Transform component of the player GameObject
            this.player = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure the player has the 'Player' tag.");
        }

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // zombie animator
    }

    void Update()
    {
        if (IsPlayerInRange())
        {
            isIdle = false;
            isRunning = true;
            RotateTowardsPlayer();
            MoveTowardsPlayer();
            
            if (Time.time >= nextAttackTime && !isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else {
            isIdle = true;
            isRunning = false;
        }
        
        animator.SetBool(IsRunning, isRunning);
        animator.SetBool(IsIdle, isIdle);
    }

    bool IsPlayerInRange()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            return distanceToPlayer <= detectionRadius;
        }
        return false;
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;
            controller.Move(move);
        }
    }

    void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Keep the rotation in the horizontal plane
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator AttackPlayer()
    {
        Debug.Log("isAttacking");
        isAttacking = true;
        // Deal damage to player
        // PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        // if (playerHealth != null)
        // {
        //     playerHealth.TakeDamage(damage);
        // }
        // Set next attack time
        nextAttackTime = Time.time + attackInterval;
        // Wait for the attack interval before allowing another attack
        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }
}
