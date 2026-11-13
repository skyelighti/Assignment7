using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class HealthData
{
    /// <summary>
    /// for persistence and loading
    /// </summary>
    public int current { get; private set; }
    public int max { get; private set; }

    public event Action Changed;

    public HealthData(int hp)
    {
        current = hp;
        max = hp;
    }

    public void SetCurrent(int hp)
    {
        int value = Math.Clamp(hp, 0, max);

        if (current == value) return;

        current = value;
        Changed?.Invoke();
    }
}
