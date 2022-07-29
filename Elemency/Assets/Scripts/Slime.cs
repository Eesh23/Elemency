using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    //FireSlime Script
    [Header("Attributes")]
    public EnemySlime fireSlimeSO;
    public float slimeHealth;
    public float slimeDamage;
    public float slimeWalkSpeed;
    private Rigidbody2D slimeRB;

    [Header("Patrol AI")]
    [SerializeField] private bool mustPatrol;
    [SerializeField] private bool mustTurn;

    public Collider2D wallCollider;
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    private Player player;

    private void Start()
    {
        slimeRB = GetComponent<Rigidbody2D>();
        slimeHealth = fireSlimeSO.health;
        slimeDamage = fireSlimeSO.damage;
        slimeWalkSpeed = fireSlimeSO.walkSpeed;
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            // If circle is touching ground, dont flip, else flip
            mustTurn = !(Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer));
        }
    }
    
    private void Patrol()
    {
        if (mustTurn || wallCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        slimeRB.velocity = new Vector2(slimeWalkSpeed * Time.fixedDeltaTime, slimeRB.velocity.y);
    }

    private void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        slimeWalkSpeed *= -1;
        mustPatrol = true;
    }

    private void takeDamage(float damage)
    {
        slimeHealth -= damage;
        if(slimeHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject collisionObject = other.gameObject;
        if(collisionObject.tag == "FireMagic")
        {
            takeDamage(player.magicPower * 0.5f);
        }
        else if(collisionObject.tag == "WaterMagic")
        {
            takeDamage(player.magicPower * 1.5f);
        }
    }


}
