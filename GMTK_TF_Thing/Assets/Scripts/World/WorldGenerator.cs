using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] GameObject tracker;
    [SerializeField] int spawnOnDelta;
    [SerializeField] int sizePerSection;
    [SerializeField] List<GameObject> sections;
    private int curSection;
    private Stack<GameObject> lastThree;

    private GameObject GetRandomSection() { return sections[Random.Range(0, sections.Count)]; }

    private void Awake()
    {
        Instantiate(GetRandomSection());
        curSection++;
    }

    private void FixedUpdate()
    {
        var nextSpawn = curSection * sizePerSection;
        if (tracker.transform.position.z > nextSpawn - spawnOnDelta)
        {
            var c = Instantiate(GetRandomSection());
            c.transform.position = new Vector3(0, 0, nextSpawn);
            curSection++;
        }
    }

}
