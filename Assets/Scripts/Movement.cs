using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	private Vector3 m_velocity;
	public float mase;
	
	public float maxVelocity;

	[HideInInspector]
	public float[] limitX;
	[HideInInspector]
	public float[] limitY;
	[HideInInspector]
	public Vector3 initialVelocity;
	[HideInInspector]
	public List<Movement> m_listaBirds;

	// Use this for initialization
	void Start () {
		m_velocity = initialVelocity;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 force = calculateSteeringForce();
		
		//f = m * a;
		Vector3 aceleration = force / mase;
		
		//Delta Vel = a * t
		Vector3 deltaVel = aceleration * Time.deltaTime;
		
		//new Vel
		m_velocity += deltaVel;

		
		//truncate vel
		if( m_velocity.magnitude >= maxVelocity) {
			m_velocity -= deltaVel;
		}
		
		//calculate new position
		transform.localPosition += m_velocity * Time.deltaTime;
		
		//circle world
		if( transform.localPosition.x < limitX[0]) {
			transform.localPosition = new Vector3(limitX[1],transform.localPosition.y,transform.localPosition.z);
		}
		if( transform.localPosition.x > limitX[1]) {
			transform.localPosition = new Vector3(limitX[0],transform.localPosition.y,transform.localPosition.z);
		}
		if( transform.localPosition.y < limitY[0]) {
			transform.localPosition = new Vector3(transform.localPosition.x,limitY[1],transform.localPosition.z);
		}
		if( transform.localPosition.y > limitY[1]) {
			transform.localPosition = new Vector3(transform.localPosition.x,limitY[0],transform.localPosition.z);
		}
	}

	private Vector3 calculateSteeringForce() {
		Vector3 v1 = Vector3.up;
		return v1;
	}

	public Vector3 getVelocity() {
		return m_velocity;
	}
}
