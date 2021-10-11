using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Net;
using System.Net.Sockets;
using System.Threading;
#else
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif

namespace StudyMismatch2D3D.S23_Output_HoloLens {

    public class UDPMessageEvent:UnityEvent<string,string,byte[]> {

    }

    public class UdpManager:Singleton<UdpManager> {

        [Tooltip("port to listen for incoming data")]
        [System.NonSerialized] public string internalPort = "5567";

        [Tooltip("IP-Address for sending")]
        [System.NonSerialized]
        //public string externalIP = "192.168.1.148";
        public string externalIP = "192.168.1.112";

        [Tooltip("Port for sending")]
        [System.NonSerialized]
        public string externalPort = "5568";

#if UNITY_EDITOR
        protected Thread receiveThread;
        protected UdpClient client;
#else
        protected DatagramSocket socket;
#endif


        [Tooltip("Function to invoke at incoming packet")]
        public UDPMessageEvent udpEvent = null;

        protected readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

        void UDPMessageReceived(string host,string port,byte[] data) {
            externalIP = host;
            MsgProcessor.Process(data);
        }


#if UNITY_EDITOR
        void Start() {
            receiveThread = new Thread(new ThreadStart(Socket_MessageReceived)) {
                IsBackground = true
            };
            receiveThread.Start();

            if(udpEvent == null) {
                udpEvent = new UDPMessageEvent();
                udpEvent.AddListener(UDPMessageReceived);
            }
        }

#else
         async void Start()
    {
        if (udpEvent == null)
        {
            udpEvent = new UDPMessageEvent();
            udpEvent.AddListener(UDPMessageReceived);
        }
 
 
        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;
 
        HostName IP = null;
        try
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
 
            IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
            .SingleOrDefault(
                hn =>
                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                    == icp.NetworkAdapter.NetworkAdapterId);
            await socket.BindEndpointAsync(IP, internalPort);
                    Debug.Log("Socket started listening on " + IP + " " + internalPort);

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }
 
    }
#endif

        void Update() {
            while(ExecuteOnMainThread.Count > 0) {
                ExecuteOnMainThread.Dequeue().Invoke();
            }
        }

#if UNITY_EDITOR
        void OnApplicationQuit() {
            if(receiveThread.IsAlive) {
                receiveThread.Abort();
            }
            if(receiveThread != null)
                receiveThread.Abort();
            client.Close();
        }
#else
    void OnApplicationQuit() {
        if (socket != null){
            socket.Dispose();
            socket = null;
        }
    }
#endif

#if UNITY_EDITOR
        public void SendUDPMessage(string message) {
            byte[] data = Encoding.ASCII.GetBytes(message);
            SendUDPMessage(externalIP,externalPort,data);
        }


        public void SendUDPMessage(string HostIP,string HostPort,byte[] data) {

        }
#else

    //Send an UDP-Packet
    public async void SendUDPMessage(string message)
    {
        await _SendUDPMessage(externalIP, externalPort, message);
    }
 
    private async System.Threading.Tasks.Task _SendUDPMessage(string externalIP, string externalPort, string message)
    {
                byte[] data = Encoding.ASCII.GetBytes(message);
        using (var stream = await socket.GetOutputStreamAsync(new Windows.Networking.HostName(externalIP), externalPort))
        {
            using (var writer = new Windows.Storage.Streams.DataWriter(stream))
            {
                writer.WriteBytes(data);
                await writer.StoreAsync();
 
            }
        }

    }

               

#endif

#if UNITY_EDITOR
        private void Socket_MessageReceived() {

            client = new UdpClient(int.Parse(internalPort));
            Debug.Log("UDP listens to any IPAdress on port " + internalPort);

            while(true) {
                try {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = client.Receive(ref anyIP);

                    ExecuteOnMainThread.Enqueue(() => {
                        if(udpEvent != null)
                            udpEvent.Invoke(anyIP.Address.ToString(),internalPort,data);
                    });

                } catch(Exception err) {
                    Debug.Log(err.ToString());
                    OnApplicationQuit();
                }
            }

        }
#else
        private async void Socket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args){
            //Debug.Log("GOT MESSAGE FROM: " + args.RemoteAddress.DisplayName);

            //Stream streamIn = args.GetDataStream().AsStreamForRead();
            ////MemoryStream ms = ToMemoryStream(streamIn);
            ////byte[] msgData = ms.ToArray();
            //StreamReader reader = new StreamReader(streamIn);
            //string message = await reader.ReadLineAsync();
            //byte[] msgData = Encoding.ASCII.GetBytes(message);
        using (var reader = args.GetDataReader())
        {
            var buf = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(buf);
         ExecuteOnMainThread.Enqueue(() => {
                if (udpEvent != null){
                    udpEvent.Invoke(args.RemoteAddress.DisplayName, internalPort, buf);
        }
            });
        }    

           
    }

    //    static MemoryStream ToMemoryStream(Stream input)
    //{
    //    try
    //    {                                         // Read and write in
    //        byte[] block = new byte[0x1000];       // blocks of 4K.
    //        MemoryStream ms = new MemoryStream();
    //        while (true)
    //        {
    //            int bytesRead = input.Read(block, 0, block.Length);
    //            if (bytesRead == 0) return ms;
    //            ms.Write(block, 0, bytesRead);
    //        }
    //    }
    //    finally { }
    //}

#endif
    }
}