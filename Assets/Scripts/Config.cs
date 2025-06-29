using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{

    public static readonly int PLAYER_SPEED = 5; 

    public static readonly int PLAYER_SPOT_ANGLE = 90;
    public static readonly float PLAYER_LIGHT_INTENSITY = 0.25f;

    public static readonly float PLAYER_SPOT_DISTANCE = 8f;

    public static readonly float OBJECT_WAKE_TIME = 4;
    public static readonly float OBJECT_MOVE_TIME = 5;

    public static readonly float WIN_DURARION = 4f;

    public static readonly float CLOSEST_DIS_TO_PLAYER_SQR = 25f;
    public static readonly float CLOSEST_DIS_TO_TARGET_SQR = 25f;

    public static readonly string TAG_MOVING_OBJECT = "MovingObject";
    public static readonly string TAG_PLAYER = "Player";
    public static readonly string TAG_ORIGINLOCATION = "OriginLocation";

    public static readonly string COUNTDOWN_TEXT_PREFAB = "Prefabs/CountDownText";
    public static readonly string PLAYER_PREFAB = "Prefabs/Player";
    public static readonly string AUDIO_PATH_HEADER = "Audio/";
}
