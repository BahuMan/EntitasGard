﻿using Entitas;

[Input]
public class MouseOverEntity: IComponent
{
    //this field will contain the same id as stored in IDComponent.value
    //and it should have the same type.
    public int value;
}
