using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace StupidTemplate.Mods
{
    internal static class CameraMods
    {
        private static Coroutine spectateRandomCoroutine;

        public static void StartSpectateRandom()
        {
            if (spectateRandomCoroutine != null)
            {
                CoroutineRunner.Stop(spectateRandomCoroutine);
                spectateRandomCoroutine = null;
            }
            spectateRandomCoroutine = CoroutineRunner.Run(SpectateRandomLoop());
        }

        public static void StopSpectateRandom()
        {
            if (spectateRandomCoroutine != null)
            {
                CoroutineRunner.Stop(spectateRandomCoroutine);
                spectateRandomCoroutine = null;
            }
        }

        private static IEnumerator SpectateRandomLoop()
        {
            while (true)
            {
                List<PlayerInfo> players = PlayerManager.GetAllPlayers();

                if (players != null && players.Count > 0)
                {
                    int randomIndex = Random.Range(0, players.Count);
                    PlayerInfo randomPlayer = players[randomIndex];

                    if (randomPlayer.playerObject != null)
                        CameraController.SetTarget(randomPlayer.playerObject);
                }

                yield return new WaitForSeconds(5f);
            }
        }

        public static void SpectateSelf()
        {
            GameObject selfPlayer = PlayerManager.GetLocalPlayer();

            if (selfPlayer != null)
            {
                CameraController.SetTarget(selfPlayer);
                StopSpectateRandom();
            }
        }
    }
}
