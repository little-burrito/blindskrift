using UnityEngine;
using System.Collections;

public class Material : MonoBehaviour {

    public bool canBeSimultaneous;

    public AudioSource[] tapSounds;
    public AudioSource[] dragSounds;
    [ HideInInspector ]
    public float dragVolumeMultiplier = 1.0f;
    protected AudioSource dragSound;
    protected int lastTapSound = 0;
    protected int lastDragSound = 0;

    protected bool isDragged;

    protected AudioSource GetTapSound() {
        if ( tapSounds.Length > 0 ) {
            int tapSound = Random.Range( 0, tapSounds.Length );
            if ( tapSound == lastTapSound ) {
                tapSound++;
                if ( tapSound >= tapSounds.Length ) {
                    tapSound = 0;
                }
            }
            lastTapSound = tapSound;
            return tapSounds[ tapSound ];
        }
        return null;
    }

    protected AudioSource GetDragSound() {
        if ( dragSounds.Length > 0 ) {
            int dragSound = Random.Range( 0, dragSounds.Length );
            if ( dragSound == lastDragSound ) {
                dragSound++;
                if ( dragSound >= dragSounds.Length ) {
                    dragSound = 0;
                }
            }
            lastDragSound = dragSound;
            return dragSounds[ dragSound ];
        }
        return null;
    }

    protected void PlayTapSound() {
        if ( tapSounds.Length > 0 ) {
            GetTapSound().Play();
        }
    }

    protected void PlayDragSound() {
        if ( dragSounds.Length > 0 ) {
            dragSound = GetDragSound();
            dragSound.timeSamples = Random.Range( 0, dragSound.clip.samples - 1 );
            dragSound.Play();
        }
    }

    protected void SetDragSoundIntensity( float speedMagnitude ) {
        if ( dragSounds.Length > 0 ) {
            if ( dragSound == null ) {
                dragSound = GetDragSound();
            }
            dragSound.volume = Mathf.Min( Mathf.Abs( speedMagnitude ) * dragVolumeMultiplier / 100.0f, 1.0f );
        }
    }

    protected void StopDragSound() {
        if ( dragSounds.Length > 0 ) {
            if ( dragSound == null ) {
                dragSound = GetDragSound();
            }
            dragSound.Stop();
        }
    }

    public virtual void Tap( PlayerTouch playerTouch ) {
        PlayTapSound();
    }

    public virtual void BeginDrag( PlayerTouch playerTouch ) {
        PlayDragSound();
        isDragged = true;
    }

    public virtual void Drag( PlayerTouch playerTouch ) {
        SetDragSoundIntensity( playerTouch.rb.velocity.magnitude );
    }

    public virtual void DontDrag( PlayerTouch playerTouch ) {
        SetDragSoundIntensity( 0.0f );
    }

    public virtual void EndDrag( PlayerTouch playerTouch ) {
        StopDragSound();
        isDragged = false;
    }
}
