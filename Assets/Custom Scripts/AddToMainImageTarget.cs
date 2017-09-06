using UnityEngine;
using System.Collections;

public class AddToMainImageTarget : MonoBehaviour {

    void Start () {
        GameObject stadium = GameObject.FindGameObjectWithTag ("ImageTarget");
        transform.SetParent (stadium.transform, false);
    }
}