using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] Transform _grabPoint;

    public Transform GetGrabPoint() => _grabPoint;

}
