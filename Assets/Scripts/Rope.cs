using UnityEngine;
using System.Collections.Generic;

public class Rope : MonoBehaviour
{
    //VARIABLES
    private float m_ropeSize = 0.2f;
    private List<GameObject> m_ropeSegments = new List<GameObject>();

    //REFERENCES
    [SerializeField] private GameObject m_player;
    [SerializeField] private Vector2 m_stickPoint;
    [SerializeField] private GameObject m_ropeSegmentPrefab;
    private LineRenderer m_lineRenderer;

    private void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        GenerateRope();
    }

    private void GenerateRope()
    {
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
                Quaternion.identity);
            ropeSegment.transform.localScale = new Vector2(m_ropeSize, m_ropeSize);
            ropeSegment.transform.SetParent(this.transform);
            Debug.Log(m_ropeSegments[i - 1].GetComponent<Rigidbody2D>());
            ropeSegment.GetComponent<DistanceJoint2D>().connectedBody = m_ropeSegments[i - 1].GetComponent<Rigidbody2D>();
            m_ropeSegments.Add(ropeSegment);
        }

        m_player.GetComponent<DistanceJoint2D>().connectedBody = m_ropeSegments[m_ropeSegments.Count- 1].GetComponent<Rigidbody2D>();
    }
}
