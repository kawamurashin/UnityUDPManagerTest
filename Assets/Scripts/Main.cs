using Controller;
using UnityEngine;
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        var obj = new GameObject("ControllerManager");
        obj.AddComponent<ControllerManager>();
        obj.transform.parent = transform;
    }
}
