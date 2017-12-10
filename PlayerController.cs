using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private AudioSource audioSource;
    //private Quaternion calibrationQuaternion;

    public Boundary boundary;
    public float speed;
    public float tilt;

    public GameObject shot;
    public Transform shotSpawn;
    public SimpleTouchpad touchPad;
    public FireTouchpad fireTouchpad;

    public float fireRate;
    private float nextFire;

    //the first frame of game
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        ////Create UI button for this (shouldn;t be in start)
        //CalibrateAccelerometer();
    }

    //executed just before every frame
    private void Update()
    {
        if (fireTouchpad.GetTouchStatus() && Time.time>nextFire)
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
            nextFire = Time.time + fireRate;
        }
    }

    //called before each fixed physics step
    private void FixedUpdate()
    {
        //applying movement from touchPad input
        Vector2 direction = touchPad.GetDirection();
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);

        //accessing the Rigidbody component from unity and assigning value
        rb.velocity = movement * speed;

        //constraining ship movement
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 acceleration = FixAcceleration(accelerationRaw);
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
        //rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    ////setting original Quaternion position based on snapshot
    ////inverting to have a calibrationQuaternion to later check against what new calibrations are
    //void CalibrateAccelerometer()
    //{
    //    //taking snapshot of current Vector3 input acceleration
    //    Vector3 accelerationSnapshot = Input.acceleration;
    //    //creating quaternion(rotation) using above V3 snapshot
    //    Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
    //    calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    //}

    //fixing the acceleration every frame by sending "acceleration"
    //multiplying that by acceleration = an acceleration that is fixed
    //as an offset from the original position

    //Vector3 FixAcceleration(Vector3 acceleration)
    //{
    //    Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
    //    return fixedAcceleration;
    //}
}