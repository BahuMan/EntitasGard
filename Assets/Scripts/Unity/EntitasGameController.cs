using System;
using Entitas;
using UnityEngine;

public class EntitasGameController : MonoBehaviour {

    Entitas.Systems _systems;

	// Use this for initialization
	void Start () {

        //set up auto-id generation:
        Contexts.sharedInstance.game.OnEntityCreated += AddId;

        //create entities for entire grid with hexes:
        HexGridBehaviour _grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        FindEntitiesInUnity(Contexts.sharedInstance.game, _grid);

        //now init system:
        _systems = new RootSystem(Contexts.sharedInstance);
        _systems.Initialize();
	}

    //this method gets called whenever a game entity has been created
    private void AddId(IContext context, IEntity entity)
    {
        (entity as IID).AddID(entity.creationIndex);
    }

    private void FindEntitiesInUnity(GameContext game, HexGridBehaviour grid)
    {
        EntitasInit[] toEntitas = GameObject.FindObjectsOfType<EntitasInit>();
        foreach (var u in toEntitas)
        {
            GameEntity e = game.CreateEntity();
            e.AddGameObject(u.gameObject);
            HexCellBehaviour cell = grid.GetCell(grid.axial_to_cube(grid.pixel_to_axial(u.transform.position)));
            if (cell != null)
            {
                e.AddLocation(cell);
            }
            e.isSelectable = true;
            e.isUnit = true;
        }
    }

    // Update is called once per frame
    void Update () {
        _systems.Execute();
        _systems.Cleanup();
	}
}
