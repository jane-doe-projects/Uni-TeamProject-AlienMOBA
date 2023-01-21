using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardResources : MonoBehaviour
{
    /* Written by Daniela on 17/07/2021
     * 
     */
    private void LateUpdate()
    {
        // turn game object towards the main camera / screen
        Quaternion lookRotation = Camera.main.transform.rotation;
        transform.rotation = lookRotation;
    }
}
