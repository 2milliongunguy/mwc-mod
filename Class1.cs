using System.Linq;
using BepInEx;
using HutongGames.PlayMaker;
using UnityEngine;

// using UnityStandardAssets.Characters.FirstPerson;

[BepInPlugin("com.2milliongunguy.mwcmod", "MWC Mod", "1.0.0")]
public class MWCMod : BaseUnityPlugin
{
    void Start()
    {
        Logger.LogInfo("MWC Custom Mod Loaded!");
    }

    private GameObject player;
    private bool isInGame = false;
    private bool showMenu = false;

    private PlayMakerFSM cursorFSM;

    void Update()
    {
        if (!isInGame)
        {
            try
            {
                player = GameObject.Find("PLAYER");
                if (player != null)
                {
                    isInGame = true;
                    Logger.LogInfo("Player object found, mod is now active.");

                    if (cursorFSM == null)
                    {
                        // actually use the playerMakerFSM to find the cursor FSM
                        cursorFSM = PlayMakerFSM.FindFsmOnGameObject(player, "Update Cursor");

                        if (cursorFSM != null)
                            Logger.LogInfo("Cursor FSM found.");
                        else
                            Logger.LogInfo("Cursor FSM not found on player object.");
                    }
                }
                else
                {
                    Logger.LogInfo("Player object not found yet.");
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogInfo("Error while searching for player object: " + ex.Message);
            }
        }

        if (isInGame && Input.GetKeyDown(KeyCode.F6))
        {
            Logger.LogInfo(
                "F6 key pressed - executing mod functionality. Menu is now "
                    + (showMenu ? "hidden" : "shown")
                    + "."
            );

            showMenu = !showMenu;
            Logger.LogInfo("Toggling menu visibility to: " + showMenu);
            if (cursorFSM != null)
            {
                try
                {
                    string eventName = showMenu ? "MENU" : "FINISHED";
                    Logger.LogInfo("Sending event to Cursor FSM: " + eventName);
                    if (cursorFSM.enabled == false)
                    {
                        cursorFSM.enabled = true;
                        Logger.LogInfo("Cursor FSM was disabled, now enabled.");
                    }
                    cursorFSM.SendEvent(eventName);
                    Logger.LogInfo("Sent event to Cursor FSM: " + eventName);
                }
                catch (System.Exception ex)
                {
                    Logger.LogInfo("Error sending event to Cursor FSM: " + ex.Message);
                }
            }
        }
    }

    void OnGUI()
    {
        if (isInGame && showMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            float menuWidth = 200;
            float menuHeight = 150;

            float x = (Screen.width - menuWidth) / 2;
            float y = (Screen.height - menuHeight) / 2;

            GUI.Box(new Rect(x, y, menuWidth, menuHeight), "MWC Mod Menu");

            if (GUI.Button(new Rect(x + 10, y + 30, 180, 30), "Do Action 1"))
            {
                Logger.LogInfo("Action 1 button clicked.");
            }
        }
    }
}
