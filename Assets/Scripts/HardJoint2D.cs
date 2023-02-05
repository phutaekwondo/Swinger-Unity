using System;
using UnityEngine;

public class HardJoint2D : MonoBehaviour
{
    //VARIABLES
    private float m_distance;
    //REFERENCES
    [SerializeField] GameObject m_connectedGameObject;

    private void Update() 
    {
        MaintainDistance();
        UpdateVeclocity();

    }

    private void UpdateVeclocity()
    {
        //apply gravity
        //rotate the vector
    }

    private void MaintainDistance()
    {
    }
}
