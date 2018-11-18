using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls
{
    public static ControlDown down = new ControlDown();
    public static ControlUp up = new ControlUp();
    public static ControlHold hold = new ControlHold();
}

public abstract class ControlBase
{
    // Base
    protected abstract bool Key(KeyCode key);
    protected abstract bool Key(string key);
    protected abstract bool MouseButton(int mouseButton);

    // Character
    public bool Sprint => Key("left shift") || Key("right shift");
    public bool Jump => Key("space");
    public bool Shoot => MouseButton(0);

    // Game core
    public bool Quit => Key("end");
    public bool Esc => Key("escape");

    // Temporary
    public bool CreateHole => Key("k");
    public bool Test1 => Key("l");
}

public class ControlDown : ControlBase
{
    protected override bool Key(KeyCode key) => Input.GetKeyDown(key);
    protected override bool Key(string key) => Input.GetKeyDown(key);
    protected override bool MouseButton(int mouseButton) => Input.GetMouseButtonDown(mouseButton);
}

public class ControlUp : ControlBase
{
    protected override bool Key(KeyCode key) => Input.GetKeyUp(key);
    protected override bool Key(string key) => Input.GetKeyUp(key);
    protected override bool MouseButton(int mouseButton) => Input.GetMouseButtonUp(mouseButton);
}

public class ControlHold : ControlBase
{ 
    protected override bool Key(KeyCode key) => Input.GetKey(key);
    protected override bool Key(string key) => Input.GetKey(key);
    protected override bool MouseButton(int mouseButton) => Input.GetMouseButton(mouseButton);
}