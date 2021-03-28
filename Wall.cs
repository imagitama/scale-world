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
    void OnTriggerEnter(Collider collider) {
        Debug.Log("OnTriggerEnter");
        scaler.OnTestPlayerNextToWall();
    }

    void OnTriggerExit(Collider collider) {
        Debug.Log("OnTriggerExit");
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