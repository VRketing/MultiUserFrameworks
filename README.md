# MultiUserFrameworks
 This Repository was created to test possible Multi User Frameworks for the ViLiEv Application. 

# Packages to be added

- Agora 🟥
- Photon (PUN, Fusion, Voice) ✅
- Colyseus 🟩
- Nakama ✅
- Netcode 🟥

## Photon
To prevent any troubles with missing App IDs the following App IDs were created to test the application:
- Fusion: 74a7982a-0ad6-48fd-8321-552a02792210
- Voice: 504b9418-139f-43ba-904c-8fd1c57012ee
- PUN: 18542fb2-7cce-43db-ac1c-c2cc77f6b4e1

To test Photon follow these steps:

1. Start the MultiUserFrameworks.exe in Builds/Dedicated Server.
2. Open the Scene "Test".
3. Open the Inspector of the "Prototype Network Start" Object and make sure that the option "Auto Start As" is set to "Client".
4. Start the Game Preview
5. The first Client will be spawned automatically. If additional CLients are needed click on the "NetworkManager" Object and then on the button "Add additional Client".

<u>WIP</u>
- Host Dedicated Server on remote machine

## Nakama
To test the Nakama Framework, a few prerequisite Files are needed.

1. Nakama Server Files -> Provided in Server/nakama
2. CockroachDB -> Download necessary files [here](https://www.cockroachlabs.com/docs/releases/). 

For a setup tutorial check out [How to install Nakama](https://heroiclabs.com/docs/nakama/getting-started/install/).

To run the test setup follow these steps:

1. Start a local CockroachDB Server. 
2. If you start up CockroachDB for the first time, migrate up Nakama. This step can be skipped if CockroachDB has already been started once.
3. Start Nakama.exe in Terminal Window

(The Setup Tutorial of Nakama can be referenced for details to the prior three steps.)

4. Start the Game Preview

## Research results and thoughts
As of now the best way to implement a system capable of handling the needed functionalities would be to use Photon for State Synchronization and Voice Chat and Nakama for User Authentication and User State Management (Saving the users Avatar for example and giving it back once logged in again). There are still some frameworks left to test and further discussions might reveal better solutions. 