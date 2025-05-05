using UnityEngine;

namespace Source.Components.Items
{
    public class Knife : PhysicalItem
    {
        public override int Width => 1;
        public override int Height => 2;
        public override Color Color => Color.red;
    }
}