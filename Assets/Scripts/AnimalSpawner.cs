using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject animalPrefab;
    private Transform[] burrows;
    public float spawnRate = 2.5f;

    void Start() => InvokeRepeating("SpawnRequest", 2f, spawnRate);

    public void SetBurrows(Transform[] newBurrows) => burrows = newBurrows;

    void SpawnRequest()
    {
        if (burrows == null || burrows.Length < 2) return;

        int start = Random.Range(0, burrows.Length);
        int end = Random.Range(0, burrows.Length);
        while (start == end) end = Random.Range(0, burrows.Length);

        GameObject animal = Instantiate(animalPrefab, burrows[start].position, Quaternion.identity);
        Animal script = animal.GetComponent<Animal>();
        script.targetBurrow = burrows[end];
        script.speed += (GameManager.Instance.currentLevel * 0.5f);
    }
}