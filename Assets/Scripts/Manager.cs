using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour {

	public int totalBirds;

	public float[] limitX;
	public float[] limitY;

	List<GameObject> m_listaGO;

	public GameObject prefab;

	// Use this for initialization
	void Start () {
		populate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void populate() {
		m_listaGO = new List<GameObject>();
		for(int i = 0; i < totalBirds; ++i) {
			GameObject go = Instantiate(prefab);
			m_listaGO.Add(go);

			go.transform.position= new Vector3(Random.Range(limitX[0],limitX[1]),Random.Range(limitY[0],limitY[1]),0);
			go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0,360)));

			go.GetComponent<Movement>().limitX = limitX;
			go.GetComponent<Movement>().limitY = limitY;
			go.GetComponent<Movement>().initialVelocity = new Vector3(Random.Range(0,1),Random.Range(0,1), 0);
		}
	}
}
