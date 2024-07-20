using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace ET
{
    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    public class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }

        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

    public class ResourcesComponent : Singleton<ResourcesComponent>, ISingletonAwake
    {
        public void Awake()
        {
            YooAssets.Initialize();
        }

        protected override void Destroy()
        {
            YooAssets.Destroy();
        }

        public async ETTask CreatePackageAsync(string packageName, bool isDefault = false)
        {
            ResourcePackage package = YooAssets.CreatePackage(packageName);
            if (isDefault)
            {
                YooAssets.SetDefaultPackage(package);
            }

            GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
            EPlayMode ePlayMode = globalConfig.EPlayMode;

            // 编辑器下的模拟模式
            switch (ePlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    EditorSimulateModeParameters createParameters = new();
                    createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("ScriptableBuildPipeline", packageName);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.OfflinePlayMode:
                {
                    OfflinePlayModeParameters createParameters = new();
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    HostPlayModeParameters createParameters = new();
                    createParameters.BuildinQueryServices = new GameQueryServices();
                    createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Log.Info($"当前版本: {package.GetPackageVersion()}");
        }

        public async ETTask Update()
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            
            var op1 = package.UpdatePackageVersionAsync();
            await op1.Task;
            if (op1.Status != EOperationStatus.Succeed) Log.Error(op1.Error);
            Log.Info($"$获取最新版本 {op1.PackageVersion}");
            
            var op2 = package.UpdatePackageManifestAsync(op1.PackageVersion, true);
            await op2.Task;
            if (op2.Status != EOperationStatus.Succeed) Log.Error(op2.Error);
            Log.Info($"$对比最新版本 {op1.PackageVersion}");
            
            var op3 = package.CreateResourceDownloader(20, 3);
            if (op3.TotalDownloadCount == 0) return;
            op3.BeginDownload();
            await op3.Task;
            if (op3.Status != EOperationStatus.Succeed) Log.Error(op3.Error);
            Log.Info($"$更新到最新版本 {op1.PackageVersion}");
            
            Log.Warning($"更新完成, 当前版本{package.GetPackageVersion()}");
        }

        static string GetHostServerURL()
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            string hostServerIP = "http://192.168.0.108:8080";
            //string appVersion = "v1.0";

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            {
                return $"{hostServerIP}/CDN/Android";
                //return $"{hostServerIP}/CDN/Android/{appVersion}";
            }
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            {
                return $"{hostServerIP}/CDN/IPhone";
                //return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            }
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            {
                return $"{hostServerIP}/CDN/WebGL";
                //return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            }

            return $"{hostServerIP}/CDN/PC";
            //return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
            if (Application.platform == RuntimePlatform.Android)
            {
                return $"{hostServerIP}/CDN/Android";
                //return $"{hostServerIP}/CDN/Android/{appVersion}";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return $"{hostServerIP}/CDN/IPhone";
                //return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return $"{hostServerIP}/CDN/WebGL";
                //return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            }

            return $"{hostServerIP}/CDN/PC";
            //return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }

        public void DestroyPackage(string packageName)
        {
            ResourcePackage package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssets();
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
            await handle.Task;
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<Dictionary<string, T>> LoadAllAssetsAsync<T>(string location) where T : UnityEngine.Object
        {
            AllAssetsHandle allAssetsOperationHandle = YooAssets.LoadAllAssetsAsync<T>(location);
            await allAssetsOperationHandle.Task;
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (UnityEngine.Object assetObj in allAssetsOperationHandle.AllAssetObjects)
            {
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }

            allAssetsOperationHandle.Release();
            return dictionary;
        }
    }
}