
using System.Collections.Generic;
using UnityEngine;

public class ConnectionRenderer : MonoBehaviour
{
    public Connection connection;

    [SerializeField] private LineRenderer m_LineRenderer;
    
    public void RenderConnection()
    {
        m_LineRenderer.positionCount = 2;
        List<Vector3> pos = new List<Vector3>();
        pos.Add(connection.top.transform.position);
        pos.Add(connection.bottom.transform.position);
        m_LineRenderer.startWidth = 0.1f;
        m_LineRenderer.endWidth = 0.1f;
        m_LineRenderer.SetPositions(pos.ToArray());
        m_LineRenderer.useWorldSpace = true;
    }

    private void OnDisable()
    {
        m_LineRenderer.positionCount = 0;
    }
}
