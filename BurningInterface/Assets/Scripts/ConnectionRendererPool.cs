
using UnityEngine;
using UnityEngine.Pool;

public class ConnectionRendererPool : MonoBehaviour
{
    public static ConnectionRendererPool instance;

    [SerializeField] private ConnectionRenderer m_ConnectionRenderer;
    private ObjectPool<ConnectionRenderer> m_Pool;

    private void Awake()
    {
        Configure();
    }

    private void Configure()
    {
        if (!instance)
        {
            instance = this;
            ConfigurePool();
        }
        else Destroy(gameObject);
    }
    
    public ConnectionRenderer Spawn(Connection _connection)
    {
        ConnectionRenderer _connectionRenderer =  m_Pool.Get();
        _connectionRenderer.connection = _connection;
        _connectionRenderer.RenderConnection();
        return _connectionRenderer;
    }

    public void Release(ConnectionRenderer _connectionRenderer)
    {
        m_Pool.Release(_connectionRenderer);
    }

    private void ConfigurePool()
    {
        m_Pool = new ObjectPool<ConnectionRenderer>(() => { return Instantiate(m_ConnectionRenderer); },
            _connectionRenderer => { _connectionRenderer.gameObject.SetActive(true); }, _connectionRenderer => { _connectionRenderer.gameObject.SetActive(false); },
            _connectionRenderer => { Destroy(_connectionRenderer.gameObject); }, false, 10, 20);
    }
}
