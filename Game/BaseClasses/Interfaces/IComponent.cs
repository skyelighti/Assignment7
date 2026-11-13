using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// anything that is a component will always require an owner and a function to connect it
/// </summary>
public interface IComponent : IObject
{
    GameObject owner { get; }

    public void Connect();
}
