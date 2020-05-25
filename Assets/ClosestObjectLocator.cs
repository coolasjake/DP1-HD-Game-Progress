using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosestObjectLocator : MonoBehaviour
{
    public Text distanceOutput;
    public Transform heatBar;
    public Image heatBarColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        float neardistance = NearestObjectDistance(transform.position + Camera.main.transform.forward * 3f);
        distanceOutput.text = neardistance.ToString();
        if (neardistance < 10) {
            Vector3 s = heatBar.localScale;
            heatBar.transform.localScale = new Vector3((1 - (neardistance / 10f)), (1 - (neardistance / 10f)), s.z);
            Color c = heatBarColor.color;
            heatBarColor.color = new Color(1 - (neardistance /10f), 0, (neardistance / 10f) + 0.3f);
        }
    }

    public float NearestObjectDistance(Vector3 position)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Pickupable");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return (distance / 3 / 10);
    }
}

