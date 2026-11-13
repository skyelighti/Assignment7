using Gum.Forms.Controls;
using Gum.GueDeriving;
using Gum.Managers;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class InventoryUI
{
    Panel Root = new();
    Inventory inventory;
    int Columns = 5;
    int SlotSize = 72;
    int Gap = 8;
    int Padding = 16;

    RectangleRuntime background = new();
    Panel slotContainer = new();
    Label emptyLabel = new();
    public bool activeSelf { get; private set; } = false;
    readonly InputManager input;
    public InventoryUI(Inventory inv, InputManager input)
    {
        inventory = inv;
        this.input = input;
        Init();
        inv.OnInvUpdate += RefreshInventory;

    }
    public void Init()
    {
        activeSelf = false;
        Root.Dock(Dock.Fill);

        Root.AddChild(background);
        background.Anchor(Anchor.Center);
        background.Width = Columns * (SlotSize + Gap) - Gap + Padding * 2;
        background.IsFilled = true;
        background.FillRed = 28;
        background.FillGreen = 32;
        background.FillBlue = 42;
        background.FillAlpha = 245;

        background.AddChild(slotContainer);
        slotContainer.Anchor(Anchor.TopLeft);
        slotContainer.X = Padding;
        slotContainer.Y = Padding;
        slotContainer.Width = Columns * (SlotSize + Gap) - Gap;

    }

    void RefreshInventory()
    {
        foreach (var child in slotContainer.Visual.Children.ToArray())
            child.Parent = null;

        int count = inventory.items.Count;
        int rows = Math.Max(1, (count + Columns - 1) / Columns);

        slotContainer.Height = rows * (SlotSize + Gap) - Gap;
        background.Height = Padding + slotContainer.Height + Padding;
        emptyLabel.Visual.Visible = count == 0;

        for (int index = 0; index < count; index++)
        {
            var item = inventory.items[index];

            var slot = new RectangleRuntime();
            slotContainer.AddChild(slot);

            slot.Anchor(Anchor.TopLeft);
            slot.X = (index % Columns) * (SlotSize + Gap);
            slot.Y = (index / Columns) * (SlotSize + Gap);
            slot.Width = SlotSize;
            slot.Height = SlotSize;
            slot.IsFilled = true;
            slot.FillBlue = 78;

            var region = item.sprite?.Region;

            if (region != null)
            {
                var icon = new SpriteRuntime();
                slot.AddChild(icon);
                icon.Anchor(Anchor.Center);

                icon.Texture = region.Texture;
                icon.TextureAddress = TextureAddress.Custom;
                icon.SourceRectangle = region.SourceRectangle;
                icon.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
                icon.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;

                float scale = Math.Min((SlotSize - 16f) / region.Width, (SlotSize - 16f) / region.Height);

                icon.Width = region.Width * scale;
                icon.Height = region.Height * scale;
            }
            else
            {
                var name = new Label();
                slot.AddChild(name);
                name.Anchor(Anchor.Center);
                name.Visual.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
                name.Width = SlotSize - 8;
                name.Text = item.name;
            }
        }
    }

    // Moved from the old Inventory model to retain the template's UI bindings.
    public void InventoryToggle(GameAction action)
    {
        if (action == GameAction.ToggleInventory)
        {
            if (input.inputActionMap == InputActionMap.Gameplay)
            {
                SetActive(true);
                input.UpdateActionMap(InputActionMap.Inventory);
            }
            else if (input.inputActionMap == InputActionMap.Inventory)
            {
                SetActive(false);
                input.UpdateActionMap(InputActionMap.Gameplay);
            }
        }
    }

    public void SetActive(bool state)
    {
        if (state)
        {
            Root.AddToRoot();
            activeSelf = true;
            RefreshInventory();
        }
        else
        {
            Root.RemoveFromRoot();
            activeSelf = false;
        }
    }

}
