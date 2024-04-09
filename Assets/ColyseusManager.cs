using Colyseus;
using Colyseus.Schema;
using SchemaTest.InheritedTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColyseusManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    async void Start()
    {
        ColyseusClient client = new ColyseusClient("ws://localhost:2567");
        //Debug.Log(client);
        ColyseusRoom<MyRoomState> room = await client.JoinOrCreate<MyRoomState>("my_room");
        // Something has been added to Schema
        room.State.players.OnAdd += (key, player) =>
        {
            Debug.Log($"{key} has joined the Game!");
            Instantiate(playerPrefab);
            _players.Add(key, playerPrefab);
        };

        // Something has changed in Schema
        room.State.players.OnChange += (key, player) =>
        {
            Debug.Log($"{key} has been changed!");
        };

        // Something has been removed from Schema
        room.State.players.OnRemove += (key, player) =>
        {
            Debug.Log($"{key} has left the Game!");
            if (_players.ContainsKey(key))
            {
                Destroy(_players[key]);
                _players.Remove(key);
            }
        };
    }
}

public partial class MyRoomState : Schema
{
    
    public MapSchema<Player> players = new MapSchema<Player>();
}
