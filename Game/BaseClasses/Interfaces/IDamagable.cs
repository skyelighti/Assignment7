using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// anythiing that implements this must be able to take damage
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// int max is a useful measure for displaying healthbars, and configuring them through code and acts as top limit to the amount of HP if needed
    /// curr serves to represent the current amount of health an entity has
    /// </summary>
    public (int curr, int max) health { get; }
    //have  this be protected set
    public void takeDamage(int dmg);

    //class implements how damage works
    // maybe also add a ui thing, where it should have a method to update UI?
    public event Action DmgTaken;
    public event Action Death;
}
