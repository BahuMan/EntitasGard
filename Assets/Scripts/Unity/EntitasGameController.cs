﻿using UnityEngine;

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
        for(var c=grid.AllCells(); c.MoveNext();)
        {
            HexCellBehaviour cell = c.Current;
            GameEntity ge = game.CreateEntity();

            Vector3 world = cell.transform.position;
            Vector3 cube = cell.cubeCoordinates;
            ge.AddHexCell(world.x, world.y, world.z, cube.x, cube.y, cube.z);
            ge.AddGameObject(cell.gameObject);

            //now that the entity has been created with a unique ID,
            //put this ID on the unity GameObject for easy reference:
            EntitasLink el = cell.gameObject.AddComponent<EntitasLink>();
            el.id = ge.iD.value;
        }
    }

    private void FindEntitiesInUnity(GameContext game, HexGridBehaviour grid)
    {
        EntitasInit[] toEntitas = GameObject.FindObjectsOfType<EntitasInit>();
        foreach (var u in toEntitas)
        {
            GameEntity ge = game.CreateEntity();
            ge.AddGameObject(u.gameObject);
            HexCellBehaviour cell = grid.GetCell(grid.axial_to_cube(grid.pixel_to_axial(u.transform.position)));
            if (cell != null)
            {
                ge.AddLocation(cell);
            }
            ge.isSelectable = true;
            ge.isUnit = true;

            //now that the entity has been created with a unique ID,
            //put this ID on the unity GameObject for easy reference:
            EntitasLink el = u.gameObject.AddComponent<EntitasLink>();
            el.id = ge.iD.value;
        }
    }

    // Update is called once per frame
    void Update () {
        _systems.Execute();
        _systems.Cleanup();
	}
}
