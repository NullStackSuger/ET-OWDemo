namespace ET.Server
{
    public static class LSFUnitHelper
    {
        public static LSUnit Creat(this LSUnitComponent self, LockStepUnitInfo playerInfo, string tag)
        {
            LSUnit unit = self.Creat(playerInfo.PlayerId);

            unit.Position = playerInfo.Position;
            unit.Rotation = playerInfo.Rotation;
            unit.Tag = tag;
            /*unit.AddComponent<AOIEntity, float, float, int>(
                (float)unit.Position.x / AOIManagerComponent.CellSize, 
                (float)unit.Position.z / AOIManagerComponent.CellSize, 1);*/
            unit.AddComponent<ActionComponent, int>(playerInfo.ActionGroup);
            unit.AddComponent<BuffComponent>();
            unit.AddComponent<CastComponent>();
            unit.AddComponent<LSFInputComponent>();
            unit.AddComponent<B3CollisionComponent, int>(playerInfo.RigidBodyId);
            unit.AddComponent<CheckOnGroundComponent>();
            DataModifierComponent dataModifierComponent = unit.AddComponent<DataModifierComponent>();

            dataModifierComponent.Add(new Default_Speed_ConstantModifier() { Value = 15 });

            dataModifierComponent.Add(new Default_Hp_FinalMaxModifier() { Value = 100 });
            dataModifierComponent.Add(new Default_Hp_FinalMinModifier() { Value = 0 });

            dataModifierComponent.Add(new Default_Hp_ConstantMaxModifier() { Value = 50 });
            dataModifierComponent.Add(new Default_Hp_ConstantMinModifier() { Value = 0 });
            dataModifierComponent.Add(new Default_Hp_ConstantModifier() { Value = 50 });

            dataModifierComponent.Add(new Default_Hp_FinalConstantMaxModifier() { Value = 50 });
            dataModifierComponent.Add(new Default_Hp_FinalConstantMinModifier() { Value = 0 });
            dataModifierComponent.Add(new Default_Hp_FinalConstantModifier() { Value = 50 });

            dataModifierComponent.Add(new Default_Atk_ConstantModifier() { Value = 5 });

            dataModifierComponent.Add(new Default_BulletCount_FinalMaxModifier() { Value = 150 });
            dataModifierComponent.Add(new Default_BulletCount_FinalMinModifier() { Value = 0 });
            dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = 150 });

            dataModifierComponent.Add(new Default_Shield_FinalMaxModifier() { Value = 200 });
            dataModifierComponent.Add(new Default_Shield_FinalMinModifier() { Value = 0 });
            dataModifierComponent.Add(new Default_Shield_ConstantModifier() { Value = 200 });

            dataModifierComponent.Add(new Default_ENumeric_ConstantModifier() { Value = 30 });
            dataModifierComponent.Add(new Default_ECD_ConstantModifier() { Value = 1000 });

            dataModifierComponent.Add(new Default_QNumeric_ConstantModifier() { Value = 30 });
            dataModifierComponent.Add(new Default_QCD_ConstantModifier() { Value = 1000 });

            dataModifierComponent.Add(new Default_CNumeric_ConstantModifier() { Value = 30 });
            dataModifierComponent.Add(new Default_CCD_ConstantModifier() { Value = 1000 });

            dataModifierComponent.Publish(DataModifierType.Speed);
            //dataModifierComponent.Publish(DataModifierType.Hp);
            //dataModifierComponent.Publish(DataModifierType.BulletCount);

            return unit;
        }
    }
}