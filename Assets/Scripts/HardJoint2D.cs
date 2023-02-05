using UnityEngine;

public class HardJoint2D : MonoBehaviour
{
    //VARIABLES
    private float m_distance;
    //REFERENCES
    [SerializeField] GameObject m_connectedGameObject;

    //PUBLIC METHODS

    //PRIVATE METHODS
    private void Start() {
        m_distance = Vector2.Distance(transform.position, m_connectedGameObject.transform.position);
    }

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
        Vector2 direction = m_connectedGameObject.transform.position - transform.position;
        float currentDistance = direction.magnitude;
        direction.Normalize();

        Vector2 newPostion = (Vector2)transform.position + direction * (currentDistance - m_distance);
        transform.position = newPostion;
    }
}
