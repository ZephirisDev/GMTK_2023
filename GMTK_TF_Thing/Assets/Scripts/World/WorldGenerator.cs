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
    private List<Direction> directions = new List<Direction>();
    private List<GameObject> loadedSections = new List<GameObject>();
    private List<Vector2Int> poses = new List<Vector2Int>();
    private bool GoForward = true;

    private Section GetRandomSection() {
        var validSections = GetValidSections();
        var section = validSections[Random.Range(0, validSections.Count)];
        //lastThree.Enqueue(section);
        //lastThree.Dequeue();
        return section;
    }

    public void Switch()
    {
        GoForward = false;
        poses.Remove(poses.Last());
        poses.Remove(poses.Last());
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
        if (GoForward)
        {
            if (Physics.OverlapSphere(new Vector3(poses.Last().x * sizePerSection, 0, poses.Last().y * sizePerSection), sizePerSection, LayerLibrary.Player).Length > 0)
                SpawnNext();
        }
        else
        {
            if (loadedSections.Count <= 4) return;

            if (Physics.OverlapSphere(new Vector3(poses.Last().x * sizePerSection, 0, poses.Last().y * sizePerSection), sizePerSection, LayerLibrary.Player).Length > 0)
            {
                poses.Remove(poses.Last());
                loadedSections[loadedSections.Count - 4].SetActive(true);
                Destroy(loadedSections.Last());
                loadedSections.Remove(loadedSections.Last());
            }
        }
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
        loadedSections.Add(c);
        if (loadedSections.Count > 5)
            loadedSections[loadedSections.Count - 5].SetActive(false);
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

        poses.Add(curNextSpawnPos);
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
