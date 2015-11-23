using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone
{
    /// <summary>
    /// Default -> Default mode is Copy
    /// AsReference -> Will assign reference of existing object to the cloned object
    /// Ignore -> Will skip clone and reference operations ( will result in a null / non-assignation )
    /// Copy -> Will clone / copy the object
    /// </summary>
    public enum CloneMode
    {
        Default,
        AsReference,
        Ignore,
        Copy
    }
}
