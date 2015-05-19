using UnityEngine;
using System.Collections;

public class Slider : Material {

    public float range = 10.0f;
    public float rangePadding = 1.0f;
    public float minValue = 0.0f;
    public float maxValue = 3.0f;
    public float value = 0.0f;
    public GameObject sliderKnob;

	// Use this for initialization
	protected virtual void Start () {
        UpdateValue( value );
	}

    public override void BeginDrag( PlayerTouch playerTouch ) {
        base.BeginDrag( playerTouch );
    }

    public override void Drag(PlayerTouch playerTouch) {
        base.Drag( playerTouch );
        float nonPaddedRange = range * transform.localScale.y - rangePadding * transform.localScale.y * 2;
        float basePosition = ( transform.position.y - nonPaddedRange * 0.5f );
        float rangeValue = Mathf.Max( 0.0f, Mathf.Min( nonPaddedRange, playerTouch.transform.position.y - basePosition ) );
        SetValue( ( maxValue - minValue ) * rangeValue / nonPaddedRange + minValue );
    }

    public override void EndDrag( PlayerTouch playerTouch ) {
        if ( isDragged ) {
            Drag( playerTouch );
        }
        base.EndDrag( playerTouch );
    }

    public void SetValue( float value ) {
        this.value = Mathf.Min( maxValue, Mathf.Max( minValue, value ) );
        float nonPaddedRange = range * transform.localScale.y - rangePadding * transform.localScale.y * 2;
        float basePosition = ( transform.position.y - nonPaddedRange * 0.5f );
        sliderKnob.transform.position = new Vector3( transform.position.x, basePosition + ( ( ( value - minValue ) / ( maxValue - minValue ) ) * nonPaddedRange ), transform.position.z );
        UpdateValue( value );
    }

    protected virtual void UpdateValue( float value ) {

    }

    void OnDrawGizmos() {
        Gizmos.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
        Gizmos.DrawCube( transform.position, new Vector3( transform.localScale.x, range * transform.localScale.y - rangePadding * transform.localScale.y * 2.0f, transform.localScale.z ) );
        Gizmos.color = new Color( 1.0f, 1.0f, 0.0f, 0.5f );
        Gizmos.DrawCube( new Vector3( transform.position.x, transform.position.y + range * transform.localScale.y * 0.5f - rangePadding * transform.localScale.y * 0.5f, transform.position.z ), new Vector3( transform.localScale.x, rangePadding * transform.localScale.y, transform.localScale.z ) );
        Gizmos.DrawCube( new Vector3( transform.position.x, transform.position.y - range * transform.localScale.y * 0.5f + rangePadding * transform.localScale.y * 0.5f, transform.position.z ), new Vector3( transform.localScale.x, rangePadding * transform.localScale.y, transform.localScale.z ) );
    }
}
