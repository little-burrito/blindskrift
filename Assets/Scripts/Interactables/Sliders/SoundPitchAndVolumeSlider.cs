using UnityEngine;
using System.Collections;

public class SoundPitchAndVolumeSlider : Slider {

    public AudioSource sound;

    protected override void UpdateValue( float value ) {
        value = Mathf.Min( 3.0f, Mathf.Max( 0.0f, value ) );
        base.UpdateValue( value );
        sound.pitch = value;
        //sound.volume = 0.2f + ( value / 3 ) * 0.8f;
    }
}
