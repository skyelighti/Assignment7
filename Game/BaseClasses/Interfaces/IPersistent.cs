using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// this is mainly used for components where if the gameobject is meant to be saved, it should also be saved alongside it
/// the nice part of this is even if the component implements Ipersistent, but the gameobject does not hold a persistenceComponent, nothing will save
/// </summary>
public interface IPersistent
{
    // keeping ID out of this interface because this is for components generally and components dont need ID?
    void CaptureState(GameObjectSaveData data);
    void RestoreState(GameObjectSaveData data);
}