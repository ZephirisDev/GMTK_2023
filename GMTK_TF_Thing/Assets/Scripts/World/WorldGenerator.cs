using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] GameObject tracker;
    [SerializeField] int spawnOnDelta;
    [SerializeField] int sizePerSection;
    [SerializeField] List<Section> sections;
    [SerializeField] Section startSection;
    private Queue<Section> lastThree = new Queue<Section>();
    private Direction curLayoutDirection;
    private Direction nextMovementChange;
    private Vector2Int curNextSpawnPos = new Vector2Int(0, 0);
    private Vector2Int curPlayerPos = new Vector2Int(0, -1);
    private List<Direction> directions = new List<Direction>();

    private Section GetRandomSection() {
        var validSections = GetValidSections();
        var section = validSections[Random.Range(0, validSections.Count)];
        //lastThree.Enqueue(section);
        //lastThree.Dequeue();
        return section;
    }

    private List<Section> GetValidSections()
    {
        var s = sections;
        if (curLayoutDirection != Direction.Forward)
            s = s.Where(x => x.direction != curLayoutDirection).ToList();
        return s;
    }

    private void Awake()
    {
        PlaceSection(startSection);
    }

    private void FixedUpdate()
    {
        Debug.Log(new Vector3(curNextSpawnPos.x * sizePerSection, 0, curNextSpawnPos.y * sizePerSection));
        if (Physics.OverlapSphere(new Vector3(curNextSpawnPos.x * sizePerSection, 0, curNextSpawnPos.y * sizePerSection), sizePerSection * 1.5f, LayerLibrary.Player).Length > 0)
            SpawnNext();
        /*
        if (curLayoutDirection == Direction.Forward)
        {
            var nextY = (curPlayerPos.y - 2) * sizePerSection - spawnOnDelta;
            if(tracker.transform.position.z > nextY)
            {
                SpawnNext();
            }
        }
        else
        {
            var nextX = (curPlayerPos.x - 2) * sizePerSection - spawnOnDelta;
            if (tracker.transform.position.x > nextX)
            {
                SpawnNext();
            }
        }*/
    }

    private void SpawnNext()
    {
        PlaceSection(GetRandomSection());
    }

    private void PlaceSection(Section section)
    {
        var c = Instantiate(section.gameObject);
        c.transform.position = new Vector3((curNextSpawnPos.x) * sizePerSection, 0, (curNextSpawnPos.y) * sizePerSection);
        directions.Add(section.direction);
        switch (curLayoutDirection)
        {
            case Direction.Left:
                c.transform.Rotate(new Vector3(0, -90));
                break;
            case Direction.Right:
                c.transform.Rotate(new Vector3(0, 90));
                break;
            default:
                break;
        }

        if (section.direction != Direction.Forward)
        {
            curLayoutDirection = curLayoutDirection == Direction.Forward ? section.direction : Direction.Forward;
        }

        nextMovementChange = section.direction;
        switch (curLayoutDirection)
        {
            case Direction.Left:
                curNextSpawnPos += new Vector2Int(-1, 0);
                break;
            case Direction.Right:
                curNextSpawnPos += new Vector2Int(1, 0);
                break;
            default:
                curNextSpawnPos += new Vector2Int(0, 1);
                break;
        }

    }

}
