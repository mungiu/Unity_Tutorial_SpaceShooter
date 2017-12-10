using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;

    //current instance of "GameController"
    private GameController gameController;
    //giving hazard score value
    public int scoreValue;

    private void Start()
    {
        //setting refference to current instance of "GameController", holding our script 
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        //setting gameController refference to "gameControllerObject" components (methods) of type GameController
        if (gameControllerObject != null)
            this.gameController = gameControllerObject.GetComponent<GameController>();

        if (this.gameController == null)
            Debug.Log("Can't find 'GameController' script");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ignoring "Boundary" and "Enemy(ship/meteor)" collision
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
            return;
        
        //if we did not associate an "explosion" in unity with the colided object it won't destroy
        if (explosion != null)
            Instantiate(explosion, this.transform.position, this.transform.rotation);

        //"Player" collision
        if (other.CompareTag("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }

        Destroy(other.gameObject);
        Destroy(this.gameObject);

        //calling AddScore function from current instance of "GameController"
        this.gameController.AddScore(scoreValue);
    }
}
