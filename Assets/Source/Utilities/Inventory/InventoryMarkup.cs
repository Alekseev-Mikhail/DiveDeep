namespace Source.Utilities.Inventory
{
    public struct InventoryMarkup
    {
        public InventoryMarkup(int inventoryWidth, int inventoryHeight, float slotSize, float margin)
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