using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.SRPG.Scripts
{
    public class TrackObject : Script
    {
        public string objectName = null;
        public AxisAlignedBox keepInBounds = AxisAlignedBox.Null;

        private void afterupdate()
        {
            var root = Root.instance;
            var go = root.RootObject.find(objectName);
            if (go != null)
            {
                if (keepInBounds.IsNull)
                {
                    this.gameObject.transform.DerivedPosition = go.transform.DerivedPosition;
                }
                else
                {
                    var p = go.transform.DerivedPosition;
                    var np = gameObject.transform.DerivedPosition;

                    keepInBounds.Center = gameObject.transform.DerivedPosition;
                    if (p.X < keepInBounds.X0)
                        np.X += p.X - keepInBounds.X0;
                    if (p.X > keepInBounds.X1)
                        np.X += p.X - keepInBounds.X1;
                    if (p.Y < keepInBounds.Y0)
                        np.Y += p.Y - keepInBounds.Y0;
                    if (p.Y > keepInBounds.Y1)
                        np.Y += p.Y - keepInBounds.Y1;

                    gameObject.transform.DerivedPosition = np;
                }
            }
        }


    }
}
