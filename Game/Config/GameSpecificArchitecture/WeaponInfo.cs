using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class WeaponInfo : IItemProperty
{
    public int dmgamt { get; private set; }

    public WeaponInfo(int amt) {
        dmgamt = amt;
    }
}
