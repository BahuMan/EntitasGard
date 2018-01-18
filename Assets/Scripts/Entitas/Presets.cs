using UnityEngine;

public class Presets
{
    public enum EntitasPresetEnum { BASE, BARRACKS, VEHICLE, TURRET }

    private GameContext _game;
    private HexGridBehaviour _grid;

    public Presets(GameContext game, HexGridBehaviour grid)
    {
        _game = game;
        _grid = grid;
    }

    public GameEntity CreateBlueprint(EntitasPresetEnum preset)
    {
        return CreateBlueprint(PresetToPrefab(preset));
    }

    private EntitasInit PresetToPrefab(EntitasPresetEnum preset)
    {
        switch (preset)
        {
            case EntitasPresetEnum.BASE: return GameObject.Instantiate<EntitasInit>(Resources.Load<EntitasInit>("Prefabs/Base"));
            case EntitasPresetEnum.BARRACKS: return GameObject.Instantiate<EntitasInit>(Resources.Load<EntitasInit>("Prefabs/Barracks"));
            case EntitasPresetEnum.VEHICLE: return GameObject.Instantiate<EntitasInit>(Resources.Load<EntitasInit>("Prefabs/Unit"));
            case EntitasPresetEnum.TURRET: return GameObject.Instantiate<EntitasInit>(Resources.Load<EntitasInit>("Prefabs/DefenseTower"));
        }
        Debug.LogError("Preset code couldn't create Unity prefab for " + System.Enum.GetName(typeof(EntitasPresetEnum), preset));
        return null;
    }

    public GameEntity CreateBlueprint(EntitasInit UnityObject)
    {
        switch (UnityObject.EntitasBlueprint)
        {
            case EntitasPresetEnum.BASE: return CreateBase(UnityObject);
            case EntitasPresetEnum.BARRACKS: return CreateBarracks(UnityObject);
            case EntitasPresetEnum.VEHICLE: return CreateVehicle(UnityObject);
            case EntitasPresetEnum.TURRET: return CreateTurret(UnityObject);
        }

        Debug.LogError("Preset code couldn't create Entity for " + UnityObject.name);
        return null;
    }

    private GameEntity CreateBase(EntitasInit unityObject)
    {
        GameEntity ge = CreateCommon(unityObject);

        ge.AddHealth(500);
        ge.isCanBuildTower = true;
        ge.isCanBuildBarracks = true;
        return ge;
    }

    private GameEntity CreateTurret(EntitasInit unityObject)
    {
        GameEntity ge = CreateCommon(unityObject);
        ge.isSelectable = true;
        ge.AddHealth(200);
        ge.AddWeapon(3, 90f, 2f, 2);
        ge.AddWeaponRotation(0f);

        return ge;
    }

    private GameEntity CreateVehicle(EntitasInit unityObject)
    {
        GameEntity ge = CreateCommon(unityObject);

        ge.AddNavigable(90f, .5f);
        ge.AddHealth(50);
        ge.AddWeaponRotation(0);
        ge.AddWeapon(2, 45, 2f, 1); //turret has half turnrate of vehicle to illustrate correction

        return ge;
    }

    private GameEntity CreateBarracks(EntitasInit unityObject)
    {
        GameEntity ge = CreateCommon(unityObject);

        ge.AddHealth(500);
        ge.isCanBuildVehicle = true;

        return ge;
    }

    private GameEntity CreateCommon(EntitasInit unityObject)
    {
        GameEntity ge = _game.CreateEntity();
        ge.AddGameObject(unityObject.gameObject);

        Vector3 pos = unityObject.transform.position;
        Quaternion rot = unityObject.transform.rotation;
        ge.AddWorldCoordinates(pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w);

        HexCellBehaviour cell = _grid.GetCell(_grid.axial_to_cube(_grid.pixel_to_axial(unityObject.transform.position)));
        if (cell != null)
        {
            int id = cell.GetComponent<EntitasLink>().id;
            ge.AddLocation(cell, id);
        }

        HexSelectable selectable = unityObject.GetComponent<HexSelectable>();
        if (selectable != null)
        {
            ge.isSelectable = true;
        }

        TeamColor team = unityObject.GetComponent<TeamColor>();
        if (team != null)
        {
            ge.AddTeam(team.teamNr);
        }

        //now that the entity has been created with a unique ID,
        //put this ID on the unity GameObject for easy reference:
        EntitasLink el = unityObject.gameObject.AddComponent<EntitasLink>();
        el.id = ge.iD.value;

        return ge;
    }
}