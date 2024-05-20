using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 25f; // Adjust this value for dash speed
    public float bulletTimeScale = 0.3f; // Adjust this value for bullet time scale
    private float fireTimer;
    public float fireRate = 1f;
    public Rigidbody2D rb;
    public Weapon weapon;

    Vector2 moveDirection;
    Vector2 mousePosition;

    bool isDashing = false;
    bool isBulletTime = false;

    [SerializeField] private Animator anim;
    [SerializeField] private float meleeSpeed;
    [SerializeField] private float damage;

    [SerializeField] private Collider2D meleeCollider; // Collider for melee attack range
    [SerializeField] private float meleeDuration; // Duration of the melee attack

    float timeUntilMelee;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Toggle bullet time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBulletTime = !isBulletTime;
            Time.timeScale = isBulletTime ? bulletTimeScale : 1f;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash(new Vector2(moveX, moveY)));
        }

        //Firing
        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            weapon.Fire();
            fireTimer = fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

        // Melee attack
        if (timeUntilMelee < 0f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                MeleeAttack();
                timeUntilMelee = meleeSpeed;
            }
        }
        else
        {
            timeUntilMelee -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        float currentMoveSpeed = isDashing ? moveSpeed * dashSpeed : moveSpeed;
        rb.velocity = moveDirection * currentMoveSpeed;

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f; // Get Aim Angle - Where the player is pointing
        rb.rotation = aimAngle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //other.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log("Enemy hit");
        }
    }

    void MeleeAttack()
    {
        anim.SetTrigger("Sword_attack"); // Trigger melee attack animation
        StartCoroutine(EnableMeleeCollider()); // Enable the melee collider for a short duration
    }

    IEnumerator EnableMeleeCollider()
    {
        meleeCollider.enabled = true; // Enable the melee collider
        yield return new WaitForSeconds(meleeDuration); // Wait for the specified duration
        meleeCollider.enabled = false; // Disable the melee collider
    }

    IEnumerator Dash(Vector2 dashDirection)
    {
        isDashing = true;

        // Normalize the dash direction to maintain consistent speed
        dashDirection.Normalize();

        // Calculate dash distance 
        float dashDistance = 3f; // Adjust this value to suit your game

        // Calculate dash end position
        Vector2 dashEndPosition = rb.position + dashDirection * dashDistance;

        // Move the Rigidbody towards the dash end position using physics
        while ((Vector2.Distance(rb.position, dashEndPosition) > 1f)) // Also checks if distance threshold is small enough to be ignored, preventing an indefinite loop
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
    }
}
