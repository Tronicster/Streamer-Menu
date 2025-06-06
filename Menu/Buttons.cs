using StupidTemplate.Classes;
using StupidTemplate.Mods;
using static StupidTemplate.Settings;

namespace StupidTemplate.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "Settings", method =() => SettingsMods.EnterSettings(), isTogglable = false, toolTip = "Opens the main settings page for the menu."},

                new ButtonInfo {
                    buttonText = "Spectate Random Every 5s",
                    isTogglable = true,
                    enableMethod = () => CameraMods.StartSpectateRandom(),
                    disableMethod = () => CameraMods.StopSpectateRandom(),
                    toolTip = "Toggle spectating a random player every 5 seconds."
                },
                new ButtonInfo {
                    buttonText = "Spectate Self",
                    isTogglable = false,
                    method = () => CameraMods.SpectateSelf(),
                    toolTip = "Switch camera to yourself."
                },

                // New Leaderboard Overlay toggle button
                new ButtonInfo {
                    buttonText = "Leaderboard Overlay",
                    isTogglable = true,
                    enableMethod = () => OverlayMods.ToggleLeaderboardOverlay(true),
                    disableMethod = () => OverlayMods.ToggleLeaderboardOverlay(false),
                    toolTip = "Toggle the lobby player list overlay with player colors."
                },

                // Other placeholders below if needed
                new ButtonInfo { buttonText = "regular placeholder", isTogglable = false},
                new ButtonInfo { buttonText = "togglable placeholder"},
                new ButtonInfo { buttonText = "regular placeholder 2", isTogglable = false},
                new ButtonInfo { buttonText = "togglable placeholder 2"},
                new ButtonInfo { buttonText = "regular placeholder 3", isTogglable = false},
                new ButtonInfo { buttonText = "togglable placeholder 3"},
                new ButtonInfo { buttonText = "regular placeholder 4", isTogglable = false},
                new ButtonInfo { buttonText = "togglable placeholder 4"},
                new ButtonInfo { buttonText = "regular placeholder 5", isTogglable = false},
                new ButtonInfo { buttonText = "togglable placeholder 5"},
                new ButtonInfo { buttonText = "regular placeholder 6", isTogglable = false},
                new ButtonInfo { buttonText = "togglable placeholder 6"},
            },

            // Other menu pages unchanged
        };
    }
}
