using UnityEngine;
using UnityEngine.TestTools.Utils;

public abstract class BaseTests 
{
    protected const Object SKIP_FRAME = null;
    protected static FloatEqualityComparer FloatComparer = FloatEqualityComparer.Instance;
}
