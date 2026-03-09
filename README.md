# Dağıtık Sistemlerde Servis Bağımlılıklarının Performans Analizi

Bu proje, Ahmet Yesevi Üniversitesi Yazılım Mühendisliği Dönem Projesi kapsamında yazılmıştır.

## 🎯 Araştırma Problemi
Dağıtık mimarilerde kurgulanan merkezi bir API servisinin, servis bağımlılıkları (dependency) arttıkça eşzamanlı istekleri karşılama oranındaki değişim ve sunucu kaynak tüketimi (CPU/RAM) arasındaki ilişki nedir?

## 🛠️ Yöntem ve Teknik Detaylar
Çalışmada, üniversitenin hazırlama ilkelerine uygun olarak **"Deneme Modeli" (Experimental Model)** uygulanmıştır.

* **Geliştirme Ortamı:** .NET Core Web API (C#)
***Test Araçları:** JMeter ve Apache Benchmark 
* **Veri Güvenirliği:** Her bir test senaryosu, istatistiksel sapmaları önlemek adına **en az 30 kez** tekrarlanmış ve ortalamalar baz alınmıştır.
* **Ölçülen Metrikler:** Yanıt süresi (ms), Throughput (req/sec), CPU Kullanımı (%) ve RAM Tüketimi (MB).

## 📁 Proje Yapısı
- `/src`: .NET Core API kaynak kodları ve bağımlılık simülasyonları.
- `/tests`: JMeter .jmx test planları ve yük testi senaryoları.
- `/docs`: Dönem projesi rapor taslağı ve PDF çalışma planı.

## 🚀 Çalıştırma ve Test
API ayağa kaldırıldıktan sonra JMeter üzerinden ilgili `.jmx` dosyaları çalıştırılarak veri toplama süreci başlatılabilir. Sonuçlar raporun "Bulgular" kısmında analiz edilmek üzere CSV formatında dışa aktarılmaktadır.
