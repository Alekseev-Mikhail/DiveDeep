namespace Source.Utilities.Inventory.Views
{
    public struct ColoredInventoryMarkup
    {
        public ColoredInventoryMarkup(int inventoryWidth, int inventoryHeight, float slotSize, float margin)
        {
            SlotSize = slotSize;
            Margin = margin;
            SizeAndMargin = slotSize + margin;
            FrameWidth = SizeAndMargin * inventoryWidth + margin;
            FrameHeight = SizeAndMargin * inventoryHeight + margin;
        }
        
        public float SlotSize { get; }
        public float Margin { get; }
        public float SizeAndMargin { get; }
        public float FrameWidth { get; }
        public float FrameHeight { get; }
    }
}