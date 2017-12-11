using Entitas;
using UnityEngine;

public class EntitasGameController : MonoBehaviour {

    Systems _systems;

	// Use this for initialization
	void Start () {
        _systems = new RootSystem(Contexts.sharedInstance);

        _systems.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        _systems.Execute();
        _systems.Cleanup();
	}
}
