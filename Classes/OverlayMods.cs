using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace StupidTemplate.Mods
{
    public static class OverlayMods
    {
        private static GameObject overlayObject;
        private static RectTransform contentPanel;
        private static GameObject entryPrefab;
        private static Coroutine overlayUpdater;
        private static TextMeshProUGUI playerCountText;

        public static void ToggleLeaderboardOverlay(bool enable)
        {
            if (enable)
            {
                if (overlayObject == null)
                    CreateOverlay();

                overlayObject.SetActive(true);

                if (overlayUpdater == null)
                    overlayUpdater = CoroutineRunner.Run(UpdateOverlayLoop());
            }
            else
            {
                if (overlayObject != null)
                    overlayObject.SetActive(false);

                if (overlayUpdater != null)
                {
                    CoroutineRunner.Stop(overlayUpdater);
                    overlayUpdater = null;
                }
            }
        }

        private static void CreateOverlay()
        {
            overlayObject = new GameObject("LeaderboardOverlay");
            var canvas = overlayObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            overlayObject.AddComponent<CanvasScaler>();
            overlayObject.AddComponent<GraphicRaycaster>();

            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(overlayObject.transform);
            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.75f, 0.5f);
            rect.anchorMax = new Vector2(0.95f, 0.9f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var image = panel.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.5f);

            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = 4f;
            layout.padding = new RectOffset(5, 5, 5, 5);

            contentPanel = panel.GetComponent<RectTransform>();

            // Player count label
            GameObject countTextGO = new GameObject("PlayerCountText");
            countTextGO.transform.SetParent(panel.transform);
            playerCountText = countTextGO.AddComponent<TextMeshProUGUI>();
            playerCountText.fontSize = 22;
            playerCountText.alignment = TextAlignmentOptions.Center;
            playerCountText.text = "Players: 0 / 10";

            var countRect = countTextGO.GetComponent<RectTransform>();
            countRect.sizeDelta = new Vector2(200, 30);

            // Entry prefab
            entryPrefab = new GameObject("EntryPrefab");
            entryPrefab.AddComponent<RectTransform>();

            var button = entryPrefab.AddComponent<Button>();
            var text = entryPrefab.AddComponent<TextMeshProUGUI>();
            text.fontSize = 20;
            text.alignment = TextAlignmentOptions.Left;

            var colors = button.colors;
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.25f);
            button.colors = colors;

            entryPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 25);
            entryPrefab.SetActive(false);
        }

        private static IEnumerator UpdateOverlayLoop()
        {
            while (true)
            {
                if (overlayObject != null && overlayObject.activeSelf)
                    UpdateOverlay();

                yield return new WaitForSeconds(1f);
            }
        }

        private static void UpdateOverlay()
        {
            foreach (Transform child in contentPanel)
            {
                if (child.name != "PlayerCountText")
                    GameObject.Destroy(child.gameObject);
            }

            var players = PlayerManager.GetAllPlayers();
            GameObject localPlayer = PlayerManager.GetLocalPlayer();

            // Update count text
            playerCountText.text = $"Players: {players.Count} / 10";

            foreach (var player in players)
            {
                GameObject entry = GameObject.Instantiate(entryPrefab, contentPanel);
                entry.SetActive(true);

                var text = entry.GetComponent<TextMeshProUGUI>();
                var button = entry.GetComponent<Button>();

                string displayName = player.playerName;
                if (player.playerObject == localPlayer)
                    displayName += " (You)";

                text.text = displayName;
                text.color = player.playerColor;

                GameObject target = player.playerObject;

                button.onClick.AddListener(() =>
                {
                    CameraController.SetTarget(target);
                    CameraMods.StopSpectateRandom();
                });
            }
        }
    }
}
