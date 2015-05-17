using UnityEngine;
using System.Collections;

public class BlopScript : MonoBehaviour {

	public float addToOutputRate;
	public float addToGrowthRate;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		//should be set by the gro producing it
		addToGrowthRate = 1;
		addToOutputRate = 1;
	}
	
	// Update is called once per frame
	void Update () {
		//should be doing path finding and moving on the path here
	}

	void FixedUpdate(){
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		if (x > 50 || x < -50 || z > 50 || z < -50) {
			Destroy(gameObject);
		}

		offset = new Vector3 ((x + 0.01f) * Time.deltaTime, y, (z + 0.01f) * Time.deltaTime);
		transform.position += offset;
	}

	void SetEnergy(float outAdd, float growAdd){
		addToGrowthRate = growAdd;
		addToOutputRate = outAdd;
	}
	
}
