using UnityEngine;

public class Lesson2 : MonoBehaviour
{
    [SerializeField] private int _firstIntegerNumber;
    [SerializeField] private int _secondIntegerNumber;
    [SerializeField] private float _firstFloatNumber;
    [SerializeField] private bool _firstCheck;
    [SerializeField] private MyVector3 _firstVector;
    [SerializeField] private MyVector3 _secondVector;
    [SerializeField] private Vector3 _firstUnityVector;
    
    [SerializeField] private string _firstText;
    private NotAScript _firstScript;
    
    [ContextMenu("CreateNotAScript")]
    private void CreateNotAScript()
    {
        NotAScript script = _firstScript;
        MyVector3 vector = new MyVector3();
    }
    
    [ContextMenu("Hello World")]
    private void HelloWorld()
    {
        HelloWorld0();
        HelloWorld1();
    }

    private void HelloWorld0()
    {
        Debug.Log("Hello world 0");
    }
    
    private void HelloWorld1()
    {
        Debug.Log("Hello world 1");
    }

    
    [ContextMenu("Add")]
    private void Add()
    {
        int result = _firstIntegerNumber + _secondIntegerNumber;
        Debug.Log(result);
    }
}
