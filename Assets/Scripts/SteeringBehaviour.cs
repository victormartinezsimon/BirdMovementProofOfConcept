using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringBehaviour {

	public SteeringBehaviour(){}

	public Vector3 Seek(Transform origin, Transform destiny, Vector3 actualVelocity, float maxVelocity) {
		Vector3 desiredVelocity = (destiny.position - origin.position).normalized * maxVelocity;

		return desiredVelocity - actualVelocity;
	}
	private Vector3 Seek(Transform origin, Vector3 destiny, Vector3 actualVelocity, float maxVelocity) {
		Vector3 desiredVelocity = (destiny - origin.position).normalized * maxVelocity;
		
		return desiredVelocity - actualVelocity;
	}

	public Vector3 flee(Transform origin, Transform destiny, Vector3 actualVelocity, float maxVelocity) {
		Vector3 desiredVelocity = (origin.position - destiny.position).normalized * maxVelocity;
		
		return desiredVelocity - actualVelocity;
	}

	public Vector3 separation(Transform origin, List<Transform> others) {

		Vector3 force = Vector3.zero;
		for(int i = 0; i < others.Count; i++) {
			Vector3 v = origin.transform.position - others[i].transform.position;
			force += v.normalized/v.magnitude;
		}
		return force;
	}

	public float rotation(Transform origin, List<Transform> others) {

		float finalRotation = 0;

		for(int i = 0; i < others.Count; i++) {
			finalRotation += others[i].eulerAngles.z;
		}

		if(others.Count != 0) {
			finalRotation = finalRotation / others.Count;
		}

		return finalRotation - origin.eulerAngles.z;
	}
	public Vector3 cohesion (Transform origin, List<Transform> others, Vector3 actualVelocity, float maxVelocity) {

		Vector3 centerOfMass = Vector3.zero;
		Vector3 force = Vector3.zero;
		for(int i = 0; i < others.Count; i++) {
			centerOfMass += others[i].position;
		}
		if(others.Count != 0) {
			centerOfMass = centerOfMass / others.Count;
			force = Seek(origin,centerOfMass,actualVelocity,maxVelocity);
		}
		return force;
	}
}
