using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public TextMeshProUGUI playerName;

    public Image backgroundImage;

    public Color highlightedColor;

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
    }

    public void ApplyLocalChanges()
    {
        Debug.Log(backgroundImage.color);
        backgroundImage.color = highlightedColor;
    }
}
