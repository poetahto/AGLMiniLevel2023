using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField, Range(1.0f, 10.0f)] private float m_detectionRange;

    private Transform m_transform;

    private List<Collider> m_selectables = new();
    
    private void Awake()
    {
        m_transform = transform;
    }

    private void Update()
    {
        m_selectables = Physics.OverlapSphere(m_transform.position, m_detectionRange * 1.5f)
            .Where(c => c.TryGetComponent(out ISelectable o)).ToList();

        foreach (var selectable in m_selectables)
        {
            var s = selectable.GetComponent<ISelectable>();
            if (Vector3.Distance(m_transform.position, selectable.transform.position) <= m_detectionRange)
            {
                s.Select();
            }
            else
            {
                s.DeSelect();
            }
        }
    }

    public Transform GetCollectableTransform(ICollectable collectable)
    {
        foreach (var s in m_selectables)
        {
            if (!s.TryGetComponent<ICollectable>(out var c)) continue;
            
            if (c == collectable)
                return s.transform;
        }

        return null;
    }

    public IEnumerable<ICollectable> GetCollectables() 
    {
        var list = new List<ICollectable>();
        foreach (var selectable in m_selectables)
        {
            if (selectable.TryGetComponent(out ICollectable c)) 
                list.Add(c);
        }
        return list;
    }
    
    public IEnumerable<IInteractable> GetIntractable() 
    {
        var list = new List<IInteractable>();
        foreach (var selectable in m_selectables)
        {
            if (selectable.TryGetComponent(out IInteractable c)) 
                list.Add(c);
        }
        return list;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_detectionRange);
    }
}
