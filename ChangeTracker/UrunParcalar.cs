using System;
using System.Collections.Generic;

#nullable disable

namespace ChangeTracker
{
    public partial class UrunParcalar
    {
        public int UrunId { get; set; }
        public int ParcaId { get; set; }

        public virtual Parcalar Parca { get; set; }
        public virtual Urunler Urun { get; set; }
    }
}
