using System;
using System.Collections.Generic;

#nullable disable

namespace ChangeTracker
{
    public partial class Parcalar
    {
        public Parcalar()
        {
            UrunParcalars = new HashSet<UrunParcalar>();
        }

        public int Id { get; set; }
        public string ParcaAdi { get; set; }
        public int? UrunId { get; set; }

        public virtual Urunler Urun { get; set; }
        public virtual ICollection<UrunParcalar> UrunParcalars { get; set; }
    }
}
