using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{

    [SerializeField] private float knockbackForce = 2f;

    public PlayerMovement player;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            Debug.Log("Rata si player");

            Vector3 knockDirection = transform.position - collision.transform.position;

            if (player != null)
            {
                player.Knockback(knockDirection, knockbackForce);
                StartCoroutine(knockbackDuration());
            }
        }
    }
    public void StopKnockback()
    {
        rb.velocity = Vector3.zero; // Stops movement
                                    // You can also reset any flags like isKnockedBack = false;
    }

    IEnumerator knockbackDuration()
    {
        yield return new WaitForSeconds(2f);

        if (player != null)
        {
            StopKnockback(); // You need to implement this in your player's script
        }
    }

    

}
