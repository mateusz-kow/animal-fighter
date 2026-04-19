using UnityEngine;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject burrowPrefab;

    public List<Transform> GenerateLevel(int level)
    {
        string[] tags = { "Grass", "Burrow", "Animal" };
        foreach (string t in tags)
        {
            foreach (GameObject o in GameObject.FindGameObjectsWithTag(t)) Destroy(o);
        }

        int size = 10 + (level * 4); 
        int burrowCount = 3 + level;
        List<Transform> burrows = new List<Transform>();

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject g = Instantiate(grassPrefab, new Vector3(x, y, 0), Quaternion.identity);
                g.tag = "Grass";
            }
        }

        int attempts = 0;
        while (burrows.Count < burrowCount && attempts < 100)
        {
            attempts++;
            Vector3 randomPos = new Vector3(Random.Range(0, size), Random.Range(0, size), 0);
            
            bool tooClose = false;
            foreach (Transform b in burrows)
            {
                if (Vector3.Distance(randomPos, b.position) < 3.5f)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                GameObject burrowObj = Instantiate(burrowPrefab, randomPos, Quaternion.identity);
                burrowObj.tag = "Burrow";
                burrows.Add(burrowObj.transform);
            }
        }
        return burrows;
    }
}