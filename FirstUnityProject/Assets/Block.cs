using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public float getPositionY() {
		return transform.position.y;
	}

	void Start() {
		// con el paso de tiempo incrementaremos la gravedad del objeto Block para que caiga mas rapido
		// GetComponent<Rigidbody2D>().gravityScale += Time.timeSinceLevelLoad / 10f;
	}

	// Update is called once per frame
	void Update () {

		// si el objeto se pasa de la coordenada y = -2 se elimina para liberar memoria
		if (transform.position.y < -2f) {
			Destroy(gameObject);
		}
	}
}
