using GameName1.Gnomoria.Scripts.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Gnomoria.Scripts.Tools
{
    public class ToolManager : Script
    {
        private Dictionary<string, Tool> tools = new Dictionary<string, Tool>();
        private WorldManager world;
        public string currentTool;

        private void init()
        {
            world = rootObject.getScript<WorldManager>();
        }

        public void registerTool(Tool tool)
        {
            tools.Add(tool.toolName, tool);
        }

        public Tool getCurrentTool()
        {
            Tool tool;
            if (string.IsNullOrEmpty(currentTool) || !tools.TryGetValue(currentTool, out tool))
                tool = tools["inspector"];

            return tool;
        }

        private void ongui()
        {
            var screenSize = screen.size;
            var margin = new Vector2(5, 5);
            var items = 10;
            var iconSize = new Vector2(32, 32);

            var totalSize = items * margin + items * iconSize;
            var totalSizeOver2 = totalSize/ 2f;
            var centerX = screenSize.X / 2;


            var aabb = AxisAlignedBox.FromRect(centerX - totalSizeOver2.X, screenSize.Y - margin.Y * 4 - iconSize.Y , totalSize.X, margin.Y * 2 + iconSize.Y);
            gui.box(aabb);

        }

        private void picktool(string name)
        {
            currentTool = name;
        }
    }
}
