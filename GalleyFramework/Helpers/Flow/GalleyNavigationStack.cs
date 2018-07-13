using System;
using System.Collections.Generic;
using GalleyFramework.Views;
using GalleyFramework.ViewModels;
using GalleyFramework.Extensions;
using System.Linq;
namespace GalleyFramework.Helpers.Flow
{
    public sealed class GalleyNavigationStack : Stack<GalleyNavigationLayer>
    {
        public GalleyNavigationLayer CurrentLayer => this.Any() ? Peek() : null;
    }
}
