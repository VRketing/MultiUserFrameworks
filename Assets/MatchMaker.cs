using Nakama;
using Nakama.Snippets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMaker : MonoBehaviour
{
    public GameObject playerPrefab;
    public NakamaManager manager;
    private Dictionary<string, GameObject>  players = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SetupMatch();
    }

    void SetupMatch()
    {
        //IClient client = new Client("defaultkey");
        ISocket socket = manager.Socket;
        socket.Connected += async () =>
        {
            Debug.Log("we ended up here");
            var match = await socket.CreateMatchAsync("Test");
            await socket.JoinMatchAsync(match.Id);

            foreach (var presence in match.Presences)
            {
                // Spawn a player for this presence and store it in a dictionary by session id.
                var go = Instantiate(playerPrefab);
                players.Add(presence.SessionId, go);
            }
        };

    }
}
