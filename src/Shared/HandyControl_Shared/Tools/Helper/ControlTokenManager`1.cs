using System;
using System.Collections.Generic;
using System.Windows;

namespace HandyControl.Tools;

internal class ControlTokenManager<T>(
    Action<string, T> registerCallback = null,
    Action<string, T> unregisterCallback = null
)
    where T : FrameworkElement
{
#if NET40
    private readonly Dictionary<string, WeakReference> _controlDict = new();

    public void Register(string token, T control)
    {
        if (string.IsNullOrEmpty(token) || control == null)
        {
            return;
        }

        _controlDict[token] = new WeakReference(control);
        registerCallback?.Invoke(token, control);
    }

    public void Unregister(string token, T control)
    {
        if (string.IsNullOrEmpty(token) || control == null)
        {
            return;
        }

        if (!_controlDict.TryGetValue(token, out var reference))
        {
            return;
        }

        if (!ReferenceEquals(reference.Target, control))
        {
            return;
        }

        _controlDict.Remove(token);
        unregisterCallback?.Invoke(token, control);
    }

    public void Unregister(T control)
    {
        if (control == null)
        {
            return;
        }

        string unregisteredToken = null;
        foreach (var item in _controlDict)
        {
            if (!ReferenceEquals(control, item.Value.Target))
            {
                continue;
            }

            unregisteredToken = item.Key;
            break;
        }

        if (unregisteredToken == null)
        {
            return;
        }

        _controlDict.Remove(unregisteredToken);
        unregisterCallback?.Invoke(unregisteredToken, control);
    }

    public void Unregister(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        if (!_controlDict.TryGetValue(token, out var reference))
        {
            return;
        }

        _controlDict.Remove(token);
        unregisterCallback?.Invoke(token, (T) reference.Target);
    }

    public bool TryGetControl(string token, out T control)
    {
        control = null;

        if (string.IsNullOrEmpty(token) || !_controlDict.TryGetValue(token, out var reference))
        {
            return false;
        }

        control = (T) reference.Target;
        return true;
    }

    public void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not T control)
        {
            return;
        }

        if (e.NewValue == null)
        {
            Unregister(control);
        }
        else
        {
            Register(e.NewValue.ToString(), control);
        }
    }
#else
    private readonly Dictionary<string, WeakReference<T>> _controlDict = new();

    public void Register(string token, T control)
    {
        if (string.IsNullOrEmpty(token) || control == null)
        {
            return;
        }

        _controlDict[token] = new WeakReference<T>(control);
        registerCallback?.Invoke(token, control);
    }

    public void Unregister(string token, T control)
    {
        if (string.IsNullOrEmpty(token) || control == null)
        {
            return;
        }

        if (!_controlDict.TryGetValue(token, out var reference))
        {
            return;
        }

        if (!reference.TryGetTarget(out var target))
        {
            return;
        }

        if (!ReferenceEquals(target, control))
        {
            return;
        }

        _controlDict.Remove(token);
        unregisterCallback?.Invoke(token, control);
    }

    public void Unregister(T control)
    {
        if (control == null)
        {
            return;
        }

        string unregisteredToken = null;
        foreach (var item in _controlDict)
        {
            if (!item.Value.TryGetTarget(out var target) || !ReferenceEquals(control, target))
            {
                continue;
            }

            unregisteredToken = item.Key;
            break;
        }

        if (unregisteredToken == null)
        {
            return;
        }

        _controlDict.Remove(unregisteredToken);
        unregisterCallback?.Invoke(unregisteredToken, control);
    }

    public void Unregister(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        if (!_controlDict.TryGetValue(token, out var reference))
        {
            return;
        }

        _controlDict.Remove(token);

        if (!reference.TryGetTarget(out var target))
        {
            return;
        }

        unregisterCallback?.Invoke(token, target);
    }

    public bool TryGetControl(string token, out T control)
    {
        control = null;

        if (string.IsNullOrEmpty(token) || !_controlDict.TryGetValue(token, out var reference))
        {
            return false;
        }

        return reference.TryGetTarget(out control);
    }

    public void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not T control)
        {
            return;
        }

        if (e.NewValue == null)
        {
            Unregister(control);
        }
        else
        {
            Register(e.NewValue.ToString(), control);
        }
    }
#endif
}
