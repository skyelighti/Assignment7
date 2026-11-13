
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class DescriptionProperty: IItemProperty
{
    //#thinking... i want items to also be able to display stuff in dialouge boxes, like if you interact with an item
    //summary could just be like a short blurb if like u hover an item in inv
    public List<string> dialogueIDOrder { get; private set; }
    public string summary { get; private set; }

    public DescriptionProperty(List<string> ids = null, string summary = null)
    {
        dialogueIDOrder = ids ?? new List<string>();
        this.summary = summary;
    }
}
