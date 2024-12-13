using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private Transform _grabPos;

    public Transform GetGrabPos() => _grabPos; 
}
