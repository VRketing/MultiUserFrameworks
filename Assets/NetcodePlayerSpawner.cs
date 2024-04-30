/*
using Unity.Entities;
using UnityEngine;

public struct PlayerSpawner : IComponentData
{
    public Entity Player;
}

[DisallowMultipleComponent]
public class NetcodePlayerSpawner : MonoBehaviour
{
    public GameObject Player;

    class Baker : Baker<NetcodePlayerSpawner>
    {
        public override void Bake(NetcodePlayerSpawner authoring)
        {
            PlayerSpawner component = default(PlayerSpawner);
            component.Player = GetEntity(authoring.Player, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, component);
        }
    }
}
*/