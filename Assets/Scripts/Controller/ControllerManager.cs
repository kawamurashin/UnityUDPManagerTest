using System;
using Model;
using Model.UserDatagrams;
using UnityEngine;
using View;

namespace Controller
{
    public class ControllerManager : MonoBehaviour
    {
        private ModelManager _modelManager;
        private ViewManager _viewManager;

        private void Start()
        {
            Debug.Log("controller start start");
            _modelManager = ModelManager.Instance();
            _modelManager.gameObject.transform.parent = transform;

            _modelManager.AddCallBack(ModelCallback);
            var obj = new GameObject("View Manager");
            _viewManager = obj.AddComponent<ViewManager>();
            obj.transform.parent = transform;
            _viewManager.AddCallBack(ViewCallback);
            Debug.Log("controller start end");
            SetViewMessage("init");
            
            
        }

        private void ViewCallback(string value)
        {
            SetViewMessage(value);
            _modelManager.SendUdp(value);
        }

        private void ModelCallback(UdpData udpData)
        {
            Debug.Log("udp :" + udpData.Text);
            SetViewMessage(udpData.Text);
        }
        //
        private void SetViewMessage(string text)
        {
            var dateTime = DateTime.Now;
            var h = dateTime.Hour;
            var m = (dateTime.Minute + 100).ToString().Substring(1);
            var s = (dateTime.Second + 100).ToString().Substring(1);
            var ms = (dateTime.Millisecond + 1000).ToString().Substring(1);
            var str = "[" + h + ":" + m + ":" + s + ":" + ms + "]" + text;
            
            _viewManager.SetMessage(str);
        }
    }
}