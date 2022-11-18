using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ExampleContext context = new();

            #region ChangeTracker Property'si
            //Takip edilen nesnelere erişebilmemizi sağlayan ve gerektirdiği takdirde işlemler gerçekleştirmemizi sağlayan bir properydir.
            //Context classı'nın base'i class'ı olan DbContext sınıfının bir member'ıdır.
            //var urunler = await context.Urunlers.ToListAsync(); //Takip edilecek nesneler belleğe gelir.
            //context.Urunlers.Remove(urunler[2]);
            //var datas = context.ChangeTracker.Entries();
            //await context.SaveChangesAsync();
            //Console.WriteLine();
            #endregion

            #region DetectChanges Metodu
            //Yapılan işlemde changetracker'ın bu işlemi takip edip etmediğine dair şüphemiz olabilir.
            /*Ef Core, context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri Change Tracker sayesinde takip edebilmekte ve nesnelerde
             olan verisel değişiklikler yakalanarak bunların anlık görüntüleri (snapshot)'ini oluşturabilir.
            */
            /*Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir.SaveChanges fonksiyonu çağırıldığı anda nesneler
              Ef core tarafından otomatik kontrol edilirler.
             */
            //Ancak,yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişikliklerin algılanmasını opsiyonel olarak gerçekleştirmek isteyebiliriz.
            //İşte bunun DetectChanges fonksiyonu kullanılabilir ve her ne kadar Ef core değişiklikleri algılıyor olsa da siz yine de iradenizle kontrole zorlayabilirsiniz.

            //var urun = await context.Urunlers.FirstOrDefaultAsync(u => u.UrunId == 3);
            //urun.Fiyat = 123;
            //var datas = context.ChangeTracker.Entries();
            //context.ChangeTracker.DetectChanges(); // işi garantiye almak. İzlemeyi garantiye almak.Ekstra maliyet.
            //await context.SaveChangesAsync();
            //Console.WriteLine();

            #endregion

            #region AutoDetectChangesEnabled Property'si
            //SaveChanges fonksiyonu otomatik olarak DetectedChanges'i çağırır. Eğer bu property devre dışı bırakılırsa DecectedChanges devre dışı bırakılır ve izlenme olmaz.

            //context.ChangeTracker.AutoDetectChangesEnabled = false;
            //var urun = await context.Urunlers.FirstOrDefaultAsync(u => u.UrunId == 4);
            //urun.Fiyat = 666;
            //await context.SaveChangesAsync();
            //var datas = context.ChangeTracker.Entries();
            //Console.WriteLine();
            #endregion

            #region Entries Metodu
            //Contextteki Entry metodunun koleksiyonel versiyonudur. Entry tek bir nesnenin izlenme neticesini verirken, Entries birden fazla nesnenin izlenme neticesini verir.
            //Entries metodu, DetectChanges metodunu tetikler.Bu durum da tıpkı SaveChanges'da olduğu gibi bir maliyettir.
            //var urunler = await context.Urunlers.ToListAsync();
            //urunler.FirstOrDefault(u => u.UrunId == 4).UrunAdi = "Şapka";
            //urunler.FirstOrDefault(u => u.UrunId == 8).UrunAdi = "Yüzüks";
            //context.ChangeTracker.Entries().ToList().ForEach(e =>
            //{
            //    if (e.State == EntityState.Deleted)
            //    {
            //        //..
            //    }
            //    else if (e.State == EntityState.Modified)
            //    {
            //        Console.WriteLine("Modified");
            //    }
            //    else
            //    {
            //        //...
            //    }
            //});
            #endregion

            #region AcceptAllChanges Metodu
            /* SaveChanges() veya SaveChanges(true) tetiklendiğinde Ef core her şeyin yolunda gittiğini varsayarak track ettiği verilerin takibini keser ve yeni değişikliklerin
             takip edilmesini bekler.Böyle bir durumda beklenmeyen bir durum/olası bir hata söz konusu olursa eğer Ef core takip ettiği nesneleri bırakacağı için bir düzeltme mevzu bahis olmayacaktır.*/
            //Haliyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges metotları devreye girecektir.
            // SaveChanges(false) Ef Core'a gerekli veritabanı komutlarını yürütmesini söyler ancak gerektiğinde yeniden oynatılabilmesi için değişiklikleri beklemeye/nesneleri takibe devam eder.
            //Ta ki AcceptAllChanges metodunu kendi irademizle çağırana kadar.
            //SaveChanges(false) ile işlemin başarılı olduğundan emin olduktan sonra AcceptAllChanges metodunu kendi irademiz ile çağırıp nesnelerin takibini kesebiliriz.
            //var urunler = await context.Urunlers.ToListAsync();
            //urunler.FirstOrDefault(u => u.UrunId == 4).UrunAdi = "asdasfdsdf";
            //urunler.FirstOrDefault(u => u.UrunId == 8).UrunAdi = "Yüasdasdsdfzüks";
            //await context.SaveChangesAsync(false);
            //context.ChangeTracker.AcceptAllChanges();
            //Console.WriteLine();
            #endregion

            #region HasChanges Metodu
            //Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisini verir.
            //Aka planda DetectChanges metodunu tetikler.
            //var result = context.ChangeTracker.HasChanges();
            //Console.WriteLine();
            #endregion

            #region EntityStates

            #region Detached
            //Detached Nesnenin change tracker mekanizması tarafından takip edilmediğini ifade eder.
            //Urunler urun = new();
            //Console.WriteLine(context.Entry(urun).State);
            //await context.SaveChangesAsync(); // Bu metodu çalıştırsan bile veri tabanında herhangi bir değişiklik olmaz.Çünkü takip edilen herhangi bir nesne yok.
            #endregion

            #region Added
            //Veritabanına eklenecek nesneyi ifade eder.Added henüz veritabanına işlenmeyen veriyi ifade eder.SaveChanges fonksiyonu çağırıldığında insert sorgusu oluşturulacağı anlamına gelir.
            //Urunler urun = new Urunler
            //{
            //    UrunAdi = "Şişme mont",
            //    Fiyat = 500
            //};
            //await context.Urunlers.AddAsync(urun);
            //Console.WriteLine(context.Entry(urun).State);
            //await context.SaveChangesAsync();
            #endregion

            #region Unchanged
            //Veritabanından sorgulandığından beri nesne üzerinde herhangi bir değişiklik yapılmadığını ifade eder.
            //Sorgu neticesinde elde edilen tüm nesneler başlangıçta bu state değerindedir.
            //var urun = context.Urunlers.FirstOrDefault(u => u.UrunId == 2);
            //Console.WriteLine(urun.UrunAdi);
            //urun.UrunAdi = "İspanyol Paca"; //Aynı değer atandığı için unchanged.
            //Console.WriteLine(context.Entry(urun).State);

            #endregion

            #region Modified
            //Nesne üzerinde değişiklik/güncelleme yapıldığını ifade eder.Savechanges fonksiyonu çağırıldığında update sorgusu oluşturulacağı anlamına gelir.
            //var urun = context.Urunlers.FirstOrDefault(u => u.UrunId == 2);
            //Console.WriteLine(urun.UrunAdi);
            //urun.UrunAdi = "İspanyol Pacaaaaaaaa"; //Aynı değer atandığı için unchanged.
            //Console.WriteLine(context.Entry(urun).State);
            //await context.SaveChangesAsync();
            #endregion

            #region Deleted
            //Nesnenin silindiğini ifade eder. Savechanges fonk. çağırıldığında delete sorgusu oluşturulacağı anlamına gelir.
            //var urun = context.Urunlers.FirstOrDefault(u => u.UrunId == 3);
            //context.Urunlers.Remove(urun);
            //Console.WriteLine(context.Entry(urun).State);
            //await context.SaveChangesAsync();
            #endregion



            #endregion

            #region Conetxt nesnesi üzerinden change tracker 
            var urun = await context.Urunlers.FirstOrDefaultAsync(u => u.UrunId == 5);
            urun.Fiyat = 233;
            urun.UrunAdi = "fggfg"; //Modified || Update

            #region OriginalValues Property'si
            //Yukarda değerlerini değiştirdiğimiz nesnenin veritabanındaki orijinal değerlerini aldık.
            var fiyat = context.Entry(urun).OriginalValues.GetValue<float>(nameof(urun.Fiyat));
            var urunAdi = context.Entry(urun).OriginalValues.GetValue<string>(nameof(urun.UrunAdi));
            Console.WriteLine("OriginalValues;(veritabanındaki değeri)");
            Console.WriteLine(fiyat);
            Console.WriteLine(urunAdi);
            #endregion

            #region CurrentValues Property'si
            //SaveChanges yapmadık, ama veritabanından ilgili nesneyi çağırıp değer atadık.Veritabanında değiştirmeden buradan hangi değer atanmış görebiliriz.
            //Veritabanındaki değeri getirmez, o anki instance'ın heapteki değerini gösterir.
            var urunAdiCurrent = context.Entry(urun).CurrentValues.GetValue<string>(nameof(urun.UrunAdi));
            var fiyatCurrent = context.Entry(urun).CurrentValues.GetValue<float>(nameof(urun.Fiyat));
            Console.WriteLine("CurrentValues ;(buradan atanan değer)");
            Console.WriteLine(urunAdiCurrent);
            Console.WriteLine(fiyatCurrent);
            #endregion
            #region GetDatabaseValues Metodu
            //Original Values muadili
            var x = await context.Entry(urun).GetDatabaseValuesAsync();
            Console.WriteLine(x.EntityType);
            //Console.WriteLine();
            #endregion
            #endregion

            #region Change Tracker'ın Interceptor Olarak Kullanması
            var eklenecekUrun = new Urunler { UrunAdi = "Peçetelik", Fiyat = 50 };
            context.Urunlers.Add(eklenecekUrun);
            await context.SaveChangesAsync();
            #endregion
        }
    }
}
