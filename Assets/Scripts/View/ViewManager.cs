using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class ViewManager : MonoBehaviour
    {
        public delegate void Callback(string value);

        private Callback _callbacks;

        public void AddCallBack(Callback callback)
        {
            _callbacks += callback;
        }
        private List<string> _messages = new List<string>();
        private  string _textToEdit = "";

        public void OnGUI()
        {
            _textToEdit = GUI.TextArea(new Rect(150, 10, 500, 180), _textToEdit);
            if (GUI.Button(new Rect(10, 10, 100, 40), "start"))
            {
                _callbacks("start");
            }

            if (GUI.Button(new Rect(10, 60, 100, 40), "stop"))
            {
                _callbacks("stop");
            }
        }

        private void Start()
        {
            Debug.Log("view start");
        }
        public void SetMessage(string value)
        {

            _messages.Add(value);
            _messages = CheckListLength(_messages);
            
            //var message = DateTime.Now.ToString()+ "\n";
            var message = "";
            var n = _messages.Count;
            for (var i = n - 1; i >= 0; i--)
            {
                message = message + "\n" + _messages[i];
            }

            _textToEdit = message;
            //send(str);
        }
        
        private static List<string> CheckListLength(List<string> list)
        {
            if (list.Count <= 6) return list;
            list.RemoveAt(0);
            CheckListLength(list);
            return list;
        }
    }
}