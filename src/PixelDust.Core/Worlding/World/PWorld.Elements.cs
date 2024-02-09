﻿using PixelDust.Core.Elements;
using PixelDust.Core.Elements.Interfaces;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld : IElementManager
    {
        public void InstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            InstantiateElement(pos, (uint)PElementsHandler.GetIdOfElementType<T>());
        }
        public void InstantiateElement(Vector2Int pos, uint id)
        {
            InstantiateElement(pos, PElementsHandler.GetElementById(id));
        }
        public void InstantiateElement(Vector2Int pos, PElement value)
        {
            _ = TryInstantiateElement(pos, value);
        }
        public void UpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            _ = TryUpdateElementPosition(oldPos, newPos);
        }
        public void SwappingElements(Vector2Int element1, Vector2Int element2)
        {
            _ = TrySwappingElements(element1, element2);
        }
        public void DestroyElement(Vector2Int pos)
        {
            _ = TryDestroyElement(pos);
        }
        public void ReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            _ = TryReplaceElement<T>(pos);
        }
        public void ReplaceElement(Vector2Int pos, uint id)
        {
            _ = TryReplaceElement(pos, id);
        }
        public void ReplaceElement(Vector2Int pos, PElement value)
        {
            _ = TryReplaceElement(pos, value);
        }
        public PElement GetElement(Vector2Int pos)
        {
            _ = TryGetElement(pos, out PElement value);
            return value;
        }
        public ReadOnlySpan<(Vector2Int, PWorldElementSlot)> GetElementNeighbors(Vector2Int pos)
        {
            _ = TryGetElementNeighbors(pos, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors);
            return neighbors;
        }
        public PWorldElementSlot GetElementSlot(Vector2Int pos)
        {
            _ = TryGetElementSlot(pos, out PWorldElementSlot value);
            return value;
        }
        public void SetElementTemperature(Vector2Int pos, short value)
        {
            _ = TrySetElementTemperature(pos, value);
        }

        public bool TryInstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            return TryInstantiateElement(pos, (uint)PElementsHandler.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(Vector2Int pos, uint id)
        {
            return TryInstantiateElement(pos, PElementsHandler.GetElementById(id));
        }
        public bool TryInstantiateElement(Vector2Int pos, PElement value)
        {
            if (!InsideTheWorldDimensions(pos) || !IsEmptyElementSlot(pos))
            {
                return false;
            }

            NotifyChunk(pos);

            this.Elements[pos.X, pos.Y].Instantiate(value);
            return true;
        }
        public bool TryUpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmptyElementSlot(oldPos) ||
                !IsEmptyElementSlot(newPos))
            {
                return false;
            }

            NotifyChunk(oldPos);
            NotifyChunk(newPos);

            this.Elements[newPos.X, newPos.Y].Copy(this.Elements[oldPos.X, oldPos.Y]);
            this.Elements[oldPos.X, oldPos.Y].Destroy();
            return true;
        }
        public bool TrySwappingElements(Vector2Int element1, Vector2Int element2)
        {
            if (!InsideTheWorldDimensions(element1) ||
                !InsideTheWorldDimensions(element2) ||
                IsEmptyElementSlot(element1) ||
                IsEmptyElementSlot(element2))
            {
                return false;
            }

            NotifyChunk(element1);
            NotifyChunk(element2);

            PWorldElementSlot oldValue = this.Elements[element1.X, element1.Y];
            PWorldElementSlot newValue = this.Elements[element2.X, element2.Y];

            this.Elements[element1.X, element1.Y].Copy(newValue);
            this.Elements[element2.X, element2.Y].Copy(oldValue);

            return true;
        }
        public bool TryDestroyElement(Vector2Int pos)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmptyElementSlot(pos))
            {
                return false;
            }

            NotifyChunk(pos);
            this.Elements[pos.X, pos.Y].Destroy();

            return true;
        }
        public bool TryReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            return TryDestroyElement(pos) && TryInstantiateElement<T>(pos);
        }
        public bool TryReplaceElement(Vector2Int pos, uint id)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, id);
        }
        public bool TryReplaceElement(Vector2Int pos, PElement value)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, value);
        }
        public bool TryGetElement(Vector2Int pos, out PElement value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmptyElementSlot(pos))
            {
                value = null;
                return false;
            }

            value = this.Elements[pos.X, pos.Y].Instance;
            return true;
        }
        public bool TryGetElementNeighbors(Vector2Int pos, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors)
        {
            neighbors = default;

            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            Vector2Int[] neighborsPositions = GetElementNeighborPositions(pos);

            (Vector2Int, PWorldElementSlot)[] slotsFound = new (Vector2Int, PWorldElementSlot)[neighborsPositions.Length];
            int count = 0;

            for (int i = 0; i < neighborsPositions.Length; i++)
            {
                Vector2Int position = neighborsPositions[i];
                if (TryGetElementSlot(position, out PWorldElementSlot value))
                {
                    slotsFound[count] = (position, value);
                    count++;
                }
            }

            if (count > 0)
            {
                neighbors = new ReadOnlySpan<(Vector2Int, PWorldElementSlot)>(slotsFound, 0, count);
                return true;
            }

            return false;
        }
        public bool TryGetElementSlot(Vector2Int pos, out PWorldElementSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            value = this.Elements[pos.X, pos.Y];
            return !value.IsEmpty;
        }
        public bool TrySetElementTemperature(Vector2Int pos, short value)
        {
            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            if (this.Elements[pos.X, pos.Y].Temperature != value)
            {
                NotifyChunk(pos);
                this.Elements[pos.X, pos.Y].SetTemperatureValue(value);
            }

            return true;
        }

        // Tools
        public bool IsEmptyElementSlot(Vector2Int pos)
        {
            return !InsideTheWorldDimensions(pos) || this.Elements[pos.X, pos.Y].IsEmpty;
        }

        // Utilities
        private static Vector2Int[] GetElementNeighborPositions(Vector2Int pos)
        {
            return
            [
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y - 1),
                new(pos.X - 1, pos.Y - 1),
                new(pos.X + 1, pos.Y),
                new(pos.X - 1, pos.Y),
                new(pos.X, pos.Y + 1),
                new(pos.X + 1, pos.Y + 1),
                new(pos.X - 1, pos.Y + 1),
            ];
        }
    }
}
