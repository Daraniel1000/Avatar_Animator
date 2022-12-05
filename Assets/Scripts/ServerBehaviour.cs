using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;
using System.Collections.Generic;
using Assets.Scripts;

public class ServerBehaviour : MonoBehaviour
{

    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    [SerializeField]
    public GameObject m_vertexObject;

    private GameObject m_faceRoot;

    public static int[] vertexNumbers = new int[] { 76, 73, 11, 303, 306, 404, 16, 180, 74, 184, 304, 408, 90, 77, 320, 307, 175, 169, 394, 215, 435, 147, 214, 205, 376, 434, 425 };
    //private List<GameObject> vertexObjects = new List<GameObject>();
    private FaceHelper faceHelper;
    private bool isClientReady = false;

    // Start is called before the first frame update
    void Start()
    {
        m_faceRoot = GameObject.Find("Root");
        faceHelper = new FaceHelper(m_faceRoot);

        m_Driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = 9000;
        if (m_Driver.Bind(endpoint) != 0)
            Debug.Log("Failed to bind to port 9000");
        else
        {
            m_Driver.Listen();
            Debug.Log("Listening on port 9000");
        }

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        //foreach (var vertex in vertexNumbers)
        //{
        //    vertexObjects.Add(Instantiate(m_vertexObject));
        //}
    }

    private void SendVertexNumbers(NetworkConnection c)
    {
        m_Driver.BeginSend(c, out var writer);
        writer.WriteInt(vertexNumbers.Length);
        foreach (int vertex in vertexNumbers)
        {
            writer.WriteInt(vertex);
        }
        m_Driver.EndSend(writer);
    }

    public void UpdateAllClients()
    {
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (m_Connections[i].IsCreated)
            {
                SendVertexNumbers(m_Connections[i]);
            }
        }
        isClientReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // Clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            Debug.Log($"Accepted a connection: {c.InternalId}");
        }

        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
                continue;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {    
                if (cmd == NetworkEvent.Type.Data)
                {
                    Vector3 currentVertex = new Vector3();
                    Quaternion rotation = new Quaternion();
                    currentVertex.x = stream.ReadFloat();
                    currentVertex.y = stream.ReadFloat();
                    currentVertex.z = stream.ReadFloat();
                    m_faceRoot.transform.position = currentVertex * faceHelper.faceScale;
                    rotation.x = stream.ReadFloat();
                    rotation.y = stream.ReadFloat();
                    rotation.z = stream.ReadFloat();
                    rotation.w = stream.ReadFloat();
                    m_faceRoot.transform.rotation = rotation;
                    //Debug.Log(stream.Length);
                    if(stream.Length > 28)
                        faceHelper.HandleFaceUpdate(stream);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                    isClientReady = false;
                }
            }
        }
    }

    public void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }
}
