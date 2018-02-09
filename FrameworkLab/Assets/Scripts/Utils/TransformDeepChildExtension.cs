using UnityEngine;

namespace Framework.Util
{
    /// <summary>
    /// Allow to make a research from a Transform into the GrandChildren.
    /// Usefull as the Transform.Find method only work for the children, and not the GrandChildren.
    /// </summary>
    public static class TransformDeepChildExtension
    {
        /// <summary>
        /// Breadth-first search
        /// </summary>
        /// <param name="aParent">The parent of the transform to find</param>
        /// <param name="aName">The name of the Transform to find</param>
        /// <returns></returns>
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}