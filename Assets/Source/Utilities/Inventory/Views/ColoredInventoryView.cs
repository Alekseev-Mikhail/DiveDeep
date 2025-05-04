using System.Collections.Generic;
using Source.Utilities.Inventory.Items;
using Source.Utilities.Inventory.Views.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Utilities.Inventory.Views
{
    public class ColoredInventoryView : IView<ColoredItem>
    {
        private readonly InventoryMarkup _markup;
        private readonly Dictionary<VisualElement, ColoredInventoryDragger> _itemDraggers = new();

        public ColoredInventoryView(InventoryMarkup markup)
        {
            _markup = markup;
        }

        public void ConstructView(UIDocument document, Inventory<ColoredItem> inventory)
        {
            document.rootVisualElement.style.justifyContent = Justify.Center;
            document.rootVisualElement.style.alignItems = Align.Center;

            var frame = ConstructFrame();
            document.rootVisualElement.Add(frame);

            for (var x = 0; x < inventory.GetWidth(); x++)
            {
                for (var y = 0; y < inventory.GetHeight(); y++)
                {
                    frame.Add(ConstructSlot(x, y));
                }
            }

            foreach (var (item, position) in inventory.GetItems())
            {
                var element = ConstructItem(item, position);
                var dragger = new ColoredInventoryDragger(item, inventory, _markup);
                element.AddManipulator(dragger);
                _itemDraggers.Add(element, dragger);
                frame.Add(element);
            }
        }

        public void BreakDown()
        {
            foreach (var (element, dragger) in _itemDraggers)
            {
                element.RemoveManipulator(dragger);
            }

            _itemDraggers.Clear();
        }

        private VisualElement ConstructFrame() => new()
        {
            style =
            {
                display = DisplayStyle.Flex,
                flexDirection = FlexDirection.Column,
                justifyContent = Justify.SpaceBetween,
                backgroundColor = new Color(0, 0, 0, 0.5f),
                width = _markup.FrameWidth,
                height = _markup.FrameHeight
            }
        };

        private VisualElement ConstructSlot(int x, int y) => new()
        {
            style =
            {
                position = Position.Absolute,
                left = _markup.SizeAndMargin * x + _markup.Margin,
                top = _markup.SizeAndMargin * y + _markup.Margin,
                backgroundColor = new Color(0, 0, 0, 1),
                minWidth = _markup.SlotSize,
                minHeight = _markup.SlotSize
            }
        };

        private VisualElement ConstructItem(ColoredItem item, Vector2Int position) => new()
        {
            style =
            {
                position = Position.Absolute,
                left = _markup.SizeAndMargin * position.x + _markup.Margin,
                top = _markup.SizeAndMargin * position.y + _markup.Margin,
                backgroundColor = item.Color,
                minWidth = _markup.SlotSize * item.Width + _markup.Margin * (item.Width - 1),
                minHeight = _markup.SlotSize * item.Height + _markup.Margin * (item.Height - 1)
            }
        };
    }
}