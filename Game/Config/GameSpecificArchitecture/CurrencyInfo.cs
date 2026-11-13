using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class CurrencyInfo: IItemProperty
{
    public int amt { get; private set; }

    public CurrencyInfo(int amt)
    {
        this.amt = amt;
    }
}
