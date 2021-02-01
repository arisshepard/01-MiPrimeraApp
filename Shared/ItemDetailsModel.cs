using System;
using System.Collections.Generic;
using System.Text;

namespace _01_MiPrimeraApp.Shared
{
    public class ItemDetailsModel<TItem>
    {
        public string ItemName { get; set; }
        public TItem Item { get; set; }
    }
}
