namespace TimeDRODPOF.TDLib
{
    public static class TDLTags
    {
        public const string Solid = "solid";
        public const string GroundWalkable = "groundwalkable";
        public const string SideGood = "sidegood";
        public const string SideBad = "sidebad";
        public const string Wall = "wall";
        public const string Door = "door";
        public const string DoorOpen = "dooropen";
        public const string DoorClosed = "doorclosed";
        public const string Humanoid = "humanoid";
        public const string Trapdoor = "trapdoor";
        public const string WeightHigh = "dropstrapdoor";
        public const string Weapon = "weapon";
        public const string HitByWeapon = "hitbyweapon";
        public const string Pit = "pit";
        public const string Arrow = "arrow";
        public const string OrthogonalSquare = "orthogonalsquare";
        public const string DiagonalSquare = "diagonalsquare";
        public const string RequiredKill = "requiredkill";
        public const string GreenDoor = "greendoor";
        public const string GreenDoorOpen = "greendooropen";
        public const string GreenDoorClosed = "greendoorclosed";
        public const string RedDoor = "reddoor";
        public const string RedDoorOpen = "reddooropen";
        public const string RedDoorClosed = "reddoorclosed";
        public const string BlueDoor = "bluedoor";
        public const string BlueDoorOpen = "bluedooropen";
        public const string BlueDoorClosed = "bluedoorclosed";
        public const string PressurePlateSingleUse = "pressureplatesingle";
        public const string DoesNotTriggerPlate = "doesnottriggerplate";
        public const string PressurePlateMultipleUse = "pressureplatemultiple";
        public const string PressurePlateOnOffUse = "pressureplateonoff";
        public const string Player = "playertag";
        public const string PreventSpawn = "preventspawn";
        public const string Tunnel = "tunnel";
        public const string TunnelUser = "tunneluser";
        public const string BoosterUser = "boosteruser";
        public const string Booster = "booster";
        public const string Water = "water";
        public const string WeightLow = "dropsbrokentrapdoor";
        public const string Oremites = "oremites";
        public const string HotTile = "hottile";
        public const string NotAffectedByHeat = "notaffectedbyheat";
        public const string AffectedByOremites = "affectedbyoremites";
        public const string WeaponSlot = "weaponslot";
        public const string Brain = "brain";
        public const string AffectedByBrain = "affectedbybrain";
        public const string Guard = "guard";
        public const string GroundPathmapObstacle = "groundpathmapobstacle";
        public const string Stalwart = "stalwart";

        public static readonly string[] GroundObstacleTags = new[] {Solid};
        public static readonly string[] GroundAllowedTags = new[] {GroundWalkable};
        public static readonly string[] GroundExceptionTags = new[] {""};
        public static readonly string[] MonsterTargetTags = new[] {SideGood};
        public static readonly string[] StalwartTargetTags = new[] {SideBad};
    }
}