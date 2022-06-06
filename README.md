# KargoTakip
Google Maps Api
Google Cloud Firestore

Bu projede kargo dağıtım sistemi uygulaması yapılmıştır. Bir kargo firması, kullanıcıdan aldığı adres bilgilerini sistemine kayıt ederek ve bunu harita üzerinde göstererek kargocuyu canlı takip etmesi ve kargocunun en kısa yol ile teslimat yapması amaçlanmıştır. \
Proje C# dilinde masaüstü uygulaması olarak geliştirilmiştir.

Projede ilk önce kullanıcı giriş sayfası karşımıza çıkıyor. Yeni kullanıcı eklenebiliyor ve şifre değiştirilebilir. \
Kargo firması uygulamamıza giriş yaptıktan sonra teslimat için müşteri ve adres bilgileri girmesi istenmektedir. \
Adres girişi manuel olarak yazılabileceği gibi karşımıza çıkan haritadan konum seçerek de veritabanımıza kaydedilir. \
Veritabanımıza veriler gerçek zamanlı olarak kaydedilir ve kaydettiğimiz an teslimat durum sayfasından tüm kargolarımızın durumları listelenecektir. \
Buradan kargomuzu silebilir veya teslim edildi olarak işaretleyebiliriz. \
2.Guimizde ise karşımızda sadece harita ekranı vardır. Harita ekranı 1.Gui ile eş zamanlı olarak çalışır. \
Sisteme kargo eklendiğinde,silindiğinde anlık olarak 2.Guideki harita ekranından takip edilir. 2 Gui birbiriyle thread ile iletişim kurar. Ve işlemler sırayla değil aynı anda gerçekleşir. Böylelikle kargocunun hangi kargoyu teslim ettiğini takip edebiliriz. \
Teslim edilecek kargoların rotasını oluşturmak içinse en kısa yol algoritmasını kullanmamız gerekir. Böylelikle en az mesafe kaydederek bütün kargoları teslim etmiş oluruz.



<img
  src="/images/3.png"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto;  width: 300px">
  
  -Teslimat durumu veya kargo eklemek için seçim yapılır.
  
  <img
  src="/images/1.png"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto;  width: 300px">
  
  Kargo ekleme ekranı
  -Müşteri adı ve konum bilgileri girilerek kaydedilir
  
  
  <img
  src="/images/2.png"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto;  width: 300px">
  
  -Teslimat durum ekranında thread ile 2 guimiz aynı anda açılır ve eş zamanlı olarak çalışırlar.
  
