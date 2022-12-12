using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    // not a class, it is struct
    public Vector3 direction;

    public const byte MOUSEBUTTON1 = 0x01;
    public const byte MOUSEBUTTON2 = 0x02;
    public byte buttons;

    public Vector3 velocity;
    public float xAxis;
    public float zAxis;

    public bool strafe;
    public bool sprint;

}