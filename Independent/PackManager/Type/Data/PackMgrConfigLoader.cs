using Momos.Tools.EditorTools;

namespace Momos.Tools.PackManager
{
    public class PackMgrConfigLoader : ConfigLoader<PackMgrConfigAsset>
    {
        public override string AssetName => "PackageManagerConfig";
    }
}