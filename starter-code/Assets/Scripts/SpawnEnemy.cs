using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave
{
	public List<GameObject> enemyPrefabs;
	public float spawnInterval = 2;
	public int maxEnemies = 20;
}

public class SpawnEnemy : MonoBehaviour {

	public GameObject[] waypoints;
	public Wave[] waves;
	public int timeBetweenWaves = 5;

	private GameManagerBehaviour gameManager;

	private float lastSpawnTime;
	private int enemiesSpawned = 0;

	// Use this for initialization
	void Start () {
		lastSpawnTime = Time.time;
		gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
	}

	// Update is called once per frame
	void Update () {
		int currentWave = gameManager.Wave;
		if (currentWave < waves.Length)
		{
			Wave wave = waves[currentWave];
			float timeInterval = Time.time - lastSpawnTime;
			float spawnInterval = wave.spawnInterval;
			if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || (enemiesSpawned != 0 && timeInterval > spawnInterval)) && 
				(enemiesSpawned < wave.maxEnemies))
			{
				lastSpawnTime = Time.time;
				int randomIndex = Random.Range(0, wave.enemyPrefabs.Count);
				GameObject newEnemy = Instantiate(wave.enemyPrefabs[randomIndex]);
				newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
				enemiesSpawned++;
			}
			if (enemiesSpawned == wave.maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				gameManager.Wave++;
				gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
				enemiesSpawned = 0;
				lastSpawnTime = Time.time;
			}
		}
		else
		{
			gameManager.gameOver = true;
			GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
			gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
		}
	}
}
