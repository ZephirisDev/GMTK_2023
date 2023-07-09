using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerDB", menuName = "Layer Library")]
public class LayerLibrary : Initializable
{
    [SerializeField] LayerMask obstacles;
    [SerializeField] LayerMask bounds;
    [SerializeField] LayerMask rotator;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask finale;

    public static LayerMask Obstacles => Instance.obstacles;
    public static LayerMask Bounds => Instance.bounds;
    public static LayerMask Rotators => Instance.rotator;
    public static LayerMask Player => Instance.player;
    public static LayerMask Finale => Instance.finale;


    public static LayerLibrary Instance;
    public override void Init() { Instance = this; }
}
