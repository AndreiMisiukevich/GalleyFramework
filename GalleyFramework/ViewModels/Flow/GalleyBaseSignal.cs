using System;
namespace GalleyFramework.ViewModels.Flow
{
    public abstract class GalleyBaseSignal
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }

        public GalleyBaseSignal() : this(null)
        {
        }

        public GalleyBaseSignal(string name)
        {
            Name = name ?? GetType().Name;
        }
    }
}