using System.Collections.Generic;

[System.Serializable]
public class TerrainTemplate
{
    public EnumTerrain TerrainType;
    public int GameObjectId;
    public double X;
    public double Y;
    public double Z;
    public double Rotation;
    public double Scale;

    public TerrainTemplate(EnumTerrain terrainType, int gameObjectId, double x, double y, double z, double rotation, double scale)
    {
        TerrainType = terrainType;
        GameObjectId = gameObjectId;
        X = x;
        Y = y;
        Z = z;
        Rotation = rotation;
        Scale = scale;
    }

    public TerrainTemplate(object[] data)
    {
        TerrainType = (EnumTerrain)(long)data[0];
        GameObjectId = (int)(long)data[1];
        X = (double)data[2];
        Y = (double)data[3];
        Z = (double)data[4];
        Rotation = (double)data[5];
        Scale = (double)data[6];
    }
}

// Structure for player start position
[System.Serializable]
public class PlayerStartPosition
{
    public float X;
    public float Y;
    public float Z;
}

// Structure for the whole level
[System.Serializable]
public class LevelData
{
    public string LevelName;
    public PlayerStartPosition PlayerStartPosition;
    public List<TerrainTemplate> TerrainObjects;
}

[System.Serializable]
public class PlayerSettings
{
    public int Gamemode;
    public int Gravity;
}

[System.Serializable]
public class LevelDataCompressed
{
    public string LevelName;
    public List<double> PlayerStart;
    public string SongName;
    public PlayerSettings PlayerSettings;
    public List<object[]> Terrains;
}