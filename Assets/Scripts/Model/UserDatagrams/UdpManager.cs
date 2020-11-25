using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Model.UserDatagrams
{
    public class UdpManager : MonoBehaviour
    {
        public delegate void Callback(UdpData udpData);

        private Callback _callbacks;

        public void AddCallback(Callback callback)
        {
            _callbacks += callback;
        }

        private const string Host = "localHost";
        private const int ReceivePort = 10011;
        private const int SendPort = 10012;
        private const string AppType = "app01";
        private UdpClient _client;
        private Thread _thread;

        private void Start()
        {
            _client = new UdpClient();
            _client.Connect(Host, SendPort);
            _thread = new Thread(Receive) {IsBackground = true};
            _thread.Start();
        }

        /// <summary>
        /// UDPで送信
        /// </summary>
        /// <param name="text">送信コメント</param>
        public void Send(string text)
        {
            var data = new UdpData()
            {
                AppType = AppType,
                Text = text
            };
            var json = JsonUtility.ToJson(data, true);
            var dgram = Encoding.UTF8.GetBytes(json);
            _client.Send(dgram, dgram.Length);
        }

        private void Receive()
        {
            var receiver = new UdpClient(ReceivePort);
            //古い書き方のかなぁ。エラーが出る。
            //receiver.Client.ReceiveTimeout = 1000;
            while (true)
            {
                try
                {
                    IPEndPoint anyIp = null;
                    var bytesData = receiver.Receive(ref anyIp);
                    var text = Encoding.UTF8.GetString(bytesData);
                    var udpData = JsonUtility.FromJson<UdpData>(text);
                    _callbacks(udpData);
                }
                catch (Exception err)
                {
                    Debug.Log("catch :" + err);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
        private void OnApplicationQuit()
        {
            _thread.Abort();
        }
    }
}