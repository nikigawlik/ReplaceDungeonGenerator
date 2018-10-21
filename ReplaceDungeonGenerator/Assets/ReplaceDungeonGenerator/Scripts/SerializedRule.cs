using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/*
	Class that is more readable when serialized as JSON
 */
namespace ReplaceDungeonGenerator
{
    [System.Serializable]
    public class SerializedRule : ISerializationCallbackReceiver
    {
        private Rule rule;

        [SerializeField] private string match;
        [SerializeField] private string replacement;
        [SerializeField] private float weight;
        [SerializeField] private string shortDescription;

        public Rule Rule
        {
            get
            {
                return rule;
            }

            set
            {
                rule = value;
            }
        }

        public SerializedRule(Rule r)
        {
            Rule = r;
        }



        public static string PatternToString(Pattern p)
        {
            Vector3Int size = p.Size;
            string str = "";

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        str += p.GetTile(new Vector3Int(x, y, z)).Label;
                        if (z != size.z - 1)
                        {
                            str += " ";
                        }
                    }
                    if (y != size.y - 1)
                    {
                        str += ",";
                    }
                }
                if (x != size.x - 1)
                {
                    str += ";";
                }
            }

            return str;
        }

        public static Pattern StringToPattern(string str)
        {
            Vector3Int size = Vector3Int.zero;
            str = Regex.Replace(str, @"\n", "");

            string[] xLayers = str.Split(';');
            size.x = xLayers.Length;
            List<List<List<string>>> ids = new List<List<List<string>>>();
            for (int x = 0; x < xLayers.Length; x++)
            {
                string[] yRows = xLayers[x].Split(',');
                size.y = yRows.Length;
                List<List<string>> layer = new List<List<string>>();
                for (int y = 0; y < yRows.Length; y++)
                {
                    string[] zCells = yRows[y].Split(' ');
                    size.z = zCells.Length;
                    List<string> row = new List<string>();
                    for (int z = 0; z < zCells.Length; z++)
                    {
                        row.Add(zCells[z]);
                    }
                    layer.Add(row);
                }
                ids.Add(layer);
            }

            Tile[,,] tiles = new Tile[size.x, size.y, size.z];

            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    for (int z = 0; z < size.z; z++)
                    {
                        tiles[x, y, z] = new Tile(ids[x][y][z]);
                    }

            return new Pattern(tiles);
        }

        public void OnBeforeSerialize()
        {
            match = PatternToString(Rule.structure);
            replacement = PatternToString(Rule.replacement);
            weight = Rule.weight;
            shortDescription = Rule.shortDescription;
        }

        public void OnAfterDeserialize()
        {
            Rule = new Rule(StringToPattern(match), StringToPattern(replacement), weight, shortDescription);
        }
    }
}
