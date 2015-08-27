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

	public float m_distance = 4;

	private SteeringBehaviour m_steering;

	public bool m_separation;
	public bool m_cohesion;
	public bool m_orientation;
	public bool m_constant;

	// Use this for initialization
	void Start () {
		m_velocity = getAngle();
		m_steering = new SteeringBehaviour();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 force = calculateSteeringForce();
		if(force.magnitude <= 0.1f) {
			force = getAngle();
		}
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

		Debug.DrawLine(this.transform.position,this.transform.position + getAngle(), Color.red);

		//I know the velocity -> rotate go
//		transform.rotation = Quaternion.Euler(new );
		float newAngle = Mathf.Atan2(m_velocity.y, m_velocity.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0,0,newAngle));	
	}

	private Vector3 getAngle() {
		float degrees = transform.rotation.eulerAngles.z;
		
		float x = Mathf.Cos(Mathf.Deg2Rad * degrees);
		float y = Mathf.Sin(Mathf.Deg2Rad * degrees);
		
		Vector3 toReturn = new Vector3(x,y,0);
		
		
		return toReturn.normalized;
	}

	private Vector3 m_constantForce() {

		return getAngle();
	}

	private Vector3 calculateSteeringForce() {
		Vector3 v1 = Vector3.zero;

		List<Transform> transforms = new List<Transform>();
		List<Vector3> velocities = new List<Vector3>();

		for(int i = 0; i < m_listaBirds.Count; i++) {
			if(m_listaBirds[i] != this) {
				float distance = (m_listaBirds[i].transform.position - this.transform.position).magnitude;
				if(m_distance >= distance) {
					transforms.Add(m_listaBirds[i].transform);
					velocities.Add(m_listaBirds[i].getVelocity());
				}
			}
		}

		Vector3 separation = m_steering.separation(this.transform,transforms);
		Vector3 cohesion = m_steering.cohesion(this.transform,transforms,m_velocity,maxVelocity);
		Vector3 orientation = m_steering.rotation(getVelocity(),velocities);
		Vector3 constant = m_constantForce();

		if(m_separation) {
			v1 += separation;
		}
		if(m_cohesion) {
			v1 += cohesion;
		}
		if(m_cohesion) {
			v1 += orientation;
		}
		if(m_constant) {
			v1+= constant;
		}

		return v1;
	}

	public Vector3 getVelocity() {
		return m_velocity;
	}

}
