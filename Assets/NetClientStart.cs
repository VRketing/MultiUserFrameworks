using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetClientStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.StartClient();
    }
}
