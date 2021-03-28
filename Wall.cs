using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class Wall : UdonSharpBehaviour
{
    public Scaler scaler;

#if UNITY_EDITOR
    void OnCollisionEnter(Collision collision) {
        scaler.OnTestPlayerNextToWall();
    }

    void OnCollisionExit(Collision collision) {
        scaler.OnTestPlayerNotNextToWall();
    }
#endif

    void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        scaler.OnPlayerNextToWall(player);
    }

    void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        scaler.OnPlayerNotNextToWall(player);
    }
}