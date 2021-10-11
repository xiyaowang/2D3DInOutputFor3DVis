using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StudyMismatch2D3D.S23_Output_Screen {

    public static class UdpSetting {
        public static IPAddress IpRemotePC = IPAddress.Parse("192.168.137.212");

        public static IPAddress IpToListen = IPAddress.Any;

        public static int PortToSend = 5568;
        public static int PortToListen = 5567;
    } 

    public class UDPMessageEvent:UnityEvent<string,int,byte[]> {

    }

    public sealed class UdpManager:Singleton<UdpManager> {


        Socket socket; //目标socket
        EndPoint clientEnd; //客户端

        IPEndPoint ipEnd; //侦听端口
        Thread connectThread; //连接线程

        private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();
        public UDPMessageEvent udpEvent = null;
        void UDPMessageReceived(string host,int port,byte[] data) {
            MsgProcessor.Process(data);
        }

        private void Start() {
            //DontDestroyOnLoad(this.gameObject);
            CreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpRemotePC,UdpSetting.PortToSend);
        }

        private void Update() {
            while(ExecuteOnMainThread.Count > 0) {
                ExecuteOnMainThread.Dequeue().Invoke();
            }
        }

        private void OnApplicationQuit() {
            QuitSocket();
        }

        public void SendUDPMessage(byte[] data) {
            socket.SendTo(data,data.Length,SocketFlags.None,clientEnd);
        }

        private void SocketReceive() {
            while(true) {
                byte[] recvData = new byte[1024];
                int recvLen = socket.ReceiveFrom(recvData, ref clientEnd);
                ExecuteOnMainThread.Enqueue(() => {
                    if(udpEvent != null)
                        udpEvent.Invoke(clientEnd.ToString(),UdpSetting.PortToListen,recvData);
                });
            }
        }

        public void RecreateSocket(IPAddress ipToListen,int portToListen,IPAddress ipToSend,int portToSend) {
            QuitSocket();
            CreateSocket(ipToListen,portToListen,ipToSend,portToSend);
        }

        private void CreateSocket(IPAddress ipToListen, int portToListen, IPAddress ipToSend, int portToSend) {
            ipEnd = new IPEndPoint(ipToListen,portToListen);
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            socket.Bind(ipEnd);

            IPEndPoint sender = new IPEndPoint(ipToSend, portToSend);
            clientEnd = (EndPoint)sender;

            connectThread = new Thread(new ThreadStart(SocketReceive));
            connectThread.Start();

            udpEvent = new UDPMessageEvent();
            udpEvent.AddListener(UDPMessageReceived);
        }

        private void QuitSocket() {
            if(connectThread != null) {
                connectThread.Interrupt();
                connectThread.Abort();
            }

            if(socket != null)
                socket.Close();
        }

    }
}