using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace GameBase;
/// <summary>
/// these are tags for more diffreniation
/// ex items and NPCs are both interactable, but tags are for diffrentiation
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Tag
{
    Item,
    Player,
    NPC,
    Enemy

}