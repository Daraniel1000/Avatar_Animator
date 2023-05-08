using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using Unity.Networking.Transport;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Assets.Scenes.FaceTracking;
using MessagePack;
using MessagePack.Resolvers;
using Assets.Scripts.Helpers;

public class ServerBehaviour : MonoBehaviour
{

    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    public GameObject m_vertexObject;

    public Text fpsText;
    public Text updateText;
    public Text updateTextMediapipe;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    private FaceHelper faceHelper;
    private BodyHelper bodyHelper;
    private int nframes = 0;
    private UDPReceiver receiver;
    private BinaryFormatter formatter = new BinaryFormatter();

    // Start is called before the first frame update
    void Start()
    {
        faceHelper = new FaceHelper();
        bodyHelper = new BodyHelper(m_vertexObject);
        try
        {
            receiver = new UDPReceiver();
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
            Application.Quit();
        }

        //m_Driver = NetworkDriver.Create();
        //var endpoint = NetworkEndPoint.AnyIpv4;
        //endpoint.Port = 9000;
        //if (m_Driver.Bind(endpoint) != 0)
        //    Debug.Log("Failed to bind to port 9000");
        //else
        //{
        //    m_Driver.Listen();
        //    Debug.Log("Listening on port 9000");
        //}

        //m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        //foreach (var vertex in vertexNumbers)
        //{
        //    vertexObjects.Add(Instantiate(m_vertexObject));
        //}
        Application.targetFrameRate = 60;
        StaticCompositeResolver.Instance.Register(
                 MessagePack.Resolvers.GeneratedResolver.Instance,
                 MessagePack.Resolvers.StandardResolver.Instance
            );

        var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

        MessagePackSerializer.DefaultOptions = option;
    }

    private void SendVertexNumbers(NetworkConnection c)
    {
        m_Driver.BeginSend(c, out var writer);
        writer.WriteInt(faceHelper.vertexNumbers.Count);
        foreach (int vertex in faceHelper.vertexNumbers)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
            fpsText.text = m_lastFramerate.ToString();
        }
        //m_Driver.ScheduleUpdate().Complete();
        var message = receiver.PopLocalMessage();
        if (message != null)
        {
            updateTextMediapipe.text = $"MediaPipe: {nframes} since";
            nframes = -1;
            BodyData data = MessagePackSerializer.Deserialize<BodyData>(message);
            bodyHelper.HandleBodyUpdate(data);
            //Debug.Log($"Local message: {data.fps}");
            //hands
        }
        message = receiver.PopMobileMessage();
        if (message != null)
        {
            //Debug.Log($"Mobile message: {System.Text.Encoding.ASCII.GetString(message, 0, message.Length)}");
            updateText.text = $"ArCore: {nframes} since";
            nframes = -1;
            using (var stream = new MemoryStream(message))
            {
                faceHelper.HandleFaceUpdate(formatter.Deserialize(stream) as FaceKeypoints);
            }
            //var keypoints = JsonConvert.DeserializeObject<FaceKeypoints>(System.Text.Encoding.ASCII.GetString(message, 0, message.Length));
        }
        ++nframes;

        //Clean up connections
        //for (int i = 0; i < m_Connections.Length; i++)
        //{
        //    if (!m_Connections[i].IsCreated)
        //    {
        //        m_Connections.RemoveAtSwapBack(i);
        //        --i;
        //    }
        //}

        //Accept new connections
        //NetworkConnection c;
        //while ((c = m_Driver.Accept()) != default(NetworkConnection))
        //{
        //    m_Connections.Add(c);
        //    Debug.Log($"Accepted a connection: {c.InternalId}");
        //}

        //DataStreamReader stream;
        //NetworkEvent.Type cmd;
        //for (int i = 0; i < m_Connections.Length; i++)
        //{
        //    if (!m_Connections[i].IsCreated)
        //        continue;
        //    while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
        //    {
        //        if (cmd == NetworkEvent.Type.Data)
        //        {
        //            faceHelper.HandleFaceUpdate(stream);
        //            updateText.text = $"{nframes} frames since last update";
        //            nframes = 0;
        //        }
        //        else if (cmd == NetworkEvent.Type.Disconnect)
        //        {
        //            Debug.Log("Client disconnected from server");
        //            m_Connections[i] = default(NetworkConnection);
        //        }
        //    }
        //}
    }

    public void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
        receiver.Close();
    }
}
