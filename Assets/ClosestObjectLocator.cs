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
        float neardistance = NearestObjectDistance();
        distanceOutput.text = neardistance.ToString();
        if (neardistance < 10) {
            Vector3 s = heatBar.localScale;
            heatBar.transform.localScale = new Vector3((1 - (neardistance / 10f)) , s.y, s.z);
            Color c = heatBarColor.color;
            heatBarColor.color = new Color(Mathf.Round(1 - (neardistance /10f)) , (neardistance / 10f), ((neardistance / 10f)) );
        }
    }

    public float NearestObjectDistance()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Pickupable");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
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

