using UnityEngine;
using System.Collections.Generic;

public class EnumToGameObject
{
    public static Dictionary<EnumTerrain, GameObject> terrainPrefabs = new(){
        {
            EnumTerrain.Block,
            Resources.Load<GameObject>("Prefabs/GamePrefab/Block")
        },
        {
            EnumTerrain.Spike,
            Resources.Load<GameObject>("Prefabs/GamePrefab/Spike")
        },
        {
            EnumTerrain.JumpOrb,
            Resources.Load<GameObject>("Prefabs/GamePrefab/JumpOrb")
        },
        {
            EnumTerrain.ShipPortal,
            Resources.Load<GameObject>("Prefabs/GamePrefab/ShipPortal")
        },
        {
            EnumTerrain.CubePortal,
            Resources.Load<GameObject>("Prefabs/GamePrefab/CubePortal")
        },
        {
            EnumTerrain.BallPortal,
            Resources.Load<GameObject>("Prefabs/GamePrefab/BallPortal")
        }
    };
}