using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    public float speed = 1;
    public float range = 0.3f;

    private float t = 0;
    private Vector3 startPosition;
    public float randomizer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        t += randomizer;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        transform.position = new Vector3(
           startPosition.x + (Mathf.Sin(t * 0.5f * speed) * range),
           startPosition.y + (Mathf.Sin(t * 0.3f * speed) * range),
           startPosition.z + (Mathf.Sin(t * 0.7f * speed) * range)
        );
    }
}
