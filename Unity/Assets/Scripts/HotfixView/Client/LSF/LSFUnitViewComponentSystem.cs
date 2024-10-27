using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFUnitViewComponent))]
    public static partial class LSFUnitViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUnitViewComponent self)
        {
            
        }
        
        public static async ETTask InitPlayerAsync(this LSFUnitViewComponent self, string bundlePath, string assetName)
        {
            Room room = self.GetParent<Room>();

            await self.Add(room.PlayerIds, bundlePath, assetName);
        }
        
        public static async ETTask<LSFUnitView> Add(this LSFUnitViewComponent self, long unitId, string bundlePath, string assetName)
        {
            Scene root = self.Root();
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            Room room = self.GetParent<Room>();
            LSUnitComponent unitComponent = room.PredictionWorld.GetComponent<LSUnitComponent>();
            
            string assetsName = $"Assets/Bundles/{bundlePath}";
            GameObject bundleGameObject = await room.GetParent<Scene>().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{assetName}");
            
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            unitGo.name = assetName;
            LSUnit unit = unitComponent.GetChild<LSUnit>(unitId);
            unitGo.transform.position = unit.Position.ToVector();

            return self.AddChildWithId<LSFUnitView, GameObject, LSUnit>(unit.Id, unitGo, unit);
        }
        
        public static async ETTask Add(this LSFUnitViewComponent self, List<long> unitIds, string bundlePath, string assetName)
        {
            Scene root = self.Root();
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            Room room = self.GetParent<Room>();
            LSUnitComponent unitComponent = room.PredictionWorld.GetComponent<LSUnitComponent>();
            
            string assetsName = $"Assets/Bundles/{bundlePath}";
            GameObject bundleGameObject = await room.GetParent<Scene>().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{assetName}");

            foreach (long id in unitIds)
            {
                GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
                unitGo.name = assetName;
                LSUnit unit = unitComponent.GetChild<LSUnit>(id);
                unitGo.transform.position = unit.Position.ToVector();
                
                self.AddChildWithId<LSFUnitView, GameObject, LSUnit>(unit.Id, unitGo, unit);
            }
        }
        
        public static void Remove(this LSFUnitViewComponent self, long unitId)
        {
            self.RemoveChild(unitId);
        }
    }
}