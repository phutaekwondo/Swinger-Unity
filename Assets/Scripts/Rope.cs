using UnityEngine;
using System.Collections.Generic;

public class Rope : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private float m_ropeSize = 0.2f;
    private List<GameObject> m_ropeSegments = new List<GameObject>();

    //REFERENCES
    [SerializeField] private GameObject m_ropeSegmentPrefab;
    private GameObject m_player;
    private Vector2 m_stickPoint;
    private LineRenderer m_lineRenderer;

    //PUBLIC METHODS
    public void GenerateRopeWithDirection(Vector2 direction)
    {
        //find the stick point
        RaycastHit2D hit = Physics2D.Raycast(m_player.transform.position, direction, 100f, LayerMask.GetMask("Obstacle"));
        if (hit.collider != null)
        {
            m_stickPoint = hit.point;
        }
        else
        {
            return;
        }

        if (m_stickPoint == (Vector2)m_player.transform.position) { return; }
        GenerateRopeWithStickPoint(m_stickPoint);
    }

    //PRIVATE METHODS
    private void Start()
    {
        m_player = transform.parent.gameObject;
        m_lineRenderer = GetComponent<LineRenderer>();

        //testing
        GenerateRopeWithDirection(new Vector2(0.8f,1) ) ;
    }

    private void Update()
    {
        // DrawRope();
    }

    private void DrawRope()
    {
        //set the width of the rope
        m_lineRenderer.startWidth = m_ropeSize;
        m_lineRenderer.endWidth = m_ropeSize;

        m_lineRenderer.positionCount = m_ropeSegments.Count + 1;

        m_lineRenderer.SetPosition(m_ropeSegments.Count, m_player.transform.position);
        for (int i = 0; i < m_ropeSegments.Count; i++)
        {
            m_lineRenderer.SetPosition(i, m_ropeSegments[i].transform.position);
        }
    }

    private void ClearCurrentRope()
    {
        //destroy all the rope segments
        for (int i = 0; i < m_ropeSegments.Count; i++)
        {
            Destroy(m_ropeSegments[i]);
        }

        m_ropeSegments.Clear();
    }

    private void GenerateRopeWithStickPoint(Vector2 stickPoint)
    {
        ClearCurrentRope();

        m_stickPoint = stickPoint;

        //calculate the distance between the player and the stick point
        float distance = Vector2.Distance(m_player.transform.position, m_stickPoint);

        //calculate the number of segments needed to draw the rope
        int segmentsCount = (int)(distance / m_ropeSize);

        //line direction
        Vector2 direction = ((Vector2)m_player.transform.position - m_stickPoint).normalized;

        //first Segment of the rope
        ////create a new rope Segment
        GameObject firstRopeSegment = Instantiate(
            m_ropeSegmentPrefab, 
            m_stickPoint, 
            Quaternion.identity);
        firstRopeSegment.transform.localScale = new Vector2(m_ropeSize, m_ropeSize);
        firstRopeSegment.transform.SetParent(this.transform);
        firstRopeSegment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        m_ropeSegments.Add(firstRopeSegment);

        //gererate rope Segments from the player to the stick point
        for (int i = 1; i < segmentsCount; i++)
        {
            GameObject ropeSegment = Instantiate(
                m_ropeSegmentPrefab, 
                m_stickPoint + (direction * m_ropeSize * i), 
                Quaternion.identity
                );
            ropeSegment.transform.localScale = new Vector2(m_ropeSize, m_ropeSize);
            ropeSegment.transform.SetParent(this.transform);
            ropeSegment.GetComponent<HardJoint2D>().SetConnectedGameObject(m_ropeSegments[i-1]);
            m_ropeSegments.Add(ropeSegment);
        }

        m_player.GetComponent<HardJoint2D>().SetConnectedGameObject(m_ropeSegments[m_ropeSegments.Count-1]);
    }
}
