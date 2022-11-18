using System;
using System.Collections.Generic;

#nullable disable

namespace ChangeTracker
{
    public partial class Urunler
    {
        public Urunler()
        {
            Parcalars = new HashSet<Parcalar>();
            UrunParcalars = new HashSet<UrunParcalar>();
        }

        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public float Fiyat { get; set; }

        public virtual ICollection<Parcalar> Parcalars { get; set; }
        public virtual ICollection<UrunParcalar> UrunParcalars { get; set; }
    }
}
