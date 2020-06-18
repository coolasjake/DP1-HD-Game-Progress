using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SoundObjectsManager : MonoBehaviour {
    
    public List<GameObject> cubes = new List<GameObject>();
    public bool load = false;
    public static string cubeSaveName = "/cubePos.dat";

    private List<Vector3> defaultPositions = new List<Vector3>();

    private float minRangeLimit = 0;
    private float minAudioDistance = 0;
    private float maxAudioDistance = 100;
    private float maxRangeLimit = 100;

    // Use this for initialization
    void Awake() {
        if (!File.Exists(Application.persistentDataPath + cubeSaveName))
            return;

        if (cubes.Count == 0)
        {
            foreach (MusicPulse c in GetComponentsInChildren<MusicPulse>())
                cubes.Add(c.gameObject);
        }

        List<Vector3> positions = LoadCubeLocations();

        for (int i = 0; i < cubes.Count && i < positions.Count; ++i)
        {
            defaultPositions.Add(cubes[i].transform.position);
            cubes[i].transform.position = positions[i];
        }
    }

    public void ResetCubes()
    {
        for (int i = 0; i < cubes.Count && i < defaultPositions.Count; ++i)
            cubes[i].transform.position = defaultPositions[i];
    }

    public void SetMinDist(float newMinDist)
    {
        minAudioDistance = newMinDist; //Mathf.Clamp(newMinDist, minRangeLimit, maxAudioDistance);
        foreach (GameObject cube in cubes)
        {
            cube.GetComponent<AudioSource>().minDistance = minAudioDistance;
        }
    }

    public void SetMaxDist(float newMaxDist)
    {
        maxAudioDistance = newMaxDist; //Mathf.Clamp(newMinDist, minRangeLimit, maxAudioDistance);
        foreach (GameObject cube in cubes)
        {
            cube.GetComponent<AudioSource>().maxDistance = maxAudioDistance;
        }
    }

    public static void SaveCubeLocations(List<GameObject> CubeList)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (GameObject Cube in CubeList)
            positions.Add(Cube.transform.position);
        SaveCubeLocations(positions);
    }

	public static void SaveCubeLocations (List<Vector3> CubeLocations) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + cubeSaveName);

        List<SaveVect3> positions = new List<SaveVect3>();

        foreach (Vector3 pos in CubeLocations)
                positions.Add(new SaveVect3(pos));

        SaveData data = new SaveData
        {   //Add variables here.
            cubeLocations = positions
        };

        bf.Serialize (file, data);

		file.Close ();
	}

    public static List<Vector3> LoadCubeLocations()
    {
        if (File.Exists(Application.persistentDataPath + cubeSaveName))
        {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + cubeSaveName, FileMode.Open);
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();

            //Add variables here.
            List<Vector3> positions = new List<Vector3>();

            foreach (SaveVect3 SV in data.cubeLocations)
                positions.Add(SV.ToVect3);

                return positions;
        }
        else
            return new List<Vector3>();
	}

    void OnApplicationQuit()
    {
        SaveCubeLocations(cubes);
    }
}

[Serializable]
class SaveData {
	public List<SaveVect3> cubeLocations;
}

[Serializable]
class SaveVect3
{
    public float x;
    public float y;
    public float z;

    public SaveVect3(float X, float Y, float Z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public SaveVect3(float X, float Y)
    {
        x = X;
        y = Y;
        z = 0;
    }

    public SaveVect3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 ToVect3
    {
        get { return new Vector3(x, y, z); }
    }
}
