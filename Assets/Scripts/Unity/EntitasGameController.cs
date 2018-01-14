using UnityEngine;
using Index;

public class EntitasGameController : MonoBehaviour {

    Entitas.Systems _systems;

	// Use this for initialization
	void Start () {

        //first init system (important, because I want systems to exist before any entities are created):
        _systems = new RootSystem(Contexts.sharedInstance);
        _systems.Initialize();

        //create entities for entire grid with hexes:
        HexGridBehaviour grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        TurnGridToEntities(Contexts.sharedInstance.game, grid);

        //create entities for any other scene object:
        FindEntitiesInUnity(Contexts.sharedInstance.game, grid);
	}

    private void TurnGridToEntities(GameContext game, HexGridBehaviour grid)
    {
        for(var c=grid.GetCellEnumerator(); c.MoveNext();)
        {
            HexCellBehaviour cell = c.Current;
            GameEntity ge = game.CreateEntity();

            Vector3 world = cell.transform.position;
            Vector3 cube = cell.cubeCoordinates;
            Quaternion rot = cell.transform.rotation;
            ge.AddHexCell(world.x, world.y, world.z, cube.x, cube.y, cube.z);
            ge.AddGameObject(cell.gameObject);
            ge.AddWorldCoordinates(world.x, world.y, world.z, rot.x, rot.y, rot.z, rot.w);

            //now that the entity has been created with a unique ID,
            //put this ID on the unity GameObject for easy reference:
            EntitasLink el = cell.gameObject.AddComponent<EntitasLink>();
            el.id = ge.iD.value;
        }
    }

    private void FindEntitiesInUnity(GameContext game, HexGridBehaviour grid)
    {
        Presets preset = new Presets(game, grid);
        EntitasInit[] toEntitas = GameObject.FindObjectsOfType<EntitasInit>();
        foreach (var u in toEntitas)
        {
            preset.CreateBlueprint(u);
        }
    }

    // Update is called once per frame
    void Update () {
        _systems.Execute();
        _systems.Cleanup();
	}
}
