#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimeDRODPOF.TDStructure;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public static class TDELevelIO
    {
        private const string HeaderTitle = "DROD Clone Hold";
        private const int HeaderVersion = 0;

        private static readonly string[] LevelSeparator = new[] {"_**"};
        private static readonly string[] RoomDimensionSeparator = new[] {"_##"};
        private static readonly string[] RoomManagerStartSeparator = new[] {"_??"};
        private static readonly string[] RoomManagerEndSeparator = new[] {"___"};
        private static readonly string[] TileDimensionSeparator = new[] {"__)"};
        private static readonly string[] TileEntitySeparator = new[] {"__@"};
        private static readonly string[] TileSeparationSeparator = new[] {"__§"};
        private static readonly string[] EntityParametersSeparator = new[] {"__$"};
        private static readonly string[] ParameterSeparationSeparator = new[] {"__:"};
        private static readonly string[] RoomUnrequiredSeparator = new[] {"__^"};
        private static readonly string[] RoomSecretSeparator = new[] {"__&"};

        public static void SaveToFile(TDSControl mControl, string mPath)
        {
            var filePath = mPath;

            var hold = mControl.CurrentHold;

            var streamWriter = File.CreateText(filePath);

            streamWriter.WriteLine(HeaderTitle);
            streamWriter.WriteLine(HeaderVersion);

            streamWriter.WriteLine(hold.Name);

            for (var i = 0; i < hold.Levels.Count; i++)
            {
                var level = hold.Levels[i];
                streamWriter.Write(level.Name + LevelSeparator[0]);
                streamWriter.Write(level.RoomWidth + LevelSeparator[0]);
                streamWriter.Write(level.RoomHeight + LevelSeparator[0]);

                foreach (var roomKeyValuePair in level.Rooms)
                {
                    var room = roomKeyValuePair.Value;
                    streamWriter.Write(room.X + RoomDimensionSeparator[0] + room.Y + RoomManagerStartSeparator[0]);
                    var manager = room.TileManager;

                    foreach (var entity in manager.Entities)
                    {
                        streamWriter.Write(entity.Tile.X + TileDimensionSeparator[0] + entity.Tile.Y + TileEntitySeparator[0]);
                        streamWriter.Write(entity.Outline.UID.ToString());

                        foreach (var parameter in entity.Parameters)
                            streamWriter.Write(EntityParametersSeparator[0] + parameter.Name + ParameterSeparationSeparator[0] + parameter.TypeString + ParameterSeparationSeparator[0] + parameter);

                        streamWriter.Write(TileSeparationSeparator[0]);
                    }

                    if (!room.IsRequired) streamWriter.Write(RoomUnrequiredSeparator[0]);
                    if (room.IsSecret) streamWriter.Write(RoomSecretSeparator[0]);
                    streamWriter.Write(RoomManagerEndSeparator[0]);
                }

                if (i != hold.Levels.Count - 1) streamWriter.WriteLine("");
            }

            streamWriter.Flush();
            streamWriter.Close();
        }

        public static TDSControl LoadFromFile(string mPath)
        {
            var result = new TDSControl();
            var filePath = mPath;

            var streamReader = File.OpenText(filePath);

            var holdHeaderTitle = streamReader.ReadLine();
            var holdHeaderVersion = streamReader.ReadLine();

            var holdName = streamReader.ReadLine();
            var hold = TDSControl.CreateHold(result, holdName);

            var levelString = streamReader.ReadLine();

            while (!string.IsNullOrEmpty(levelString))
            {
                var levelSplit = levelString.Split(LevelSeparator, StringSplitOptions.None);
                var levelName = levelSplit[0];
                var levelRoomWidth = int.Parse(levelSplit[1]);
                var levelRoomHeight = int.Parse(levelSplit[2]);
                var levelRoomsString = levelSplit[3];

                var level = TDSControl.CreateLevel(hold, levelName, levelRoomWidth, levelRoomHeight);

                var rooms = levelRoomsString.Split(RoomManagerEndSeparator, StringSplitOptions.None);

                foreach (var roomString in rooms)
                {
                    if (string.IsNullOrEmpty(roomString)) continue;

                    var roomStringSplit = roomString.Split(new[] {RoomManagerStartSeparator[0], RoomUnrequiredSeparator[0], RoomSecretSeparator[0]}, StringSplitOptions.None);
                    var roomDimensionsString = roomStringSplit[0];
                    var roomManagerString = roomStringSplit[1];

                    var roomDimensionsSplit = roomDimensionsString.Split(RoomDimensionSeparator, StringSplitOptions.None);
                    var roomX = int.Parse(roomDimensionsSplit[0]);
                    var roomY = int.Parse(roomDimensionsSplit[1]);

                    var isRequired = roomStringSplit.Count() < 3;
                    var isSecret = roomStringSplit.Count() > 3;

                    var room = TDSControl.CreateRoom(level, roomX, roomY, isRequired, isSecret);
                    var manager = new TDETileManager(level.RoomWidth, level.RoomHeight);
                    room.TileManager = manager;

                    var roomManagerSplit = roomManagerString.Split(TileSeparationSeparator, StringSplitOptions.None);

                    foreach (var tile in roomManagerSplit)
                    {
                        if (string.IsNullOrEmpty(tile)) continue;

                        var tileStrings = tile.Split(TileEntitySeparator, StringSplitOptions.None);
                        var tilePositionString = tileStrings[0].Split(TileDimensionSeparator, StringSplitOptions.None);

                        var tileX = int.Parse(tilePositionString[0]);
                        var tileY = int.Parse(tilePositionString[1]);

                        for (var j = 1; j < tileStrings.GetLength(0); j++)
                        {
                            var entityString = tileStrings[j];

                            if (string.IsNullOrEmpty(entityString)) continue;

                            var elementSplit = entityString.Split(EntityParametersSeparator, StringSplitOptions.None);
                            var uid = int.Parse(elementSplit[0]);

                            var parameters = new List<TDEEntityParameter>();

                            for (var i = 1; i < elementSplit.Count(); i++)
                            {
                                var parameterSplit = elementSplit[i].Split(ParameterSeparationSeparator, StringSplitOptions.None);
                                var parameterName = parameterSplit[0];
                                var parameterTypeName = parameterSplit[1];

                                Type parameterType = null;
                                object parameterValue = null;

                                if (parameterTypeName == "int")
                                {
                                    parameterType = typeof (int);
                                    parameterValue = int.Parse(parameterSplit[2]);
                                }
                                else if (parameterTypeName == "bool")
                                {
                                    parameterType = typeof (bool);
                                    parameterValue = bool.Parse(parameterSplit[2]);
                                }
                                else if (parameterTypeName == "list<int>")
                                {
                                    parameterType = typeof (List<int>);

                                    var split = parameterSplit[2].Split(',');
                                    var resultValue = split.Select(int.Parse).ToList();

                                    parameterValue = resultValue;
                                }
                                else if (parameterTypeName == "string")
                                {
                                    parameterType = typeof (string);
                                    parameterValue = parameterSplit[2];
                                }

                                parameters.Add(new TDEEntityParameter(parameterName, parameterType) {Value = parameterValue});
                            }

                            var entity = new TDEEntity(TDEOutlines.GetOutlineByUID(uid)) {Parameters = new List<TDEEntityParameter>(parameters)};

                            manager.CreateEntity(tileX, tileY, entity);
                        }
                    }
                }

                levelString = streamReader.ReadLine();
            }

            streamReader.Close();

            return result;
        }
    }
}