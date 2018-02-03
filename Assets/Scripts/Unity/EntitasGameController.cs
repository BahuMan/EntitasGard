using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitasGameController : MonoBehaviour {

    private Entitas.Systems _systems;

	// Use this for initialization
	void Start () {

        //first init system (important, because I want systems to exist before any entities are created):
        _systems = new RootSystem(Contexts.sharedInstance);
        _systems.Initialize();

        HexGridBehaviour grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        GameContext game = Contexts.sharedInstance.game;
        Presets preset = new Presets(game, grid);

        TurnGridToEntities(game, grid);
        FindEntitiesInUnity(game, preset);
        SetupPlayers(game, preset, grid);
	}

    private void SetupPlayers(GameContext game, Presets preset, HexGridBehaviour grid)
    {
        PlayerListData pld = GameObject.FindObjectOfType<PlayerListData>();
        if (pld != null)
        {
            IEnumerator<Vector3> basePos = CoordinatesForBases(grid);
            foreach (var player in pld._model)
            {

                GameEntity pEntity = game.CreateEntity();
                pEntity.AddTeam(player.Color);
                pEntity.isLocalPlayer = (player.Type == PlayerTypeEnum.Human);
                pEntity.isAIPlayer = (player.Type == PlayerTypeEnum.AI);

                basePos.MoveNext();
                GameEntity baseEntity = preset.CreateBlueprint(Presets.EntitasPresetEnum.BASE);
                HexCellBehaviour home = grid.GetCell(basePos.Current);
                baseEntity.ReplaceLocation(home, home.GetComponent<EntitasLink>().id);
                baseEntity.ReplaceTeam(player.Color);
                baseEntity.ReplaceStartPosition(home.transform.position.x, home.transform.position.y, home.transform.position.z);
            }
        }
        else
        {
            Debug.Log("No player setup found; I'm assuming this scene was started in Unity Editor for debugging purposes");

            GameEntity pEntity = game.CreateEntity();
            pEntity.AddTeam(0);
            pEntity.isLocalPlayer = true;
            pEntity.isAIPlayer = false;

        }
    }

    private IEnumerator<Vector3> CoordinatesForBases(HexGridBehaviour grid)
    {
        yield return new Vector3(0, -grid.size, grid.size);
        yield return new Vector3(-grid.size, grid.size, 0);
        yield return new Vector3(grid.size, 0, -grid.size);
        yield return new Vector3(0, grid.size, -grid.size);
        yield return new Vector3(-grid.size, 0, -grid.size);
        yield return new Vector3(grid.size, -grid.size, 0);
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

    private void FindEntitiesInUnity(GameContext game, Presets preset)
    {
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
