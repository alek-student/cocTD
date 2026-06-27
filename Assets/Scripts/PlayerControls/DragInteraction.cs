using DG.Tweening;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DragInteraction : MonoBehaviour
{
    public LayerMask cardMask;

    private enum InteractState
    {
        Idle,
        Drag
    }
    InteractState state = InteractState.Idle;

    void Update()
    {
        if (state == InteractState.Drag)
        {
            target.OnDrag();
        }
        else if (state == InteractState.Idle)
        {
            var previousTarget = target;
            if (TryGetValidTarget())
            {
                if (previousTarget != target)
                {
                    target.OnHoverBegin();
                }
            }
            if (previousTarget != null && previousTarget != target)
            {
                previousTarget.OnHoverEnd();
            }
        }
    }

    DragInterface target; Transform targetTransform;
    bool TryGetValidTarget()
    {
        target = null;
        var hit = Physics2D.Raycast(InteractionHelpers.GetMousePosition(), Vector2.zero, 100, cardMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject.GetComponent<DragInterface>();
            targetTransform = hit.collider.gameObject.transform;
        }
        return target != null;
    }

    public void MouseButton(CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (state == InteractState.Idle)
        {
            if (TryGetValidTarget())
            {
                target.OnDragBegin();
                state = InteractState.Drag;
            }
        }
        else // if (state == InteractState.Drag)
        {
            target.OnDragRelease();
            state = InteractState.Idle;
        }
    }
    public void RightMouseButton(CallbackContext context)
    {
        if (context.started && state == InteractState.Drag)
        {
            target.OnDragCancel();
            state = InteractState.Idle;
        }
    }

    public void Rotate(CallbackContext context)
    {
        if (context.started && state == InteractState.Drag)
        {
            targetTransform.DORotate(new Vector3(0, 0, targetTransform.eulerAngles.z + 90 - (targetTransform.eulerAngles.z % 90)), 0.1f);
        }
    }
}
