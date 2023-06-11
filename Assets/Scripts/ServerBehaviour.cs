using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scenes.FaceTracking;
using MessagePack;
using MessagePack.Resolvers;
using Assets.Scripts.Helpers;

public class ServerBehaviour : MonoBehaviour
{
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
    private int nframes = 0, nframes1 = 0;
    private UDPReceiver receiver;
    private readonly BinaryFormatter formatter = new BinaryFormatter();

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

        StaticCompositeResolver.Instance.Register(
                 MessagePack.Resolvers.GeneratedResolver.Instance,
                 MessagePack.Resolvers.StandardResolver.Instance
            );
        var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
        MessagePackSerializer.DefaultOptions = option;

        //Application.targetFrameRate = 60;
    }

    public void CalibrateFace()
    {
        faceHelper.CalibrateFace();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFramerate();

        var message = receiver.PopLocalMessage();
        if (message != null)
        {
            updateTextMediapipe.text = $"MediaPipe: {nframes} since";
            nframes = -1;
            BodyData data = MessagePackSerializer.Deserialize<BodyData>(message);
            bodyHelper.Preview(data);
            bodyHelper.HandleBodyUpdate(data);
        }
        message = receiver.PopMobileMessage();
        if (message != null)
        {
            updateText.text = $"ArKit: {nframes1} since";
            nframes1 = -1;
            using (var stream = new MemoryStream(message))
            {
                faceHelper.HandleFaceUpdate(formatter.Deserialize(stream) as FaceKeypoints);
            }
        }

        ++nframes;
        ++nframes1;
    }

    private void CalculateFramerate()
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
    }

    public void OnDestroy()
    {
        receiver.Close();
    }
}
