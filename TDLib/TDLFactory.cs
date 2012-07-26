#region
using System.Collections.Generic;
using SFMLStart.Data;
using TimeDRODPOF.TDComponents;
using TimeDRODPOF.TDGame;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDLib
{
    public static class TDLFactory
    {
        public static TDGGame Game { get; set; }
        public static TDGInstance Instance { get; set; }
        public static Tile Tile { private get; set; }

        [EditorOutline(-1)] public static Entity StartPoint(int mDirection = 0) { return new Entity(Tile, false) {Layer = TDLLayers.Roach}; }
        [EditorOutline(00)] public static Entity Wall(bool mIsBroken = false)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Wall};

            var cRender = TDLComponentFactory.Render(@"environment\wall\purple", "walltiles", "single");
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.Break);
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.Wall, TDLRecalculations.RecalculateWallSprite);

            result.AddTags(TDLTags.Solid, TDLTags.Wall, TDLTags.HitByWeapon, TDLTags.GroundPathmapObstacle);
            result.AddComponents(cRender, cRecalculateSprites);
            if (mIsBroken)
            {
                TDLMethods.AttachBrokenOverlay(cRender);
                result.AddComponents(cHitByWeapon);
            }

            return result;
        }
        [EditorOutline(02)] public static Entity Pit()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Pit};

            var cRender = TDLComponentFactory.Render(@"environment\pit\black", "walltiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.Pit, TDLRecalculations.RecalculateDoorSprite);

            result.AddTags(TDLTags.Solid, TDLTags.Pit, TDLTags.GroundPathmapObstacle);
            result.AddComponents(cRender, cRecalculateSprites);

            return result;
        }
        [EditorOutline(03)] public static Entity Trapdoor(bool mIsOff = false, List<int> mIDs = default(List<int>), bool mIsOnWater = false, bool mIsBroken = false)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Trapdoor, UpdateOrder = TDLOrders.Trapdoor};

            var trapdoorTexture = @"environment\trapdoor";
            if (mIsOnWater) trapdoorTexture = @"environment\watertrapdoor";

            var cRender = TDLComponentFactory.Render(trapdoorTexture, "onofftiles", "on");
            var cID = TDLComponentFactory.ID(mIDs);
            var cSwitch = TDLComponentFactory.Switch(cRender, mIsOff);
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cTrapdoor = new TDCTrapdoor(mIsOnWater, mIsBroken, cSpecialSquare, cSwitch);

            if (mIsBroken) TDLMethods.AttachCrackedOverlay(cRender, 0);

            result.AddTags(TDLTags.GroundWalkable, TDLTags.Trapdoor);
            result.AddComponents(cRender, cID, cSwitch, cIDSwitchAI, cSpecialSquare, cTrapdoor);

            return result;
        }
        [EditorOutline(04)] public static Entity OrthogonalSquare(bool mIsOff = false, List<int> mIDs = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\orthogonalsquare", "onofftiles", "on");
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Arrow);
            var cID = TDLComponentFactory.ID(mIDs);
            var cSwitch = TDLComponentFactory.Switch(cRender, mIsOff);
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cOrthogonalSquare = new TDCOrthogonalSquare(cSpecialSquare, cSwitch);

            result.AddTags(TDLTags.OrthogonalSquare);
            result.AddComponents(cRender, cSpecialSquare, cID, cSwitch, cIDSwitchAI, cOrthogonalSquare);

            return result;
        }
        [EditorOutline(05)] public static Entity DiagonalSquare(bool mIsOff = false, List<int> mIDs = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\diagonalsquare", "onofftiles", "on");
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Arrow);
            var cID = TDLComponentFactory.ID(mIDs);
            var cSwitch = TDLComponentFactory.Switch(cRender, mIsOff);
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cDiagonalSquare = new TDCDiagonalSquare(cSpecialSquare, cSwitch);

            result.AddTags(TDLTags.DiagonalSquare);
            result.AddComponents(cRender, cSpecialSquare, cID, cSwitch, cIDSwitchAI, cDiagonalSquare);

            return result;
        }
        [EditorOutline(06)] public static Entity Roach(int mDirection = 0)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Roach, UpdateOrder = TDLOrders.Roach};

            var cRender = TDLComponentFactory.Render(@"monster\roach", "roachtiles", "n", true);
            var cDirection = new TDCDirection(mDirection);
            var cTarget = new TDCTarget(TDLTags.MonsterTargetTags);
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.Kill);
            var cMovement = new TDCMovement(TDCMovement.MovementType.BeelineNormal, TDLTags.GroundAllowedTags, TDLTags.GroundObstacleTags, TDLTags.GroundExceptionTags);
            var cKiller = new TDCKiller(TDLTags.MonsterTargetTags, TDLMethods.Kill);
            var cMovementTargetAI = new TDCMovementTargetAI(Instance, cMovement, cTarget, cDirection, TDLPathmaps.SideGood);
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "roachtiles");
            var cPathmapper = new TDCPathmapper(Instance, TDLPathmaps.SideBad);

            result.AddTags(TDLTags.Solid, TDLTags.SideBad, TDLTags.HitByWeapon,
                           TDLTags.RequiredKill, TDLTags.BoosterUser, TDLTags.WeightLow, TDLTags.AffectedByBrain);
            result.AddComponents(cRender, cDirection, cTarget, cHitByWeapon,
                                 cMovementTargetAI, cMovement, cKiller, cRenderDirectionAI, cPathmapper);

            return result;
        }
        [EditorOutline(21)] public static Entity RoachQueen(int mDirection = 0, int mSpawnTurns = 30)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Roach, UpdateOrder = TDLOrders.Roach};

            var cRender = TDLComponentFactory.Render(@"monster\roachqueen", "roachtiles", "n", true);
            var cDirection = new TDCDirection(mDirection);
            var cTarget = new TDCTarget(TDLTags.MonsterTargetTags);
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.Kill);
            var cMovement = new TDCMovement(TDCMovement.MovementType.BeelineNormal, TDLTags.GroundAllowedTags, TDLTags.GroundObstacleTags, TDLTags.GroundExceptionTags) {IsReverse = true};
            var cMovementTargetAI = new TDCMovementTargetAI(Instance, cMovement, cTarget, cDirection, TDLPathmaps.SideGood);
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "roachtiles");
            var cPathmapper = new TDCPathmapper(Instance, TDLPathmaps.SideBad);
            var cRoachQueen = new TDCRoachQueen(false, mSpawnTurns);

            result.AddTags(TDLTags.Solid, TDLTags.SideBad, TDLTags.HitByWeapon,
                           TDLTags.RequiredKill, TDLTags.BoosterUser, TDLTags.WeightLow, TDLTags.AffectedByBrain);
            result.AddComponents(cRender, cDirection, cTarget, cHitByWeapon,
                                 cMovementTargetAI, cMovement, cRenderDirectionAI, cPathmapper, cRoachQueen);

            return result;
        }
        [EditorOutline(10)] public static Entity Orb(List<int> mTargetIDs = default(List<int>), List<int> mTargetEffects = default(List<int>), bool mIsBroken = false, int mHealth = 2)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Orb};

            var cRender = TDLComponentFactory.Render(@"elements\orb", "orbtiles", "on");
            var cIDCaller = new TDCIDCaller(mTargetIDs, mTargetEffects);
            var cOrb = new TDCOrb(mIsBroken, mHealth, cRender, cIDCaller);
            var cHitByWeapon = new TDCHitByWeapon(cOrb.Struck);

            if (mIsBroken) TDLMethods.AttachCrackedOverlay(cRender, mHealth >= 2 ? 0 : 1);

            result.AddTags(TDLTags.Solid, TDLTags.HitByWeapon, TDLTags.GroundPathmapObstacle);
            result.AddComponents(cRender, cHitByWeapon, cIDCaller, cOrb);

            return result;
        }
        [EditorOutline(11)] public static Entity Arrow(int mDirection = 0, bool mIsOff = false, List<int> mIDs = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\arrow", "onoffdirtiles", "on_n");
            var cDirection = new TDCDirection(mDirection);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Arrow);
            var cID = TDLComponentFactory.ID(mIDs);
            var cSwitch = new TDCSwitch(cRender, mIsOff);
            cSwitch.SetOffTextureRect(Assets.Tilesets["onoffdirtiles"].GetTextureRect("off_" + cDirection.DirectionString));
            cSwitch.SetOnTextureRect(Assets.Tilesets["onoffdirtiles"].GetTextureRect("on_" + cDirection.DirectionString));
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cArrow = new TDCArrow(cSpecialSquare, cDirection, cSwitch);

            cSwitch.OnlyOnTags.Add(TDLTags.GroundPathmapObstacle);

            result.AddTags(TDLTags.Arrow);
            result.AddComponents(cRender, cDirection, cSpecialSquare, cID, cSwitch, cIDSwitchAI, cArrow);

            return result;
        }
        [EditorOutline(24)] public static Entity Water()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Pit};

            var cRender = TDLComponentFactory.Render(@"environment\water", "doortiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.Water, TDLRecalculations.RecalculateDoorSprite);

            result.AddTags(TDLTags.Solid, TDLTags.Water, TDLTags.GroundPathmapObstacle);
            result.AddComponents(cRender, cRecalculateSprites);

            return result;
        }
        [EditorOutline(30)] public static Entity Brain()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Roach, UpdateOrder = TDLOrders.Roach};

            var cRender = TDLComponentFactory.Render(@"monster\brain", true);
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.KillBrain);
            var cPathmapper = new TDCPathmapper(Instance, TDLPathmaps.SideBad);

            result.AddTags(TDLTags.Solid, TDLTags.SideBad, TDLTags.HitByWeapon,
                           TDLTags.RequiredKill, TDLTags.WeightLow, TDLTags.Brain);
            result.AddComponents(cRender, cHitByWeapon, cPathmapper);

            result.Field.TurnActions[TDLTurnActions.StartBrainCheck] += () => TDLMethods.CheckForBrains(result.Field);

            return result;
        }
        [EditorOutline(29)] public static Entity WeaponSlot(bool mIsOff = false, List<int> mIDs = default(List<int>), string mWeapon = "")
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"elements\weaponslot", "onofftiles", "on");
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cID = TDLComponentFactory.ID(mIDs);
            var cSwitch = TDLComponentFactory.Switch(cRender, mIsOff);
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cWeaponSlot = new TDCWeaponSlot(cSpecialSquare, cRender);

            cRender.AddBlankSprite();

            if (mWeapon == "reallybigsword") cWeaponSlot.Weapon = ReallyBigSword();
            else if (mWeapon == "shortsword") cWeaponSlot.Weapon = ShortSword();
            else if (mWeapon == "woodensword") cWeaponSlot.Weapon = WoodenSword();

            if (cWeaponSlot.Weapon != null) cWeaponSlot.Weapon.IsOutOfField = true;
            cWeaponSlot.RecalculateWeaponSprite();

            result.AddTags(TDLTags.WeaponSlot);
            result.AddComponents(cRender, cSpecialSquare, cID, cSwitch, cIDSwitchAI, cWeaponSlot);

            return result;
        }
        [EditorOutline(07)] public static Entity GreenDoor(bool mIsOff = false)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Door};

            var cRender = TDLComponentFactory.Render(@"environment\door\green\closed", "doortiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.DoorClosed, TDLRecalculations.RecalculateDoorSprite);
            var cSwitch = TDLComponentFactory.SwitchTexture(cRender, mIsOff, @"environment\door\green\open", @"environment\door\green\closed");
            cSwitch.OnlyOffTags.Add(TDLTags.GreenDoorOpen);
            cSwitch.OnlyOnTags.AddRange(new[] {TDLTags.GreenDoorClosed, TDLTags.Solid, TDLTags.GroundPathmapObstacle});
            var cSwitchRecalculateTagAI = new TDCSwitchRecalculateTagAI(cSwitch, cRecalculateSprites, TDLTags.GreenDoorOpen, TDLTags.GreenDoorClosed);
            var cDoor = new TDCDoor(TDCDoor.DoorType.Green, mIsOff, Game, cSwitch, cRecalculateSprites);

            result.AddTags(TDLTags.GreenDoor);
            result.AddComponents(cRender, cSwitch, cSwitchRecalculateTagAI, cRecalculateSprites, cDoor);

            return result;
        }
        [EditorOutline(18)] public static Entity BlueDoor(bool mIsOff = false)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Door};

            var cRender = TDLComponentFactory.Render(@"environment\door\blue\closed", "doortiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.DoorClosed, TDLRecalculations.RecalculateDoorSprite);
            var cSwitch = TDLComponentFactory.SwitchTexture(cRender, mIsOff, @"environment\door\blue\open", @"environment\door\blue\closed");
            cSwitch.OnlyOffTags.Add(TDLTags.BlueDoorOpen);
            cSwitch.OnlyOnTags.AddRange(new[] {TDLTags.BlueDoorClosed, TDLTags.Solid, TDLTags.GroundPathmapObstacle});
            var cSwitchRecalculateTagAI = new TDCSwitchRecalculateTagAI(cSwitch, cRecalculateSprites, TDLTags.BlueDoorOpen, TDLTags.BlueDoorClosed);
            var cDoor = new TDCDoor(TDCDoor.DoorType.Blue, mIsOff, Game, cSwitch, cRecalculateSprites);

            result.AddTags(TDLTags.BlueDoor);
            result.AddComponents(cRender, cSwitch, cSwitchRecalculateTagAI, cRecalculateSprites, cDoor);

            return result;
        }
        [EditorOutline(13)] public static Entity RedDoor(bool mIsOff = false)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Door, UpdateOrder = TDLOrders.TrapdoorDoor};

            var cRender = TDLComponentFactory.Render(@"environment\door\red\closed", "doortiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.DoorClosed, TDLRecalculations.RecalculateDoorSprite);
            var cSwitch = TDLComponentFactory.SwitchTexture(cRender, mIsOff, @"environment\door\red\open", @"environment\door\red\closed");
            cSwitch.OnlyOffTags.Add(TDLTags.RedDoorOpen);
            cSwitch.OnlyOnTags.AddRange(new[] {TDLTags.RedDoorClosed, TDLTags.Solid, TDLTags.GroundPathmapObstacle});
            var cSwitchRecalculateTagAI = new TDCSwitchRecalculateTagAI(cSwitch, cRecalculateSprites, TDLTags.RedDoorOpen, TDLTags.RedDoorClosed);
            var cDoor = new TDCDoor(TDCDoor.DoorType.Red, mIsOff, Game, cSwitch, cRecalculateSprites);

            result.AddTags(TDLTags.RedDoor);
            result.AddComponents(cRender, cSwitch, cSwitchRecalculateTagAI, cRecalculateSprites, cDoor);

            return result;
        }
        [EditorOutline(08)] public static Entity Door(bool mIsBroken = false, bool mIsOff = false, List<int> mIDs = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Door};

            var cRender = TDLComponentFactory.Render(@"environment\door\yellow\closed", "doortiles", "single");
            var cSwitch = TDLComponentFactory.SwitchTexture(cRender, mIsOff, @"environment\door\yellow\open", @"environment\door\yellow\closed");
            cSwitch.OnlyOffTags.Add(TDLTags.DoorOpen);
            cSwitch.OnlyOnTags.AddRange(new[] {TDLTags.DoorClosed, TDLTags.Solid, TDLTags.GroundPathmapObstacle});
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.BreakIfOff);
            var cID = TDLComponentFactory.ID(mIDs);
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.DoorClosed, TDLRecalculations.RecalculateDoorSprite, cID.SameIDsCondition);
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cSwitchRecalculateTagAI = new TDCSwitchRecalculateTagAI(cSwitch, cRecalculateSprites, TDLTags.DoorOpen, TDLTags.DoorClosed);
            var cDoor = new TDCDoor(TDCDoor.DoorType.Yellow, mIsOff, Game, cSwitch, cRecalculateSprites);

            result.AddTags(TDLTags.Door, TDLTags.HitByWeapon);
            result.AddComponents(cRender, cID, cSwitch, cSwitchRecalculateTagAI, cRecalculateSprites,
                                 cIDSwitchAI, cDoor);
            if (mIsBroken)
            {
                result.AddComponents(cHitByWeapon);
                TDLMethods.AttachBrokenOverlay(cRender);
            }

            return result;
        }
        [EditorOutline(15)] public static Entity PressurePlateSingleUse(List<int> mTargetIDs = default(List<int>), List<int> mTargetEffects = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Floor, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\pressureplate", "pressureplatetiles", "single");
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cIDCaller = new TDCIDCaller(mTargetIDs, mTargetEffects);
            var cFloodFiller = new TDCFloodFiller(TDLTags.PressurePlateSingleUse);
            var cPressurePlate = new TDCPressurePlate(TDCPressurePlate.PressurePlateType.Single, cRender, cFloodFiller, cIDCaller, cSpecialSquare, "triggered_single", "single");

            result.AddTags(TDLTags.PressurePlateSingleUse);
            result.AddComponents(cRender, cPressurePlate, cSpecialSquare, cIDCaller, cFloodFiller);

            return result;
        }
        [EditorOutline(16)] public static Entity PressurePlateMultipleUse(List<int> mTargetIDs = default(List<int>), List<int> mTargetEffects = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Floor, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\pressureplate", "pressureplatetiles", "multiple");
            var cIDCaller = new TDCIDCaller(mTargetIDs, mTargetEffects);
            var cFloodFiller = new TDCFloodFiller(TDLTags.PressurePlateMultipleUse);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cPressurePlate = new TDCPressurePlate(TDCPressurePlate.PressurePlateType.Multiple, cRender, cFloodFiller, cIDCaller, cSpecialSquare, "triggered_multiple", "multiple");

            result.AddTags(TDLTags.PressurePlateMultipleUse);
            result.AddComponents(cRender, cSpecialSquare, cPressurePlate, cIDCaller, cFloodFiller);

            return result;
        }
        [EditorOutline(17)] public static Entity PressurePlateOnOffUse(List<int> mTargetIDs = default(List<int>), List<int> mTargetEffects = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Floor, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\pressureplate", "pressureplatetiles", "onoff");
            var cIDCaller = new TDCIDCaller(mTargetIDs, mTargetEffects);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cFloodFiller = new TDCFloodFiller(TDLTags.PressurePlateOnOffUse);
            var cPressurePlate = new TDCPressurePlate(TDCPressurePlate.PressurePlateType.OnOff, cRender, cFloodFiller, cIDCaller, cSpecialSquare, "triggered_onoff", "onoff");

            result.AddTags(TDLTags.PressurePlateOnOffUse);
            result.AddComponents(cRender, cSpecialSquare, cPressurePlate, cIDCaller, cFloodFiller);

            return result;
        }
        [EditorOutline(20)] public static Entity Scroll(string mText = "")
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow, UpdateOrder = TDLOrders.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"elements\scroll");
            var cScroll = new TDCScroll(Game, mText);

            result.AddComponents(cRender, cScroll);

            return result;
        }
        [EditorOutline(28)] public static Entity Guard(int mDirection = 0)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Roach, UpdateOrder = TDLOrders.Guard};

            var outline1 = new TDCRenderSpriteOutline("humanoidtiles", @"humanoid\body\red", "n");
            var outline2 = new TDCRenderSpriteOutline("humanoidtiles", @"humanoid\hands\pink", "n");
            var outline3 = new TDCRenderSpriteOutline("humanoidtiles", @"humanoid\head\pink", "n");

            var cRender = new TDCRender(outline1, outline2, outline3) {IsLerped = true};
            var cDirection = new TDCDirection(mDirection);
            var cTarget = new TDCTarget(TDLTags.MonsterTargetTags);
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.Kill);
            var cMovement = new TDCMovement(TDCMovement.MovementType.FlexibleNormal, TDLTags.GroundAllowedTags, TDLTags.GroundObstacleTags, TDLTags.GroundExceptionTags);
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "humanoidtiles");
            var cWielder = new TDCWielder(cMovement, cDirection);
            var cWielderTargetAI = new TDCWielderTargetAI(cTarget, cMovement, cDirection, cWielder);
            var cMovementTargetAI = new TDCMovementTargetAI(Instance, cMovement, cTarget, cDirection, TDLPathmaps.SideGood, false, true);
            var cPathmapper = new TDCPathmapper(Instance, TDLPathmaps.SideBad);
            var cWielderRenderAI = new TDCWielderRenderAI(cWielder, cRender, cRenderDirectionAI);


            result.AddTags(TDLTags.Solid, TDLTags.Humanoid, TDLTags.SideBad, TDLTags.HitByWeapon,
                           TDLTags.WeightLow, TDLTags.BoosterUser, TDLTags.RequiredKill, TDLTags.Guard);
            result.AddComponents(cRender, cDirection, cTarget, cHitByWeapon,
                                 cWielderTargetAI, cMovementTargetAI, cMovement, cRenderDirectionAI, cWielder, cPathmapper,
                                 cWielderRenderAI);

            cWielder.SetWeapon(WoodenSword());

            return result;
        }
        [EditorOutline(26)] public static Entity Oremites()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\oremites", "doortiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.Oremites, TDLRecalculations.RecalculateDoorSprite);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Oremites);
            var cOremites = new TDCOremites(cSpecialSquare);

            result.AddTags(TDLTags.Oremites);
            result.AddComponents(cRender, cSpecialSquare, cOremites, cRecalculateSprites);

            return result;
        }
        [EditorOutline(27)] public static Entity HotTile(int mTurnsToBurn = 2)
        {
            var result = new Entity(Tile) {UpdateOrder = TDLOrders.Trapdoor, Layer = TDLLayers.Trapdoor};

            var cRender = TDLComponentFactory.Render(@"environment\hottile", "doortiles", "single");
            var cRecalculateSprites = new TDCRecalculateSprites(TDLTags.HotTile, TDLRecalculations.RecalculateDoorSprite);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cHotTile = new TDCHotTile(cSpecialSquare, mTurnsToBurn);

            result.AddTags(TDLTags.HotTile);
            result.AddComponents(cRender, cSpecialSquare, cRecalculateSprites, cHotTile);

            return result;
        }
        [EditorOutline(22)] public static Entity Tunnel(int mDirection = 0)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow};

            var cRender = TDLComponentFactory.Render(@"elements\tunnel");
            var cDirection = new TDCDirection(mDirection);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cTunnel = new TDCTunnel(cRender, cDirection, cSpecialSquare);

            result.AddTags(TDLTags.Tunnel, TDLTags.GroundPathmapObstacle);
            result.AddComponents(cRender, cSpecialSquare, cDirection, cTunnel);

            return result;
        }
        [EditorOutline(23)] public static Entity Booster(int mDirection = 0, bool mIsOff = false, List<int> mIDs = default(List<int>))
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Arrow};

            var cRender = TDLComponentFactory.Render(@"elements\booster", "onoffdirtiles", "on_n");
            var cDirection = new TDCDirection(mDirection);
            var cSpecialSquare = new TDCSpecialSquare(TDLPriorities.Trapdoor);
            var cID = TDLComponentFactory.ID(mIDs);
            var cSwitch = new TDCSwitch(cRender, mIsOff);
            cSwitch.SetOffTextureRect(Assets.Tilesets["onoffdirtiles"].GetTextureRect("off_" + cDirection.DirectionString));
            cSwitch.SetOnTextureRect(Assets.Tilesets["onoffdirtiles"].GetTextureRect("on_" + cDirection.DirectionString));
            var cIDSwitchAI = new TDCIDSwitchAI(cSwitch, cID);
            var cBooster = new TDCBooster(cDirection, cSwitch, cSpecialSquare);

            result.AddTags(TDLTags.Booster);
            result.AddComponents(cRender, cSpecialSquare, cDirection, cID, cSwitch, cIDSwitchAI, cBooster);

            return result;
        }

        // DERIVED ENTITIES
        [EditorOutline(31)] public static Entity Stalwart(int mDirection = 0)
        {
            var result = Guard(mDirection);
            result.UpdateOrder = TDLOrders.Stalwart;
            result.GetComponent<TDCRender>().GetSprite(0).Texture = Assets.GetTexture(@"humanoid\body\green");
            result.GetComponent<TDCTarget>().TargetTags = TDLTags.StalwartTargetTags;
            result.GetComponent<TDCMovementTargetAI>().PathmapName = TDLPathmaps.SideBad;
            result.GetComponent<TDCPathmapper>().ChangePathmap(TDLPathmaps.SideGood);
            result.RemoveTags(TDLTags.SideBad, TDLTags.Guard, TDLTags.RequiredKill);
            result.AddTags(TDLTags.SideGood, TDLTags.Stalwart);
            result.GetComponent<TDCWielder>().SetWeapon(ShortSword());
            return result;
        }
        [EditorOutline(32)] public static Entity StalwartRoach(int mDirection = 0)
        {
            var result = Roach(mDirection);
            result.UpdateOrder = TDLOrders.Stalwart;
            result.GetComponent<TDCRender>().AddSprite(Assets.Tilesets["roachtiles"].GetSprite("n", Assets.GetTexture(@"humanoid\cape\green")));
            result.GetComponent<TDCTarget>().TargetTags = TDLTags.StalwartTargetTags;
            result.GetComponent<TDCKiller>().TargetTags = TDLTags.StalwartTargetTags;
            result.GetComponent<TDCMovementTargetAI>().PathmapName = TDLPathmaps.SideBad;
            result.GetComponent<TDCPathmapper>().ChangePathmap(TDLPathmaps.SideGood);
            result.RemoveTags(TDLTags.SideBad, TDLTags.RequiredKill);
            result.AddTags(TDLTags.SideGood);
            return result;
        }
        [EditorOutline(33)] public static Entity StalwartRoachQueen(int mDirection = 0, int mSpawnTurns = 30)
        {
            var result = RoachQueen(mDirection, mSpawnTurns);
            result.UpdateOrder = TDLOrders.Stalwart;
            result.GetComponent<TDCRender>().AddSprite(Assets.Tilesets["roachtiles"].GetSprite("n", Assets.GetTexture(@"humanoid\cape\green")));
            result.GetComponent<TDCTarget>().TargetTags = TDLTags.StalwartTargetTags;
            result.GetComponent<TDCMovementTargetAI>().PathmapName = TDLPathmaps.SideBad;
            result.GetComponent<TDCPathmapper>().ChangePathmap(TDLPathmaps.SideGood);
            result.GetComponent<TDCRoachQueen>().IsStalwart = true;

            result.RemoveTags(TDLTags.SideBad, TDLTags.RequiredKill);
            result.AddTags(TDLTags.SideGood);
            return result;
        }
        [EditorOutline(12)] public static Entity GreenDoorOpen() { return GreenDoor(true); }
        [EditorOutline(19)] public static Entity BlueDoorOpen() { return BlueDoor(true); }
        [EditorOutline(14)] public static Entity RedDoorOpen() { return RedDoor(true); }
        [EditorOutline(01)] public static Entity WallBroken() { return Wall(true); }
        [EditorOutline(09)] public static Entity DoorOpen(bool mIsBroken = false, List<int> mIDs = default(List<int>)) { return Door(mIsBroken, true, mIDs); }
        [EditorOutline(25)] public static Entity WaterTrapdoor(bool mIsOff = false, List<int> mIDs = default(List<int>), bool mIsBroken = false) { return Trapdoor(mIsOff, mIDs, true, mIsBroken); }
        public static Entity StalwartRoachEgg()
        {
            var result = RoachEgg();
            result.GetComponent<TDCEgg>().Friendly = true;
            result.GetComponent<TDCPathmapper>().ChangePathmap(TDLPathmaps.SideGood);
            result.RemoveTags(TDLTags.SideBad, TDLTags.RequiredKill);
            result.AddTags(TDLTags.SideGood);
            return result;
        }

        // NON-EDITOR ENTITIES
        public static Entity Floor()
        {
            var result = new Entity(Tile, false) {Layer = TDLLayers.Floor};
            result.AddTags(TDLTags.GroundWalkable);
            return result;
        }
        public static Entity Player(int mDirection = 0)
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Roach, UpdateOrder = TDLOrders.Player};

            var outline1 = new TDCRenderSpriteOutline("humanoidtiles", @"humanoid\body\yellow", "n");
            var outline2 = new TDCRenderSpriteOutline("humanoidtiles", @"humanoid\hands\pink", "n");
            var outline3 = new TDCRenderSpriteOutline("humanoidtiles", @"humanoid\head\pink", "n");

            var cRender = new TDCRender(outline1, outline2, outline3) {IsLerped = true};
            var cDirection = new TDCDirection(mDirection);
            var cTarget = new TDCTarget();
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.Kill);
            var cMovement = new TDCMovement(TDCMovement.MovementType.Direct, TDLTags.GroundAllowedTags, TDLTags.GroundObstacleTags, TDLTags.GroundExceptionTags);
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "humanoidtiles");
            var cWielder = new TDCWielder(cMovement, cDirection);
            var cPathmapper = new TDCPathmapper(Instance, TDLPathmaps.SideGood);
            var cPlayer = new TDCPlayer(Game, cMovement, cDirection, cWielder);
            var cRoomSwitch = new TDCRoomSwitch(Game, cMovement);
            var cWielderRenderAI = new TDCWielderRenderAI(cWielder, cRender, cRenderDirectionAI);

            result.AddTags(TDLTags.Solid, TDLTags.Humanoid, TDLTags.SideGood, TDLTags.HitByWeapon,
                           TDLTags.Player, TDLTags.TunnelUser, TDLTags.WeightLow, TDLTags.BoosterUser);
            result.AddComponents(cRender, cDirection, cTarget, cHitByWeapon, cPlayer, cMovement, cRenderDirectionAI,
                                 cWielder, cPathmapper, cRoomSwitch, cWielderRenderAI);

            cWielder.SetWeapon(ReallyBigSword());

            /*cMovement.OnMovementFail += () =>
                                        {
                                            // This is only to prevent wall bump sounds if using a tunnel or a booster
                                            var x = cMovement.TargetX;
                                            var y = cMovement.TargetY;

                                            if (result.Field.HasEntityByTag(result.X, result.Y, TDLTags.Tunnel)) return;
                                            if (result.Field.HasEntityByTag(result.X, result.Y, TDLTags.Booster)) return;

                                            if (result.Field.HasEntityByTag(x, y, TDLTags.Wall)) TDLSounds.Play("SoundWallBump");
                                            else if (result.Field.HasEntityByTag(x, y, TDLTags.Pit)) TDLSounds.Play("SoundPitBump");
                                        };*/

            return result;
        }
        public static Entity RoachEgg()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Roach, UpdateOrder = TDLOrders.Roach};

            var cRender = TDLComponentFactory.Render(@"monster\roachegg", "roachtiles", "n", true);
            var cHitByWeapon = new TDCHitByWeapon(TDCHitByWeapon.HitActions.Kill);
            var cEgg = new TDCEgg(cRender, 4);
            var cPathmapper = new TDCPathmapper(Instance, TDLPathmaps.SideBad);

            result.AddTags(TDLTags.Solid, TDLTags.SideBad, TDLTags.HitByWeapon, TDLTags.RequiredKill);
            result.AddComponents(cRender, cHitByWeapon, cEgg, cPathmapper);

            return result;
        }

        // WEAPONS
        public static Entity ReallyBigSword()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Weapon};

            var cRender = TDLComponentFactory.Render(@"weapon\reallybigsword", "dirtiles", "n", true);
            var cDirection = new TDCDirection(0);
            var cWeapon = new TDCWeapon();
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "dirtiles");

            cWeapon.OnEquip += mWielder => mWielder.Entity.AddTags(TDLTags.WeightHigh);
            cWeapon.OnUnEquip += mWielder => mWielder.Entity.RemoveTags(TDLTags.WeightHigh);

            result.AddTags(TDLTags.Solid, TDLTags.Weapon, TDLTags.DoesNotTriggerPlate, TDLTags.AffectedByOremites);
            result.AddComponents(cRender, cDirection, cWeapon, cRenderDirectionAI);

            Instance.AddEntity(result);
            return result;
        }
        public static Entity ShortSword()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Weapon};

            var cRender = TDLComponentFactory.Render(@"weapon\sword", "dirtiles", "n", true);
            var cDirection = new TDCDirection(0);
            var cWeapon = new TDCWeapon();
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "dirtiles");

            result.AddTags(TDLTags.Solid, TDLTags.Weapon, TDLTags.DoesNotTriggerPlate, TDLTags.AffectedByOremites);
            result.AddComponents(cRender, cDirection, cWeapon, cRenderDirectionAI);

            Instance.AddEntity(result);
            return result;
        }
        public static Entity WoodenSword()
        {
            var result = new Entity(Tile) {Layer = TDLLayers.Weapon};

            var cRender = TDLComponentFactory.Render(@"weapon\woodensword", "dirtiles", "n", true);
            var cDirection = new TDCDirection(0);
            var cWeapon = new TDCWeapon();
            var cRenderDirectionAI = new TDCRenderDirectionAI(cRender, cDirection, "dirtiles");

            result.AddTags(TDLTags.Solid, TDLTags.Weapon, TDLTags.DoesNotTriggerPlate);
            result.AddComponents(cRender, cDirection, cWeapon, cRenderDirectionAI);

            Instance.AddEntity(result);
            return result;
        }
    }
}