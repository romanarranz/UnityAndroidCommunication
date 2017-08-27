using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 2f;
	public float mapWidth = 5f;
	private bool colission = false;
	private float hInput = 0;

	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();

		// en cuanto se instancie el Player le comunicamos a Android Framework el maximo rango de movimiento
		// que tiene el player
		AndroidManager.GetInstance().CallJavaFunc("playerWidth", mapWidth.ToString());
	}

	void Update(){
		// si no ha chocado en cada frame comprobamos si tenemos que actualizar la puntuacion
		if (!colission) {
			isDodgingBlocks();
		}
	}

	// registrar movimiento
	void FixedUpdate() {

		if (Application.platform != RuntimePlatform.Android) {
			float x = Input.GetAxis ("Horizontal");
			hInput = x;
		}

		Move(hInput);
	}

	void OnCollisionEnter2D() {
		colission = true;
		FindObjectOfType<GameManager>().EndGame();
		FindObjectOfType<GameManager> ().UpdateScore(0);
		colission = false;
	}

	public void Move(float horizontalInput) {
		float x = horizontalInput * Time.fixedDeltaTime * speed;

		Vector2 newPosition = rb.position + Vector2.right * x;

		// limitar la posicion de movimiento a -mapWidth y mapWidth
		newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);

		// mover el objeto rigido
		rb.MovePosition(newPosition);
	}

	public void StartMoving(float horizontalInput) {
		hInput = horizontalInput;
	}

	// cuando recibamos un mensaje desde el Android Framework lo parseamos y se lo mandamos
	// nuevamente al a funcion StartMoving para mover el Player, tiene que ser public para que se pueda
	// llamar desde Android Framework
	// https://github.com/inbgche/Unity-Android-Communication
	public void ReceiveMsg(string horizontalInput) {
		StartMoving(float.Parse(horizontalInput));
	}

	private void isDodgingBlocks() {
		Block[] fallingBlocks = FindObjectsOfType<Block>();

		for (int i = 0; i<fallingBlocks.Length; i++){

			// si la posicion y del objeto actual es mayor a la posicion y del bloque sumamos puntos
			if (transform.position.y > fallingBlocks[i].getPositionY()) {
				FindObjectOfType<GameManager> ().UpdateScore(1);
			}
		}
	}
}
