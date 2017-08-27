using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

	// vamos a crear 5 puntos de spawneo y uno de ellos estará vacio
	public Transform[] spawnPoints;

	public GameObject blockPrefab;

	public float timeBetweenWaves = 2f;

	private int totalWaves = 0;
	private float timeToSpawn = 2f;

	// Use this for initialization
	void Update () {

		// cuando pasen 2s spawn de bloques
		if (Time.time >= timeToSpawn) {
			if (totalWaves > 40) { // tras 40 olas quitamos 1 bloque
				SpawnBlocks (1);
			} else if (totalWaves > 15) { // tras 15 olas quitamos 2 bloques solo
				SpawnBlocks (2);
			} else {
				SpawnBlocks (3);
			}

			timeToSpawn = Time.time + timeBetweenWaves;

			totalWaves = totalWaves == 120 ? 0 : totalWaves + 1;
		}
	}

	void SpawnBlocks(int removeNBlocks) {
		List<int> randIndexes = new List<int>();

		while (randIndexes.Count < removeNBlocks) {
			int random = Random.Range (0, spawnPoints.Length);
			if (!randIndexes.Contains (random)) {
				randIndexes.Add (random);
			}
		}

		for (int i = 0; i < spawnPoints.Length; i++) {
			if (!randIndexes.Contains(i)) {
				Instantiate (blockPrefab, spawnPoints [i].position, Quaternion.identity);
			}
		}
	}
}