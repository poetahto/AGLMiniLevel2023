using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace InventorySystem
{
    [RequireComponent(typeof(UIDocument))]
    public class InventoryUIController : MonoBehaviour
    {
        private static VisualElement m_ghostIcon;
        private static bool m_isDragging;
        private static InventorySlot m_origin;

        [SerializeField] private Inventory m_inventory;
        [SerializeField] private Transform m_dropTransform;
        [SerializeField] private AudioSource m_dropSound;

        private readonly List<InventorySlot> InventorySlots = new();

        private VisualElement m_root;
        private VisualElement m_slotContainer;

        public bool IsDisplaying { get; private set; }

        private void Awake()
        {
            m_root = GetComponent<UIDocument>().rootVisualElement;
            m_slotContainer = m_root.Q<VisualElement>("SlotContainer");

            m_ghostIcon = m_root.Q<VisualElement>("GhostIcon");
            m_ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            m_ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);

            for (int i = 0; i < m_inventory.MaxCapacity; i++)
            {
                var newSlot = new InventorySlot();
                m_slotContainer.Add(newSlot);
                InventorySlots.Add(newSlot);
            }

            InitInventoryUI();

            Display(false);
        }

        private void OnEnable()
        {
            m_inventory.OnInventoryChanged += InventoryChanged;
        }

        private void OnDisable()
        {
            m_inventory.OnInventoryChanged -= InventoryChanged;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!m_isDragging) return;

            var slots = InventorySlots.Where(s => s.worldBound.Overlaps(m_ghostIcon.worldBound));
            var inventorySlots = slots as InventorySlot[] ?? slots.ToArray();

            if (inventorySlots.Length != 0)
            {
                var slot = inventorySlots.OrderBy(s => Vector2.Distance(s.worldBound.position, m_ghostIcon.worldBound.position))
                    .First();

                if (slot.isEmpty())
                {
                    slot.SetItem(m_origin.Item);
                    if (slot != m_origin)
                    {
                        m_origin.ClearItem();
                        m_origin = null;
                    }
                }

                m_origin?.SetItem(m_origin.Item);
                m_isDragging = false;
            }
            else
            {
                Instantiate(m_origin.Item.Data.Prefab, m_dropTransform.position, Quaternion.identity);
                m_dropSound.Play();

                m_inventory.Remove(m_origin.Item.Data);

                if (m_origin.Item != null)
                {
                    m_origin.SetItem(m_origin.Item);
                }
                else
                {
                    m_origin.ClearItem();
                    m_origin = null;
                }


                m_isDragging = false;
            }


            m_ghostIcon.style.visibility = Visibility.Hidden;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!m_isDragging)
            {
                return;
            }

            m_ghostIcon.style.top = evt.position.y - m_ghostIcon.layout.height / 2;
            m_ghostIcon.style.left = evt.position.x - m_ghostIcon.layout.width / 2;
        }

        private void InventoryChanged(InventoryItem _item, InventoryChangeType _changeType)
        {
            switch (_changeType)
            {
                case InventoryChangeType.PickUp:
                    if (ContainsItem(_item, out var slot))
                    {
                        slot.Stack.text = _item.StackSize.ToString();
                    }
                    else
                    {
                        var emptySlot = InventorySlots.FirstOrDefault(s => s.isEmpty());
                        emptySlot?.SetItem(_item);
                    }
                    break;
                case InventoryChangeType.Drop:
                    if (ContainsItem(_item, out var sl))
                    {
                        if (_item.StackSize == 0)
                        {
                            sl.ClearItem();
                        }
                        else
                        {
                            sl.Stack.text = _item.StackSize.ToString();
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_changeType), _changeType, null);
            }
        }

        private void InitInventoryUI()
        {
            foreach (var item in m_inventory)
            {
                var emptySlot = InventorySlots.FirstOrDefault(s => s.isEmpty());
                emptySlot?.SetItem(item);
            }
        }

        private bool ContainsItem(InventoryItem _item, out InventorySlot _slot)
        {
            _slot = InventorySlots.Find(slot => slot.Item == _item);
            return _slot != null;
        }

        public static void StartDragging(Vector2 _position, InventorySlot _origin)
        {
            m_isDragging = true;
            m_origin = _origin;

            m_ghostIcon.style.top = _position.y - m_ghostIcon.layout.height / 2;
            m_ghostIcon.style.left = _position.x - m_ghostIcon.layout.width / 2;

            m_ghostIcon.style.backgroundImage = Background.FromSprite(_origin.Item.Data.Icon);

            m_ghostIcon.style.visibility = Visibility.Visible;
        }

        public void Display(bool _arg)
        {
            IsDisplaying = _arg;
            m_root.style.display = _arg ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
