// Serializable struct to represent a wave of enemies.
[System.Serializable]
public class Wave
{
	// List of potential enemy prefab GameObjects to spawn for this wave.
	public List<GameObject> enemyPrefabs;

	// Interval between enemy spawns within this wave.
	public float spawnInterval = 2;

	// Maximum number of enemies to spawn for this wave.
	public int maxEnemies = 20;
}

public class SpawnEnemy : MonoBehaviour {

	// Array of waypoint GameObjects, presumably for the enemies to move between.
	public GameObject[] waypoints;

	// Array of waves to be spawned in the game.
	public Wave[] waves;

	// Time between each wave.
	public int timeBetweenWaves = 5;

	// Reference to the GameManagerBehaviour component for accessing game state.
	private GameManagerBehaviour gameManager;

	// Tracks the last time an enemy was spawned.
	private float lastSpawnTime;

	// Number of enemies spawned in the current wave.
	private int enemiesSpawned = 0;

	// Initialization method called when the script instance is being loaded.
	void Start () {
		// Store the current time.
		lastSpawnTime = Time.time;

		// Find the GameManager object in the scene and get its GameManagerBehaviour component.
		gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
	}

	// Called every frame.
	void Update () {
		// Get the current wave number.
		int currentWave = gameManager.Wave;

		// If we haven't gone through all the waves...
		if (currentWave < waves.Length)
		{
			// Get the current wave.
			Wave wave = waves[currentWave];

			// Calculate time since the last spawn.
			float timeInterval = Time.time - lastSpawnTime;
			float spawnInterval = wave.spawnInterval;

			// If it's time to spawn a new enemy...
			if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || 
				(enemiesSpawned != 0 && timeInterval > spawnInterval)) && 
				(enemiesSpawned < wave.maxEnemies))
			{
				// Reset the last spawn time.
				lastSpawnTime = Time.time;

				// Choose a random enemy prefab.
				int randomIndex = Random.Range(0, wave.enemyPrefabs.Count);
				GameObject newEnemy = Instantiate(wave.enemyPrefabs[randomIndex]);

				// Set the new enemy's waypoints.
				newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;

				// Increase the number of spawned enemies.
				enemiesSpawned++;
			}

			// If all enemies for this wave have been spawned and there are no enemies left in the scene...
			if (enemiesSpawned == wave.maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				// Move to the next wave.
				gameManager.Wave++;

				// Increase the player's gold by 10%.
				gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);

				// Reset the number of enemies spawned and the last spawn time.
				enemiesSpawned = 0;
				lastSpawnTime = Time.time;
			}
		}
		// If all waves are over...
		else
		{
			// Set the game as over.
			gameManager.gameOver = true;

			// Find the GameWon text object in the scene and play its "gameOver" animation.
			GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
			gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
		}
	}
}
