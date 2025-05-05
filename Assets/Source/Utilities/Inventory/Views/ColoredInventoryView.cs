using System.Collections.Generic;
using Source.Utilities.Inventory.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Utilities.Inventory.Views
{
    public class ColoredInventoryView : View<IColoredItem>
    {
        private readonly ColoredInventoryMarkup _markup;

        public ColoredInventoryView(ColoredInventoryMarkup markup)
        {
            _markup = markup;
        }

        public override Dictionary<IColoredItem, VisualElement> ConstructView(UIDocument document, Inventory<IColoredItem> inventory)
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

            var itemElements = new Dictionary<IColoredItem, VisualElement>();
            foreach (var (item, position) in inventory.GetItems())
            {
                var element = ConstructItem(item, position);
                itemElements.Add(item, element);
                frame.Add(element);
            }
            return itemElements;
        }

        public override void DeconstructView()
        {
        }

        public override Vector2Int GetNearestSlot(float x, float y)
        {
            var nearestX = Mathf.RoundToInt((x - _markup.Margin) / _markup.SizeAndMargin);
            var nearestY = Mathf.RoundToInt((y - _markup.Margin) / _markup.SizeAndMargin);
            return new Vector2Int(nearestX, nearestY);
        }

        public override Vector2 GetSlotPosition(int x, int y)
        {
            var posX = _markup.SizeAndMargin * x + _markup.Margin;
            var posY = _markup.SizeAndMargin * y + _markup.Margin;
            return new Vector2(posX, posY);
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

        private VisualElement ConstructItem(IColoredItem item, Vector2Int position) => new()
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