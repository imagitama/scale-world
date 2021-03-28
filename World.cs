
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class World : UdonSharpBehaviour
{
    public Slider slider;
    public Toggle masterToggle;
    public Text masterOutputText;

    // synced vars
    bool isMasterOnly = true;
    [UdonSynced] bool syncedIsMasterOnly = true;

    bool AmIMaster()
    {
#if UNITY_EDITOR
        return true;
#else
        return Networking.IsOwner(this.gameObject);
#endif
    }

    void Update()
    {
        if (AmIMaster())
        {
            masterOutputText.text = "You are the master";
            masterToggle.gameObject.SetActive(true);
            masterToggle.isOn = isMasterOnly;

            slider.gameObject.SetActive(true);
        }
        else
        {
#if UNITY_EDITOR
            string ownerName = "NONE";
#else
            VRCPlayerApi owner = Networking.GetOwner(this.gameObject);
            string ownerName = owner.displayName;
#endif

            masterOutputText.text = "The master is: " + ownerName;
            masterToggle.gameObject.SetActive(false);

            if (syncedIsMasterOnly)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.gameObject.SetActive(true);
            }
        }
    }

    public void OnToggleMasterOnly()
    {
        if (AmIMaster())
        {
            Debug.Log("Toggle master only");
            isMasterOnly = !isMasterOnly;
            syncedIsMasterOnly = isMasterOnly;
        }
    }

    public bool IsMasterOnly()
    {
        return syncedIsMasterOnly;
    }
}
