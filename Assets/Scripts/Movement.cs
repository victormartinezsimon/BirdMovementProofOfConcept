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
	[HideInInspector]
	public GameObject m_wallUp;
	[HideInInspector]
	public GameObject m_wallDown;
	[HideInInspector]
	public GameObject m_wallLeft;
	[HideInInspector]
	public GameObject m_wallRight;

	public float m_distance = 4;

	private SteeringBehaviour m_steering;

	public float m_separation = 1;
	public float m_cohesion = 1;
	public float m_orientation = 1;
	public float m_constant = 1;
	public float m_avoid = 10;
	public float m_wallAvoid = 1;

	public bool debug = true;

	// Use this for initialization
	void Start () {
		m_velocity = getAngle();
		m_steering = new SteeringBehaviour();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 force = calculateSteeringForce();
//		if(force.magnitude <= 0.1f) {
//			force = getAngle();
//		}
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

		if(debug) {
			Debug.DrawLine(this.transform.position, this.transform.position + force, Color.green);
			Debug.DrawLine(this.transform.position, this.transform.position + m_velocity, Color.blue);
		}

		if(Input.GetKeyDown(KeyCode.D)) {
			debug = !debug;
		}
		

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

		Vector3 avoidMouse = Vector3.zero;
		if(m_avoid != 0.0f) {
			if(Input.GetMouseButtonDown(0)) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				avoidMouse = m_steering.flee(this.transform, pos, m_velocity, maxVelocity);
			}
		}

		//avoid walls
		Vector3 nextPos = this.transform.position +  m_velocity * Time.deltaTime;
		Vector3 walls = Vector3.zero;
		if(nextPos.y + m_distance > m_wallUp.transform.position.y) {
			Vector3 destiny = nextPos;
			destiny.y += m_distance;
			walls= m_steering.flee(this.transform.position, destiny, m_velocity, maxVelocity);
//			walls = new Vector3(0,-1,0);
		}
		if(nextPos.y - m_distance < m_wallDown.transform.position.y) {
			Vector3 destiny = nextPos;
			destiny.y -= m_distance;
			walls= m_steering.flee(this.transform.position, destiny, m_velocity, maxVelocity);
//			walls = new Vector3(0,1,0);
		}
		if(nextPos.x + m_distance > m_wallRight.transform.position.x) {
			Vector3 destiny = nextPos;
			destiny.x += m_distance;
			walls= m_steering.flee(this.transform.position, destiny, m_velocity, maxVelocity);
//			walls = new Vector3(-1,0,0);
		}
		if(nextPos.x - m_distance < m_wallLeft.transform.position.x) {
			Vector3 destiny = nextPos;
			destiny.x -= m_distance;
			walls= m_steering.flee(this.transform.position, destiny, m_velocity, maxVelocity);
//			walls = new Vector3(1,0,0);
		}


		v1 += separation *  m_separation;
		v1 += cohesion * m_cohesion;
		v1 += orientation * m_cohesion;
		v1 += constant * m_constant;
		v1 += avoidMouse * m_avoid;
		v1 += walls * m_wallAvoid;


		return v1;
	}

	public Vector3 getVelocity() {
		return m_velocity;
	}

}
