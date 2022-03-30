using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

/// <summary>
/// Glowing Garden 2022
/// 
/// Creates the opposite of a circular mask that makes Mico visible in Main Menu
/// 
/// | Author: Krishna Thiruvengadam
/// </summary>
public class InverseMask : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
