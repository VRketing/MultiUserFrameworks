// Copyright 2019 The Nakama Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;
using System.Collections.Generic;
using UnityEngine;
using PimDeWitte.UnityMainThreadDispatcher;

namespace Nakama.Snippets
{
    public class MatchmakerWithRelayedMultiplayer : MonoBehaviour
    {
        private readonly IClient _client = new Client("defaultkey");
        private ISocket _socket;
        public PlayerSpawner playerSpawner;
        public int playerCount = 0;
        private async void Start()
        {
            
            var deviceId = SystemInfo.deviceUniqueIdentifier;
            var session = await _client.AuthenticateDeviceAsync(deviceId);
            Debug.Log(session);
            _socket = _client.NewSocket();
            _socket.Connected += () => Debug.Log("Socket connected.");
            _socket.Closed += () => Debug.Log("Socket closed.");
            _socket.ReceivedError += Debug.LogError;
            IUserPresence self = null;
            var connectedOpponents = new List<IUserPresence>(2);
            _socket.ReceivedMatchPresence += matchPresenceEvent =>
            {
                connectedOpponents.AddRange(matchPresenceEvent.Joins);
                // For each player that has joined in this event...
                foreach (var presence in matchPresenceEvent.Joins)
                {
                    // Spawn a player for this presence and store it in a dictionary by session id.
                    UnityMainThreadDispatcher.Instance().Enqueue(playerSpawner.SpawnPlayer(presence.SessionId));
                }

                // For each player that has left in this event...
                foreach (var presence in matchPresenceEvent.Leaves)
                {
                    // Remove the player from the game if they've been spawned
                    UnityMainThreadDispatcher.Instance().Enqueue(playerSpawner.RemovePlayer(presence.SessionId));

                }

            };

            _socket.ReceivedMatchmakerMatched += async matched =>
            {
                Debug.LogFormat("Matched result: {0}", matched);
                var match = await _socket.JoinMatchAsync(matched);

                self = match.Self;
                Debug.LogFormat("Self: {0}", self);
                connectedOpponents.AddRange(match.Presences);
            };
           
            await _socket.ConnectAsync(session);
            Debug.Log("After socket connected.");
            await _socket.AddMatchmakerAsync("*",20,20);



            // NOTE As an example create a second user and socket to matchmake against.
            for (int i = 0; i < playerCount; i++)
            {
                var deviceId2 = Guid.NewGuid().ToString();
                var session2 = await _client.AuthenticateDeviceAsync(deviceId2);
                var socket2 = _client.NewSocket();
                socket2.ReceivedMatchmakerMatched += async matched => await socket2.JoinMatchAsync(matched);
                socket2.ReceivedMatchPresence += matchPresenceEvent =>
                {
                    connectedOpponents.AddRange(matchPresenceEvent.Joins);
                    // For each player that has joined in this event...
                    foreach (var presence in matchPresenceEvent.Joins)
                    {
                        // Spawn a player for this presence and store it in a dictionary by session id.
                        UnityMainThreadDispatcher.Instance().Enqueue(playerSpawner.SpawnPlayer(presence.SessionId));
                    }

                    // For each player that has left in this event...
                    foreach (var presence in matchPresenceEvent.Leaves)
                    {
                        // Remove the player from the game if they've been spawned
                        UnityMainThreadDispatcher.Instance().Enqueue(playerSpawner.RemovePlayer(presence.SessionId));

                    }

                };
                await socket2.ConnectAsync(session2);
                await socket2.AddMatchmakerAsync("*", 20, 20);
            }
            //await socket2.JoinMatchAsync(match.Id);
            //await Task.Delay(TimeSpan.FromSeconds(10)); // disconnect after 10 seconds.
            Debug.Log("After delay socket2 closed.");
            //await socket2.CloseAsync();
        }

        private void OnApplicationQuit()
        {
            _socket?.CloseAsync();
        }
    }
}
