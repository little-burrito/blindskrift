using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject respawnPoint;

    void OnTriggerEnter2D( Collider2D other ) {
        if ( other.tag == "Player" ) {
            PlayerTouch playerTouch = other.gameObject.GetComponent<PlayerTouch>();
            playerTouch.RespawnAtGameObject( respawnPoint );
        }
    }
}
