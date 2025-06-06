using System.Collections.Generic;
using UnityEngine;

public struct PlayerInfo
{
    public string playerName;
    public Color playerColor;
    public GameObject playerObject;

    public PlayerInfo(string name, Color color, GameObject obj)
    {
        playerName = name;
        playerColor = color;
        playerObject = obj;
    }
}

public static class PlayerManager
{
    public static List<PlayerInfo> GetAllPlayers()
    {
        List<PlayerInfo> players = new List<PlayerInfo>();

        foreach (VRRig rig in Object.FindObjectsOfType<VRRig>())
        {
            if (rig != null && !rig.isOfflineVRRig)
            {
                string name = "Unknown";
                if (rig.creator != null)
                    name = rig.creator.name;
                else if (rig.playerText != null)
                    name = rig.playerText.text;
                else
                    name = rig.gameObject.name;

                players.Add(new PlayerInfo(name, rig.playerColor, rig.gameObject));
            }
        }

        return players;
    }

    public static GameObject GetLocalPlayer()
    {
        return GorillaTagger.Instance?.myVRRig?.gameObject;
    }
}
