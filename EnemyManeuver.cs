using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManeuver : MonoBehaviour
{
    //we use Vector2 instead of another customized class that would hold 2 values
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;

    private Rigidbody rb;
    private Transform playerTransform;

    public float Tilt;
    public float smoothing;
    public float maxDodge;
    private float targetLocation;
    private float currentSpeed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.velocity.z comes from the complementing "Mover" script
        currentSpeed = rb.velocity.z;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            ////setting random "targetLocation" opposite of current x-axis value = "-Mathf.Sign(...x)"
            //targetLocation = Random.Range(1, maxDodge) * -Mathf.Sign(this.transform.position.x);

            //enemy Auto follow on player
            if (playerTransform != null)
            {
                if (playerTransform.position.x < 0 && transform.position.x < playerTransform.position.x)
                    targetLocation = (transform.position.x - playerTransform.position.x) * Mathf.Sign(-1);
                else if (transform.position.x < 0 && transform.position.x > playerTransform.position.x)
                    targetLocation = playerTransform.position.x - transform.position.x;
                else if (playerTransform.position.x >= 0 && transform.position.x > playerTransform.position.x)
                    targetLocation = (transform.position.x - playerTransform.position.x) * Mathf.Sign(-1);
                else if (transform.position.x >= 0 && transform.position.x < playerTransform.position.x)
                    targetLocation = playerTransform.position.x - transform.position.x;
                else if (playerTransform.position.x < 0 && transform.position.x >= 0)
                    targetLocation = (transform.position.x - playerTransform.position.x) * Mathf.Sign(-1);
                else if (playerTransform.position.x >= 0 && transform.position.x < 0)
                    targetLocation = (playerTransform.position.x - transform.position.x);
            }

            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetLocation = 0;
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
        }
    }

    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetLocation, Time.deltaTime * smoothing);
        //accelerating according to newManeuver
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);

        //constraining enemy ship movement (NOTE: Boudary is instantiated/set in "Player" gameobject)
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

        //enemyShip rotation on side moves (negative tilt)
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -Tilt);
    }
}
