using UnityEngine;
using System.Collections;

public class BreakableWithLoopBehind : Material {

    public AudioSource loopBehind;
    public float muffledLoopVolume = 0.2f;
    public float unmuffledLoopVolume = 1.0f;
    public AudioSource breakSound;
    public int tapsUntilBreak = 3;
    private bool broken;

    void Start() {
        loopBehind.volume = muffledLoopVolume;
    }

    public override void Tap( PlayerTouch playerTouch ) {
        if ( !broken ) {
            tapsUntilBreak--;
            base.Tap( playerTouch );
            if ( tapsUntilBreak <= 0 ) {
                broken = true;
                breakSound.Play();
                loopBehind.volume = unmuffledLoopVolume;
                transform.position = new Vector3( transform.position.x, transform.position.y, transform.position.z + 20.0f );
            }
        }
    }

    public override void Drag( PlayerTouch playerTouch ) {
        if ( !broken ) {
            base.Drag( playerTouch );
        } else {
            SetDragSoundIntensity( 0.0f );
        }
    }
}
