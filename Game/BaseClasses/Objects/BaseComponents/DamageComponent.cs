using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class DamageComponent : Component, IDamagable, IPersistent
{
    public (int curr, int max) health => (data.current, data.max);
    HealthData data;
    public event Action DmgTaken;
    public event Action Death;
    public event Action Heal;
    public DamageComponent(HealthData data, GameObject o) : base(o)
    {
        this.data = data;
    }
    public DamageComponent(int hp, GameObject o): this(new HealthData(hp), o)
    {
    }

    public void takeDamage(int dmg)
    {
        if (!activeSelf||health.curr <= 0 || dmg < 0 || dmg == 0) { return; }
        if (health.curr - dmg <= 0)
        {
            data.SetCurrent(0);
            Death?.Invoke();
        }
        else {
            data.SetCurrent(data.current - dmg);
        }
        DmgTaken?.Invoke();
    }
    public void healHP(int hp)
    {
        if (health.curr <= 0 || hp < 0|| hp == 0) { return; }
        if (health.curr + hp >= health.max)
        {
            data.SetCurrent(data.max);
        }
        else
        {
            data.SetCurrent(data.current + hp);
        }
        Heal?.Invoke();
    }

    public void CaptureState(GameObjectSaveData data)
    {
        data.Health = health.curr;
    }

    public void RestoreState(GameObjectSaveData sdata)
    {
        data.SetCurrent((int)sdata.Health);
    }

    public override void OnDestroy()
    {
        
    }

    public override void Connect()
    {
        
    }
}
