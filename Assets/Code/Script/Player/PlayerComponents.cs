using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    public ObjectGrow ObjectGrow;
    public FollowTarget CameraFollow;
    public RotateByTouch CameraRotate;
    public Transform CameraPosition;
    public EntityActionsManagment PlayerActionsManagment;
    public Transform PlayerTransform;
    public HUD PlayerUI;
    public PlayerSaveData PlayerSaveData;
    public Rigidbody PlayerRigidbody => ObjectGrow.ObjectPhysics;
}
