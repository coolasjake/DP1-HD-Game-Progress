﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save : MonoBehaviour {
    
    public List<GameObject> Cubes = new List<GameObject>();
    public bool load = false;
    public static string cubeSaveName = "/cubePos.dat";

    // Use this for initialization
    void Awake() {
        if (!File.Exists(Application.persistentDataPath + cubeSaveName))
            return;
        List<Vector3> positions = LoadCubeLocations();

        for (int i = 0; i < Cubes.Count && i < positions.Count; ++i)
            Cubes[i].transform.position = positions[i];
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
        SaveCubeLocations(Cubes);
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
