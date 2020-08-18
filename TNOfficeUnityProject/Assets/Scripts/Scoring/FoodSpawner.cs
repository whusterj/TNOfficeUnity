using UnityEngine;
using Photon.Pun;

public class FoodSpawner : MonoBehaviourPun
{
    [SerializeField] private GameObject foodPrefab = null;
    [SerializeField] private float spawnHeight = -0.5f;
    [SerializeField] private float foodSpawnChance = 0.01f;

    // Photon can be disabled to test this component locally
    [SerializeField] private bool usePhoton = true;

    private GameObject[] spawnProbes = null;

    // Start is called before the first frame update
    void Start()
    {
        spawnProbes = GameObject.FindGameObjectsWithTag("FoodSpawnProbes");
        Debug.Log("Found spawnProbes: " + spawnProbes.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (usePhoton)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (GameObject probe in spawnProbes)
                {
                    if (ShouldSpawnFood()) SpawnFoodPun(probe);
                }
            }
        }
        else
        {
            foreach (GameObject probe in spawnProbes)
            {
                if (ShouldSpawnFood()) SpawnFood(probe);
            }
        }
    }

    private bool ShouldSpawnFood() => Random.Range(0f, 1f) <= foodSpawnChance;

    private void SpawnFood(GameObject probe)
    {
        Vector3 position = probe.transform.position;
        Instantiate(
            foodPrefab,
            new Vector3(
                position.x + Random.Range(-3f, 3f),
                spawnHeight,
                position.z + Random.Range(-3f, 3f)
            ),
            Quaternion.identity
        );
    }

    private void SpawnFoodPun(GameObject probe)
    {
        Vector3 position = probe.transform.position;
        Debug.Log("Spawn Probe Position: " + position.ToString());
        PhotonNetwork.Instantiate(
            foodPrefab.name,
            new Vector3(
                position.x + Random.Range(-3f, 3f),
                spawnHeight,
                position.z + Random.Range(-3f, 3f)
            ),
            Quaternion.identity
        );

    }
}
