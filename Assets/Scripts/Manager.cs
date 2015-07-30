using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour {

	public int totalBirds;

	public float[] limitX;
	public float[] limitY;

	List<Movement> m_listaGO;

	public GameObject prefab;

	// Use this for initialization
	void Start () {
		populate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void populate() {
		m_listaGO = new List<Movement>();
		for(int i = 0; i < totalBirds; ++i) {
			GameObject go = Instantiate(prefab);
			go.transform.position= new Vector3(Random.Range(limitX[0],limitX[1]),Random.Range(limitY[0],limitY[1]),0);
			go.transform.localRotation = Quaternion.Euler(new Vector3(0,0,Random.Range(0,360)));

			go.GetComponent<Movement>().limitX = limitX;
			go.GetComponent<Movement>().limitY = limitY;
			m_listaGO.Add(go.transform.GetComponent<Movement>());
		}

		for(int i = 0; i < m_listaGO.Count; i++) {
			m_listaGO[i].GetComponent<Movement>().m_listaBirds = m_listaGO;
		}
	}
}
