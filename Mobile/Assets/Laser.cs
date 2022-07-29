using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    EdgeCollider2D edgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }
    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);

        List<Vector2> edges = new List<Vector2>();
        Vector3 lineRendererPoint = transform.InverseTransformPoint(endPos);
        edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));

        Vector3 v2 = transform.InverseTransformPoint(startPos);
        Vector3 diff = lineRendererPoint - v2;
        Vector3.Normalize(diff);
        diff = diff / 50;
        v2 = diff + lineRendererPoint;
        edges.Add(v2);

        edgeCollider.SetPoints(edges);

    }
    void ShootLaser()
    {
        if (Physics2D.Raycast(laserFirePoint.position, laserFirePoint.up))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, laserFirePoint.up);
            Draw2DRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.up * defDistanceRay);
        }
    }
}
