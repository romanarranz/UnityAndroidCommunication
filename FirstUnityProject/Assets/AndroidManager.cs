using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AndroidManager : MonoBehaviour {
	private AndroidJavaObject curActivity, curActivity2;
	public string strLog = "No Java Log";
	static AndroidManager _instance;

	public static AndroidManager GetInstance() {

		if( _instance == null ) {
			_instance = new GameObject("AndroidManager").AddComponent<AndroidManager>();   
		}

		return _instance;
	}

	void Awake() {

		if (Application.platform == RuntimePlatform.Android) { 
			//AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaClass jc = new AndroidJavaClass ("com.github.romanarranz.blockgame.UnityPlayerActivity");
			AndroidJavaClass jc2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

			curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			curActivity2 = jc2.GetStatic<AndroidJavaObject>("currentActivity");

			Screen.fullScreen = false;
		}
	}

	void Start () {}

	void FixedUpdate() {
		// En Unity el boton de Atras "BackPressed" de Android no se soporta por defecto
		// por lo que añadimos lo siguiente
		if (Application.platform == RuntimePlatform.Android) {
			detectPressedKeyOrButton ();
			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit();
			}

			if (Input.GetKey(KeyCode.Home)) {
				return;
			}

			if (Input.GetKey (KeyCode.Menu)) {
				Application.Quit();
			}
		}
	}

	public void detectPressedKeyOrButton() {
		foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
			if (Input.GetKeyDown(kcode))
				Debug.Log("KeyCode down: " + kcode);
		}
	}

	public void CallJavaFunc( string strFuncName, string strTemp ) {

		if( curActivity == null && curActivity2 == null) {
			strLog = curActivity + " is null";
			return;
		}

		strLog = "Before call " + strFuncName;
		//curActivity.Call(strFuncName, new object[] { strTemp } );

		curActivity.Call( strFuncName, strTemp );
		curActivity2.Call (strFuncName, strTemp );
		strLog = strFuncName + " is Called with param " + strTemp;
	}

	void SetJavaLog(string strJavaLog) {
		strLog = strJavaLog;
	}
}