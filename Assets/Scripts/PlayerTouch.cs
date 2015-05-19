using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTouch : MonoBehaviour {

    private bool isDragging;
    private List<Material> materials;
    [ HideInInspector ]
    public Rigidbody2D rb;
    public bool controlsEnabled = true;
    public bool isAnimatingTowardsRespawn;
    public Vector3 respawnPosition;
    public float resetMultiplier = 0.05f;
    public float closestResetMargin = 0.05f;
    public AudioSource playerTakeDamage;

    private Vector3 respawnDistance;

    public float sensitivity = 10.0f;

    private float mobileTapTime = 0.3f;
    private float mobileTapDownTime;
    private float mobileTapCancelSpeed = 12.0f;
    private bool mobileTapCancelled;

	// Use this for initialization
	void Start () {
        materials = new List<Material>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if ( isAnimatingTowardsRespawn ) {
            if ( isAnimatingTowardsRespawn ) {
                Vector3 distance = respawnPosition - transform.position;
                if ( distance.magnitude > respawnDistance.magnitude * closestResetMargin ) {
                    transform.position += respawnDistance * resetMultiplier;
                } else {
                    transform.position = respawnPosition;
                    isAnimatingTowardsRespawn = false;
                    controlsEnabled = true;
                    if ( Input.GetMouseButton( 0 ) ) {
                        BeginDrag();
                    }
                }
            }
        }

        // Controls
        if ( controlsEnabled ) {
            // Cursor lock apparently needs to be done in update... this seems weird to me.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // PC controls
            // Move the touch point
            // Override if mobile
            if ( Input.touchCount == 0 ) {
                rb.velocity = new Vector3( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ), 0.0f ) * sensitivity;
            }
            // Process the input
            if ( Input.GetMouseButtonDown( 0 ) ) {
                // Override if mobile
                if ( Input.touchCount == 0 ) {
                    Tap();
                    BeginDrag();
                }
            }
            if ( isDragging ) {
                UpdateDrag();
            }
            if ( Input.GetMouseButtonUp( 0 ) ) {
                // Override if mobile
                if ( Input.touchCount == 0 ) {
                    EndDrag();
                }
            }
            // Mobile controls
            if ( Input.touchCount > 0 ) {
                foreach ( Touch touch in Input.touches ) {
                    if ( touch.fingerId == 0 ) {
                        if ( touch.deltaPosition.magnitude != 0 ) {
                            rb.velocity = new Vector3( touch.deltaPosition.x, touch.deltaPosition.y, 0.0f )/* * sensitivity*/;
                            if ( touch.deltaPosition.magnitude > mobileTapCancelSpeed ) {
                                mobileTapCancelled = true;
                            }
                        } else {
                            rb.velocity *= 0.8f;
                            if ( rb.velocity.magnitude < 0.1f ) {
                                rb.velocity = Vector3.zero;
                            }
                        }
                        if ( touch.phase == TouchPhase.Began ) {
                            BeginDrag();
                            mobileTapDownTime = Time.fixedTime;
                            mobileTapCancelled = false;
                        }
                        // isDragging uses the same function as for PC
                        // if ( isDragging ) {
                        //     UpdateDrag();
                        // }
                        if ( touch.phase == TouchPhase.Ended ) {
                            EndDrag();
                            if ( !mobileTapCancelled ) {
                                if ( Time.fixedTime - mobileTapDownTime <= mobileTapTime ) {
                                    Tap();
                                }
                            }
                        }
                    }
                }
            }
        } else {
            rb.velocity = Vector3.zero;
        }
	}

    public void RespawnAtGameObject( GameObject respawnObject ) {
        controlsEnabled = false;
        isAnimatingTowardsRespawn = true;
        respawnPosition = new Vector3( respawnObject.transform.position.x, respawnObject.transform.position.y, transform.position.z );
        respawnDistance = respawnPosition - transform.position;
        if ( isDragging ) {
            EndDrag();
        }
        playerTakeDamage.Play();
    }

    void Tap() {
        Material zmostMaterial = null;
        foreach ( Material material in materials ) {
            // Only play the material on top unless it can be played simulatenously
            if ( material.canBeSimultaneous ) {
                material.Tap( this );
            }
            // Find the material on top
            if ( zmostMaterial != null ) {
                if ( material.gameObject.transform.position.z < zmostMaterial.gameObject.transform.position.z ) {
                    zmostMaterial = material;
                }
            } else {
                zmostMaterial = material;
            }
        }
        // Play the material on top unless it's already been played
        if ( zmostMaterial != null ) {
            if ( !zmostMaterial.canBeSimultaneous ) {
                zmostMaterial.Tap( this );
            }
        }
    }

    void BeginDrag() {
        isDragging = true;
        foreach ( Material material in materials ) {
            material.BeginDrag( this );
        }
    }

    void UpdateDrag() {
        Material zmostMaterial = null;
        foreach ( Material material in materials ) {
            // Only play the material on top unless it can be played simulatenously
            material.DontDrag( this );
            if ( material.canBeSimultaneous ) {
                material.Drag( this );
            }
            // Find the material on top
            if ( zmostMaterial != null ) {
                if ( material.gameObject.transform.position.z < zmostMaterial.gameObject.transform.position.z ) {
                    zmostMaterial = material;
                }
            } else {
                zmostMaterial = material;
            }
        }
        // Play the material on top unless it's already been played
        if ( zmostMaterial != null ) {
            if ( !zmostMaterial.canBeSimultaneous ) {
                zmostMaterial.Drag( this );
            }
        }
    }

    void EndDrag() {
        isDragging = false;
        foreach ( Material material in materials ) {
            material.EndDrag( this );
        }
    }

    void OnTriggerEnter2D( Collider2D other ) {
        Material material = other.gameObject.GetComponent<Material>();
        AddMaterial( material );
    }

    void OnTriggerExit2D( Collider2D other ) {
        Material material = other.gameObject.GetComponent<Material>();
        RemoveMaterial( material );
    }

    void AddMaterial( Material material ) {
        if ( material != null ) {
            if ( !materials.Contains( material ) ) {
                materials.Add( material );
                if ( isDragging ) {
                    material.BeginDrag( this );
                }
            }
        }
    }

    void RemoveMaterial( Material material ) {
        if ( material != null ) {
            material.EndDrag( this );
            materials.Remove( material );
        }
    }
}
