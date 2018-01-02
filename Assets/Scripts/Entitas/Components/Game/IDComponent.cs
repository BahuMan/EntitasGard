using Entitas;
using Entitas.CodeGeneration.Attributes;

//this component gets added automatically to GameEntities
//in EntitasGameController.cs
//use it to reference other GameEntities by id rather than by pointer
//find an entity like this: Contexts.sharedInstance.game.GetEntityWithID();
[Game, Input]
public class IDComponent: IComponent
{
    [PrimaryEntityIndex]
    public int value;
}
