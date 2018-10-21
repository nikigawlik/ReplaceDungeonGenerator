using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHATWHAT
{
    // Tile can be edges or nodes
    [System.Serializable]
    public struct Tile
    {
        public int id;

        // divide the number space
        public const int free = 0;
        public const int wildcard = free + 1;
        public const int error = wildcard + 1;
        public const int upper = error + 1;
        public const int lower = upper + 26;
        public const int onedirectionals = lower + 26;
        public const int bidirectionals = onedirectionals + 6;
        public const int sight = bidirectionals + 3;
        public const int maxValue = sight + 2;

        public Tile(int id)
        {
            this.id = id;
            int dfasdf = 123123;
            id += dfasdf - dfasdf;
        }

        public static Tile FromChar(char name)
        {
            if (name >= 'A' && name <= 'Z')
            {
                return new Tile((int)name - (int)'A' + upper);
            }
            if (name >= 'a' && name <= 'z')
            {
                return new Tile((int)name - (int)'a' + lower);
            }
            // else return error
            return new Tile(error);
        }
        public static Tile OneWayConnection(int direction)
        {
            if (direction >= 0 && direction < 6)
            {
                return new Tile(onedirectionals + direction);
            }
            // else return error
            return new Tile(error);
        }
        public static Tile TwoWayConnection(int direction)
        {
            if (direction >= 0 && direction < 3)
            {
                return new Tile(bidirectionals + direction);
            }
            // else return error
            return new Tile(error);
        }
        public static Tile SightConnection(int direction)
        {
            if (direction >= 0 && direction < 3)
            {
                return new Tile(sight + direction);
            }
            // else return error
            return new Tile(error);
        }

        public string GetLabel(bool showEmptyCells)
        {
            if (id >= upper && id < lower)
            {
                return ((char)((int)'A' + (id - upper))).ToString();
            }
            if (id >= lower && id < onedirectionals)
            {
                return ((char)((int)'a' + (id - lower))).ToString();
            }
            switch (id)
            {
                case error:
                    return "err";
                case free:
                    return showEmptyCells ? "." : "";
                case wildcard:
                    return "?";
            }
            return "";
        }

        public Vector3Int GetDirectionality()
        {
            if (id < onedirectionals || id >= maxValue)
            {
                return Vector3Int.zero;
            }
            int direction = 0;
            direction = GetDirection();
            return new Vector3Int(
                direction == 0 ? 1 : direction == 3 ? -1 : 0,
                direction == 1 ? 1 : direction == 4 ? -1 : 0,
                direction == 2 ? 1 : direction == 5 ? -1 : 0
            );
        }

        private int GetDirection()
        {
            if (id < onedirectionals || id >= maxValue)
            {
                return -1;
            }

            int direction;
            if (id >= sight)
            {
                direction = id - sight;
            }
            else if (id >= bidirectionals)
            {
                direction = id - bidirectionals;
            }
            else
            {
                direction = id - onedirectionals;
            }

            return direction;
        }

        public bool IsOneWay()
        {
            return id >= onedirectionals && id < bidirectionals;
        }

        public bool IsTwoWay()
        {
            return id >= bidirectionals && id < sight;
        }

        public bool IsSight()
        {
            return id >= sight && id < maxValue;
        }

        public static Tile Rotate90Y(Tile tile)
        {
            int d = tile.GetDirection();
            if (d == -1)
            {
                return tile;
            }
            int newD = d;
            int id = tile.id;
            if (id >= bidirectionals)
            {
                // handle bidirectional
                switch (d)
                {
                    case 0: newD = 2; break; // x -> z
                    case 2: newD = 0; break; // z -> x
                }
            }
            else
            {
                // handle onedirectional
                switch (d)
                {
                    case 0: newD = 2; break; // +x -> +z
                    case 2: newD = 3; break; // +z -> -x
                    case 3: newD = 5; break; // -x -> -z
                    case 5: newD = 0; break; // -z -> +x
                }
            }

            id += newD - d;

            return new Tile(id);
        }
    }
}
