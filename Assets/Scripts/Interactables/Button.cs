using UnityEngine;
using System.Collections;

public class Button : Material {

    public AudioSource pressSound;
    public AudioSource releaseSound;

    public ButtonFunction buttonFunction;

    public override void Tap( PlayerTouch playerTouch ) {
    }

    public override void Drag( PlayerTouch playerTouch ) {
        base.Drag( playerTouch );
    }
    public override void DontDrag( PlayerTouch playerTouch ) {
        base.DontDrag( playerTouch );
    }

    public override void BeginDrag( PlayerTouch playerTouch ) {
        base.BeginDrag( playerTouch );
        pressSound.Play();
        Function( playerTouch );
    }

    public override void EndDrag( PlayerTouch playerTouch ) {
        if ( isDragged ) {
            releaseSound.Play();
        }
        base.EndDrag( playerTouch );
    }

    protected virtual void Function( PlayerTouch playerTouch ) {
        if ( buttonFunction != null ) {
            buttonFunction.Function( playerTouch );
        }
    }

}
