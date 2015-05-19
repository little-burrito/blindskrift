using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject objectToFollow;

    void LateUpdate() {
        this.gameObject.transform.position = new Vector3( objectToFollow.transform.position.x, objectToFollow.transform.position.y, this.gameObject.transform.position.z );
    }
}
