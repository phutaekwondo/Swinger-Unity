using UnityEngine;

public class HardJoint2D : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private float m_distance;
    private bool m_isEnable = false;
    //REFERENCES
    [SerializeField] GameObject m_connectedGameObject;
    Rigidbody2D m_rigidbody;

    //PUBLIC METHODS
    public void SetConnectedGameObject(GameObject connectingObject)
    {
        m_connectedGameObject = connectingObject;
        CalculateDistance();
        UpdateEnablement(); 
    }

    //PRIVATE METHODS
    private void Start() {
        if (!m_isEnable)
        {
            return;
        }
        CalculateDistance();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        if (!m_isEnable)
        {
            return;
        }

        UpdateEnablement();
        MaintainDistance();
        UpdateVeclocity();
    }

    private void UpdateEnablement()
    {
        if (!m_connectedGameObject || !m_rigidbody)
        {
            m_isEnable = false;
        }
        else
        {
            m_isEnable = true;
        }
    }

    private void CalculateDistance()
    {
        m_distance = Vector2.Distance(transform.position, m_connectedGameObject.transform.position);
    }

    private void UpdateVeclocity()
    {
        //rotate the vector
        Vector2 connectDirection = (Vector2)(m_connectedGameObject.transform.position - this.transform.position);
        connectDirection.Normalize();
        Vector2 velocity = m_rigidbody.velocity;
        float angle = Vector2.Angle(velocity, connectDirection);

        //cos(a-90) = Sin(a)
        float velocityWeight = velocity.magnitude;
        float newVelocityWeight = velocityWeight * Mathf.Sin((angle/180) * Mathf.PI);

        Vector2 newVelocityDirection_NegativeY = new Vector2( -connectDirection.y, connectDirection.x);
        Vector2 newVelocityDirection_NegativeX = new Vector2( connectDirection.y, -connectDirection.x);

        Vector2 newVelocityDirection;
        if (Vector2.Angle(newVelocityDirection_NegativeX, velocity) > Vector2.Angle(newVelocityDirection_NegativeY,velocity))
        {
            newVelocityDirection = newVelocityDirection_NegativeY;
        }
        else
        {
            newVelocityDirection = newVelocityDirection_NegativeX;
        }

        Vector2 newVelocity = newVelocityDirection * newVelocityWeight;

        //apply gravity
        //Unity physics already apply gravity, so take a good rest.

        m_rigidbody.velocity = newVelocity;
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
