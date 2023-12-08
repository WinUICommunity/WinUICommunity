namespace WindowUI;

internal sealed class DependencyObjectReferenceAddedEventArgs : EventArgs
{
    public DependencyObjectReferenceAddedEventArgs(Type addedItemType, int itemsTotal)
    {
        AddedItemType = addedItemType;
        ItemsTotal = itemsTotal;
    }

    public Type AddedItemType { get; }

    public int ItemsTotal { get; }
}

internal sealed class DependencyObjectReferenceRemovedEventArgs : EventArgs
{
    public DependencyObjectReferenceRemovedEventArgs(Type removedItemType, int itemsTotal)
    {
        RemovedItemType = removedItemType;
        ItemsTotal = itemsTotal;
    }

    public Type RemovedItemType { get; }

    public int ItemsTotal { get; }
}

internal sealed class DependencyObjectWeakReferences : IDisposable
{
    public readonly List<Item> items = new();

    private readonly SemaphoreSlim semaphore = new(1);

    public event EventHandler<DependencyObjectReferenceAddedEventArgs>? DependencyObjectAdded;

    public event EventHandler<DependencyObjectReferenceRemovedEventArgs>? DependencyObjectRemoved;

    public int Count => this.items.Count;

    private bool IsDisposed { get; set; }

    public record Item(Type Type, WeakReference<DependencyObject> WeakReference);

    public void Add(DependencyObject dependencyObject)
    {
        WeakReference<DependencyObject> reference = new(dependencyObject);
        Item item = new(dependencyObject.GetType(), reference);
        this.items.Add(item);
        OnDependencyObjectReferenceAdded(item.Type);
    }

    public async Task<IReadOnlyCollection<DependencyObject>> GetDependencyObjects()
    {
        try
        {
            await this.semaphore.WaitAsync();

            List<DependencyObject> dependencyObjects = new();

            for (int i = this.items.Count - 1; i >= 0; i--)
            {
                Item targetItem = this.items[i];

                if (targetItem.WeakReference.TryGetTarget(out DependencyObject? aliveObject) is false)
                {
                    Type type = targetItem.Type;
                    this.items.RemoveAt(i);
                    OnDependencyObjectReferenceRemoved(type);
                    continue;
                }

                dependencyObjects.Add(aliveObject);
            }

            return dependencyObjects;
        }
        finally
        {
            _ = this.semaphore.Release();
        }
    }

    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    private void OnDependencyObjectReferenceAdded(Type addedItemType)
    {
        DependencyObjectAdded?.Invoke(this, new DependencyObjectReferenceAddedEventArgs(addedItemType, Count));
    }

    private void OnDependencyObjectReferenceRemoved(Type removedItemType)
    {
        DependencyObjectRemoved?.Invoke(this, new DependencyObjectReferenceRemovedEventArgs(removedItemType, Count));
    }

    private void Dispose(bool isDisposing)
    {
        if (IsDisposed is not true && isDisposing is true)
        {
            this.semaphore.Dispose();
            IsDisposed = true;
        }
    }
}