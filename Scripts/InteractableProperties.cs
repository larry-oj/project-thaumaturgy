using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Pickups;

namespace projectthaumaturgy.Scripts;

public partial class InteractableProperties : GodotObject
{
    public Dictionary<PackedScene, Array<float>> Interactables { get; set; }
    public int MaxInteractables { get; set; }
    public Dictionary<WeaponResource, float> Weapons { get; set; }

    public InteractableProperties Copy()
    {
        var interactables = new Dictionary<PackedScene, Array<float>>();
        foreach (var (key, value) in Interactables)
        {
            var array = new Array<float>();
            foreach (var f in value)
            {
                array.Add(f);
            }

            interactables.Add(key, array);
        }

        var weapons = new Dictionary<WeaponResource, float>();
        foreach (var (key, value) in Weapons)
        {
            weapons.Add(key, value);
        }

        return new InteractableProperties
        {
            Interactables = interactables,
            MaxInteractables = MaxInteractables,
            Weapons = weapons
        };
    }
}