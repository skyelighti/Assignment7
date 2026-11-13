using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public interface IObject
{
    public bool activeSelf { get; }
    public void SetActive(bool state);
    //everything that shows up in scene will always have an option for enabling
    public void OnDestroy();
    //i can call for cleanup on everything
}
