using System;
using Entitas;
using UnityEngine;

public class EntitasGameController : MonoBehaviour {

    Entitas.Systems _systems;

	// Use this for initialization
	void Start () {
        _systems = new RootSystem(Contexts.sharedInstance);
        FindEntitiesInUnity(Contexts.sharedInstance.game);
        _systems.Initialize();
	}

    private void FindEntitiesInUnity(GameContext game)
    {
        EntitasInit[] toEntitas = GameObject.FindObjectsOfType<EntitasInit>();
        foreach (var u in toEntitas)
        {
            GameEntity e = game.CreateEntity();
            e.AddPosition(u.transform.position.x, u.transform.position.y, u.transform.position.z);
            e.AddComponentsGameObject(u.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        _systems.Execute();
        _systems.Cleanup();
	}
}
