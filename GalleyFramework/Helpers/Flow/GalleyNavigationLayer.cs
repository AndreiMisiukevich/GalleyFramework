using System;
using System.Collections.Generic;
using System.Linq;
namespace GalleyFramework.Helpers.Flow
{
    public sealed class GalleyNavigationLayer : List<GalleyViewItem>
    {
        public GalleyNavigationLayer(IEnumerable<GalleyViewItem> items) : base(items)
        {
        }

        public GalleyViewItem CurrentItem => this.FirstOrDefault();
    }
}
