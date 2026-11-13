using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
//modify as needed
public enum Layer
{
    Default = 0,
    Player = 1,
    Ground = 2,
    Obstacle = 3,
    Enemy = 4,
    Interactable = 5,
    Exit = 6,

    PlayerAtk =7,
    EnemyAtk = 8
    //does ui need a layermask? its already drawn seperately and gum handles collisions
    //cannot go above 31
}
//bitmasks
// 1 << i sets the ith bit to 1, w zero indexing
// | is or, which combines bits
// & is and which compares bits
