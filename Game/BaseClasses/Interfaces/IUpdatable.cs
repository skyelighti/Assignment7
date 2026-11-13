using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase
{
    /// <summary>
    /// any component that have changing values and need access to the game's update should implement this
    /// reduces unesscary update calls
    /// </summary>
    public interface IUpdatable
    {
        public void Update(GameTime gameTime);
    }
}
