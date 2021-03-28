
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class Scaler : UdonSharpBehaviour
{
    float speed = 1;
    int currentWorldSize = 1;
    bool isNextToWall = false;
    // inputs
    public World world;
    public Slider slider;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    public GameObject ceiling;
    public GameObject floor;
    public Transform origin;
    public GameObject testPlayer;

    // synced stuff
    int finalWorldSize = 1;
    [UdonSynced] int syncedFinalWorldSize = 1;

    public void OnScaleSliderValueChanged()
    {
#if UNITY_EDITOR

#else
        // only owners can sync vars
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
#endif

        finalWorldSize = (int)slider.value;
        syncedFinalWorldSize = finalWorldSize;
    }

    bool AmIOwner()
    {
#if UNITY_EDITOR
        return true;
#else
        return Networking.IsOwner(this.gameObject);
#endif
    }

    public void OnPlayerNextToWall(VRCPlayerApi player)
    {
        if (player != Networking.LocalPlayer)
        {
            return;
        }

        isNextToWall = true;
    }

    public void OnPlayerNotNextToWall(VRCPlayerApi player)
    {
        if (player != Networking.LocalPlayer)
        {
            return;
        }

        isNextToWall = false;
    }

#if UNITY_EDITOR
    public void OnTestPlayerNextToWall() {
        Debug.Log("Test Player Next To Wall");
        isNextToWall = true;
    }

    public void OnTestPlayerNotNextToWall() {
        Debug.Log("Test Player NOT Next To Wall");
        isNextToWall = false;
    }
#endif

    void UpdatePlayerPosition()
    {
        if (isNextToWall == false)
        {
            return;
        }

        if (currentWorldSize == 1)
        {
            return;
        }

        Vector3 newPositionCloserToOrigin = Vector3.Lerp(GetLocalPlayerPosition(), origin.position, 0.05f);

#if UNITY_EDITOR
        testPlayer.transform.position = newPositionCloserToOrigin;
#else
        Networking.LocalPlayer.TeleportTo(newPositionCloserToOrigin, Networking.LocalPlayer.GetRotation());
#endif
    }

    Vector3 GetLocalPlayerPosition()
    {
#if UNITY_EDITOR
        return testPlayer.transform.position;
#else
        return Networking.LocalPlayer.GetPosition();
#endif
    }

    void Update()
    {
        if (currentWorldSize == syncedFinalWorldSize)
        {
            return;
        }

        UpdatePlayerPosition();

        Vector3 finalScale = new Vector3(syncedFinalWorldSize, syncedFinalWorldSize, syncedFinalWorldSize);
        Vector3 lerpedScale = Vector3.Lerp(floor.transform.localScale, finalScale, speed * Time.deltaTime);
        lerpedScale.y = 0.05f;
        floor.transform.localScale = lerpedScale;
        ceiling.transform.localScale = lerpedScale;

        Vector3 newWall1Scale = wall1.transform.localScale;
        newWall1Scale.z = lerpedScale.z * 2;
        wall1.transform.localScale = newWall1Scale;

        Vector3 newWall2Scale = wall2.transform.localScale;
        newWall2Scale.z = lerpedScale.z * 2;
        wall2.transform.localScale = newWall2Scale;

        // move
        Vector3 newWall3Position = wall3.transform.localPosition;
        newWall3Position.x = lerpedScale.z - 0.475f;
        wall3.transform.localPosition = newWall3Position;

        // scale
        Vector3 newWall3Scale = wall3.transform.localScale;
        newWall3Scale.z = lerpedScale.z * 2;
        wall3.transform.localScale = newWall3Scale;

        // move
        Vector3 newWall4Position = wall4.transform.localPosition;
        newWall4Position.z = lerpedScale.z - 0.475f;
        wall4.transform.localPosition = newWall4Position;

        // scale
        Vector3 newWall4Scale = wall4.transform.localScale;
        newWall4Scale.z = lerpedScale.z * 2;
        wall4.transform.localScale = newWall4Scale;

        if (
            (lerpedScale.x < finalScale.x && finalScale.x - lerpedScale.x < 0.005f) ||
            (lerpedScale.x > finalScale.x && lerpedScale.x - finalScale.x < 0.005f)
        )
        {
            finalScale.y = 0.05f;
            floor.transform.localScale = finalScale;
            ceiling.transform.localScale = finalScale;

            Vector3 finalWall1Scale = wall1.transform.localScale;
            finalWall1Scale.z = finalScale.z * 2;
            wall1.transform.localScale = finalWall1Scale;

            Vector3 finalWall2Scale = wall2.transform.localScale;
            finalWall2Scale.z = finalScale.z * 2;
            wall2.transform.localScale = finalWall2Scale;

            // move
            Vector3 finalWall3Position = wall3.transform.localPosition;
            finalWall3Position.x = lerpedScale.z - 0.475f;
            wall3.transform.localPosition = finalWall3Position;

            // position
            Vector3 finalWall3Scale = wall3.transform.localScale;
            finalWall3Scale.z = finalScale.z * 2;
            wall3.transform.localScale = finalWall3Scale;

            // move
            Vector3 finalWall4Position = wall4.transform.localPosition;
            finalWall4Position.z = lerpedScale.z - 0.475f;
            wall4.transform.localPosition = finalWall4Position;

            // position
            Vector3 finalWall4Scale = wall4.transform.localScale;
            finalWall4Scale.z = finalScale.z * 2;
            wall4.transform.localScale = finalWall4Scale;

            currentWorldSize = syncedFinalWorldSize;
        }
    }
}
