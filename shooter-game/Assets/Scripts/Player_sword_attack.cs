using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_sword_attack : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float meleeSpeed;

    [SerializeField] private float damage;

    float timeUntilMelee;

    private void Update()
    {
        if (timeUntilMelee < 0f) { 
            if (Input.GetMouseButtonDown(1))
            {
                anim.SetTrigger("Sword_attack");
                timeUntilMelee = meleeSpeed;
            }
        } else {
            timeUntilMelee -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            //other.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log("Enemy hit");
        }
 
    }
    
        
    }

