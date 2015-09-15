using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour {

	public int totalBirds;

	public float[] limitX;
	public float[] limitY;

	List<Movement> m_listaGO;

	public GameObject prefab;

	public GameObject m_wallUp;
	public GameObject m_wallDown;
	public GameObject m_wallLeft;
	public GameObject m_wallRight;

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
			go.transform.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(0,360)));

			//initialization
			Movement mv = go.GetComponent<Movement>();
			mv.limitX = limitX;
			mv.limitY = limitY;
			mv.m_wallDown = m_wallDown;
			mv.m_wallUp = m_wallUp;
			mv.m_wallLeft = m_wallLeft;
			mv.m_wallRight = m_wallRight;


			m_listaGO.Add(go.transform.GetComponent<Movement>());

		}

		for(int i = 0; i < m_listaGO.Count; i++) {
			m_listaGO[i].GetComponent<Movement>().m_listaBirds = m_listaGO;
		}
	}
}
