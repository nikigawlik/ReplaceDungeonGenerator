using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	public interface IRoomGenerator {
        void Generate(bool posXOpen, bool posYOpen, bool posZOpen, bool negXOpen, bool negYOpen, bool negZOpen);
	}
}