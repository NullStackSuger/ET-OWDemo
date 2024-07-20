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
        
        [EntitySystem]
        private static void Destroy(this LSFUnitViewComponent self)
        {
            
        }

        public static async ETTask InitPlayerAsync(this LSFUnitViewComponent self, string bundlePath, string assetName, AnimatorType type)
        {
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();

            await self.Add(room.PlayerIds, bundlePath, assetName, type);
        }
        
        public static async ETTask InitPlayerAsync(this LSFUnitViewComponent self, string bundlePath, string assetName)
        {
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();

            await self.Add(room.PlayerIds, bundlePath, assetName);
        }

        public static async ETTask<LSFUnitView> Add(this LSFUnitViewComponent self, long unitId, string bundlePath, string assetName, AnimatorType type, LSUnit Owner = null)
        {
            Scene root = self.Root();
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            
            string assetsName = $"Assets/Bundles/{bundlePath}";
            GameObject bundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{assetName}");
            
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            LSUnit unit = unitComponent.GetChild<LSUnit>(unitId);
            unitGo.transform.position = unit.Position.ToVector();

            return self.AddChildWithId<LSFUnitView, AnimatorType, GameObject, LSUnit>(unit.Id, type, unitGo, Owner);
        }

        public static async ETTask Add(this LSFUnitViewComponent self, List<long> unitIds, string bundlePath, string assetName, AnimatorType type, LSUnit Owner = null)
        {
            Scene root = self.Root();
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            
            string assetsName = $"Assets/Bundles/{bundlePath}";
            GameObject bundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{assetName}");

            foreach (long id in unitIds)
            {
                GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
                LSUnit unit = unitComponent.GetChild<LSUnit>(id);
                unitGo.transform.position = unit.Position.ToVector();
                
                self.AddChildWithId<LSFUnitView, AnimatorType, GameObject, LSUnit>(unit.Id, type, unitGo, Owner);
            }
        }

        public static async ETTask<LSFUnitView> Add(this LSFUnitViewComponent self, long unitId, string bundlePath, string assetName, LSUnit Owner = null)
        {
            Scene root = self.Root();
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            
            string assetsName = $"Assets/Bundles/{bundlePath}";
            GameObject bundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{assetName}");
            
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            LSUnit unit = unitComponent.GetChild<LSUnit>(unitId);
            unitGo.transform.position = unit.Position.ToVector();

            return self.AddChildWithId<LSFUnitView, GameObject, LSUnit>(unit.Id, unitGo, Owner);
        }
        
        public static async ETTask Add(this LSFUnitViewComponent self, List<long> unitIds, string bundlePath, string assetName, LSUnit Owner = null)
        {
            Scene root = self.Root();
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            
            string assetsName = $"Assets/Bundles/{bundlePath}";
            GameObject bundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{assetName}");

            foreach (long id in unitIds)
            {
                GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
                LSUnit unit = unitComponent.GetChild<LSUnit>(id);
                unitGo.transform.position = unit.Position.ToVector();
                
                self.AddChildWithId<LSFUnitView, GameObject, LSUnit>(unit.Id, unitGo, Owner);
            }
        }
        
        public static void Remove(this LSFUnitViewComponent self, long unitId)
        {
            self.RemoveChild(unitId);
        }
    }
}