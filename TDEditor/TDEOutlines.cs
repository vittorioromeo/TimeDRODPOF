using System.Collections.Generic;
using System.Linq;
using TimeDRODPOF.TDLib;

namespace TimeDRODPOF.TDEditor
{
    public static class TDEOutlines
    {
        private static readonly List<TDEOutline> Outlines = new List<TDEOutline>();

        static TDEOutlines()
        {
            Outlines = new List<TDEOutline>();

            Populate();
            AttachMethods();
        }

        private static void Populate()
        {
            Create(-1, "StartPoint", TDLLayers.Roach, @"editor\humanoid\yellow", 3);
            Create(0, "Wall", TDLLayers.Wall, @"environment\wall\purple", 0, "walltiles", "single");
            Create(1, "WallBroken", TDLLayers.Wall, @"editor\wall\broken", 0);
            Create(2, "Pit", TDLLayers.Pit, @"environment\pit\black", 0, "doortiles", "fill");
            Create(3, "Trapdoor", TDLLayers.Trapdoor, @"environment\trapdoor", 0);
            Create(25, "WaterTrapdoor", TDLLayers.Trapdoor, @"environment\watertrapdoor", 0);
            Create(4, "OrthogonalSquare", TDLLayers.Arrow, @"environment\orthogonalsquare", 1);
            Create(5, "DiagonalSquare", TDLLayers.Arrow, @"environment\diagonalsquare", 1);
            Create(6, "Roach", TDLLayers.Roach, @"monster\roach", 3);
            Create(21, "RoachQueen", TDLLayers.Roach, @"monster\roachqueen", 3);
            Create(7, "GreenDoor", TDLLayers.Door, @"environment\door\green\closed", 0, "doortiles", "single");
            Create(12, "GreenDoorOpen", TDLLayers.Door, @"environment\door\green\open", 0, "doortiles", "single");
            Create(18, "BlueDoor", TDLLayers.Door, @"environment\door\blue\closed", 0, "doortiles", "single");
            Create(19, "BlueDoorOpen", TDLLayers.Door, @"environment\door\blue\open", 0, "doortiles", "single");
            Create(13, "RedDoor", TDLLayers.Door, @"environment\door\red\closed", 0, "doortiles", "single");
            Create(14, "RedDoorOpen", TDLLayers.Door, @"environment\door\red\open", 0, "doortiles", "single");
            Create(8, "Door", TDLLayers.Door, @"environment\door\yellow\closed", 0, "doortiles", "single");
            Create(9, "DoorOpen", TDLLayers.Door, @"environment\door\yellow\open", 0, "doortiles", "single");
            Create(10, "Orb", TDLLayers.Orb, @"elements\orb", 2);
            Create(11, "Arrow", TDLLayers.Arrow, @"environment\arrow", 2);
            Create(15, "PressurePlateSingleUse", TDLLayers.Floor, @"environment\pressureplate", 0, "pressureplatetiles", "single");
            Create(16, "PressurePlateMultipleUse", TDLLayers.Floor, @"environment\pressureplate", 0, "pressureplatetiles", "multiple");
            Create(17, "PressurePlateOnOffUse", TDLLayers.Floor, @"environment\pressureplate", 0, "pressureplatetiles", "onoff");
            Create(20, "Scroll", TDLLayers.Arrow, @"elements\scroll", 1);
            Create(22, "Tunnel", TDLLayers.Arrow, @"elements\tunnel", 1);
            Create(23, "Booster", TDLLayers.Arrow, @"elements\booster", 1);
            Create(24, "Water", TDLLayers.Pit, @"environment\water", 0, "doortiles", "fill");
            Create(26, "Oremites", TDLLayers.Trapdoor, @"environment\oremites", 0, "doortiles", "fill");
            Create(27, "HotTile", TDLLayers.Trapdoor, @"environment\hottile", 0, "doortiles", "fill");
            Create(28, "Guard", TDLLayers.Roach, @"humanoid\body\red", 3);
            Create(29, "WeaponSlot", TDLLayers.Arrow, @"elements\weaponslot", 1);
            Create(30, "Brain", TDLLayers.Roach, @"monster\brain", 3);
            Create(31, "Stalwart", TDLLayers.Roach, @"humanoid\body\green", 3);
            Create(32, "StalwartRoach", TDLLayers.Roach, @"monster\roach", 3);
            Create(33, "StalwartRoachQueen", TDLLayers.Roach, @"monster\roachqueen", 3);
        }
        private static void AttachMethods()
        {
            var methods = typeof (TDLFactory).GetMethods();
            foreach (var outline in Outlines)
            {
                outline.MethodInfo = methods.First(x => x.GetCustomAttributes(typeof (EditorOutlineAttribute), true).Any(y => ((EditorOutlineAttribute) y).UID == outline.UID));
                foreach (var parameter in outline.MethodInfo.GetParameters()) outline.AddParameter(parameter, parameter.DefaultValue);
            }
        }

        public static List<TDEOutline> GetOutlines() { return Outlines; }
        public static TDEOutline GetOutlineByUID(int mUID) { return Outlines.Find(x => x.UID == mUID); }

        private static void Create(int mUID, string mName,
                                   int mLayer, string mTextureName, int mEditorGroup, string mTilesetName = null, string mLabelName = null)
        {
            Outlines.Add(new TDEOutline(mUID, mEditorGroup, mName, mLayer, mTextureName, mTilesetName, mLabelName));
        }
    }
}