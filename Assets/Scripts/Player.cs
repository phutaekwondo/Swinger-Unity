using UnityEngine;

public class Player : MonoBehaviour
{
    //VARIABLES
    //REFERENCES
    private Rope m_rope;

    private void Start()
    {
        m_rope = GetComponentInChildren<Rope>();
    }
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            //generate rope
            if (m_rope)
            {
                m_rope.GenerateRopeWithDirection(direction);
            }
        }
    }
}
