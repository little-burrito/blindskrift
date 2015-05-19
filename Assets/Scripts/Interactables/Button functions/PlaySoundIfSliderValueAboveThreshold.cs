using UnityEngine;
using System.Collections;
//using UnityEditor;

public class PlaySoundIfSliderValueAboveThreshold : ButtonFunction {

    public Slider slider;
    public float threshold;
    public AudioSource sound;
    public bool resetSliderValue;
    public bool resetSliderToInitialSliderValue;
    public float sliderResetValue;
    private bool isResetting;
    private float resetMultiplier = 0.95f;
    private float lastKnownSliderValue;

    void Start() {
        if ( resetSliderToInitialSliderValue ) {
            sliderResetValue = slider.value;
        }
    }

    void Update() {
        if ( isResetting && slider.value == lastKnownSliderValue ) {
            if ( slider.value > sliderResetValue ) {
                slider.SetValue( slider.value * resetMultiplier );
            } else {
                isResetting = false;
            }
            lastKnownSliderValue = slider.value;
        }
    }

    public override void Function( PlayerTouch playerTouch ) {
        if ( slider.value > threshold ) {
            sound.Play();
            if ( resetSliderValue ) {
                slider.SetValue( sliderResetValue );
            }
        } else {
            if ( resetSliderValue ) {
                isResetting = true;
                lastKnownSliderValue = slider.value;
            }
        }
    }
}
/*
[ CustomEditor( typeof( PlaySoundIfSliderValueAboveThreshold ) ) ]
public class PlaySoundIfSliderValueAboveThresholdEditor : Editor {
    public override void OnInspectorGUI() {
        PlaySoundIfSliderValueAboveThreshold buttonFunction = target as PlaySoundIfSliderValueAboveThreshold;
        base.OnInspectorGUI();
        if ( buttonFunction.resetSliderValue ) {
            buttonFunction.resetSliderToInitialSliderValue = EditorGUILayout.Toggle( "Reset Slider To Initial Slider Value", buttonFunction.resetSliderToInitialSliderValue );
            if ( !buttonFunction.resetSliderToInitialSliderValue ) {
                buttonFunction.sliderResetValue = EditorGUILayout.FloatField( "Slider Reset Value", buttonFunction.sliderResetValue );
            }
        }
    }
}
*/