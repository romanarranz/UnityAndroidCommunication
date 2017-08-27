using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float slowness = 10f;
	public Text score;
	public Text highScore;
	public Text timeElapsed;

	private float timer = 0f;
	private int secElapsed = 0, minutesElapsed = 0;
	private int scoreVal = 0, highScoreVal = 0;

	void Start() {
		
		// recuperar la maxima puntuacion de playerpref
		highScoreVal = PlayerPrefs.GetInt("HighScore", 0);

		score.text = scoreVal.ToString ();
		highScore.text = highScoreVal.ToString();
	}

	void Update() {

		if (minutesElapsed > 2) {
			ExitGame ();
		} else {
			timer += Time.deltaTime;

			int seconds = (int) timer % 60;
			if (seconds != secElapsed) {

				// cambio de minuto
				if (secElapsed == 59) {
					minutesElapsed++;
				}

				secElapsed = seconds;

				string secondsStr = seconds < 10 ? "0"+seconds.ToString() : seconds.ToString();
				timeElapsed.text = "0"+minutesElapsed.ToString() +":"+ secondsStr;
			}
		}
	}

	public void UpdateScore(int newScore) {
		// actualizar el score actual
		scoreVal += newScore;
		score.text = scoreVal.ToString();

		// si el score actual es mayor al maximo conseguido se actualiza
		if (scoreVal > highScoreVal) {
			highScoreVal = scoreVal;
			highScore.text = score.text;
		}
	}

	public void EndGame() {
		// guardar mejor puntuacion
		if (highScoreVal > PlayerPrefs.GetInt("HighScore", 0)) {
			PlayerPrefs.SetInt ("HighScore", highScoreVal);
		}

		// volver a cargar el juego de nuevo reseteando la scena por completo
		//StartCoroutine(RestartLevel());
	}

	public void ExitGame() {
		Debug.Log ("Quitting game...");
		Application.Quit();
	}

	IEnumerator RestartLevel () {
		// reducir el tiempo en un factor de slowness
		Time.timeScale = 1f / slowness;
		Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;

		// espera de 1s async escrita de forma secuencial, si no ponemos la division
		// esperariamos 1s multiplicado por el factor de slowness que serian = 10s :)
		yield return new WaitForSeconds (1f / slowness);

		// despues de 1s, recuperar el tiempo de juego normal y cargar de nuevo el juego
		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.fixedDeltaTime * slowness;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		// SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
