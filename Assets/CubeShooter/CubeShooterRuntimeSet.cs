using RibynsModules.RuntimeSet;
using System.Buffers.Text;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCubeShooterRuntimeSet", menuName = CubeShooterStats.FolderName + "/CubeShooterRuntimeSet")]
public sealed class CubeShooterRuntimeSet : RuntimeSet<CubeShooter>
{
}
