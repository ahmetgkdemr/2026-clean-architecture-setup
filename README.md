# Clean Architecture Setup – 2026

Bu repository, 2026 yılı itibarıyla geliştirilecek projeler için **standart bir başlangıç mimarisi** sunar.  
Amaç; sürdürülebilir, test edilebilir ve genişletilebilir bir yapı ile projelere hızlı ve tutarlı bir şekilde başlamaktır.

---

## [ ] Mimari Yaklaşım

### Architectural Pattern
- **Clean Architecture**

---

## [ ] Kullanılan Design Patterns

- **Result Pattern** – Operasyon sonuçlarını standart ve güvenli şekilde yönetmek  
- **Repository Pattern** – Veri erişimini soyutlamak  
- **CQRS Pattern** – Okuma ve yazma işlemlerini ayırmak  
- **Unit of Work Pattern** – Transaction yönetimini merkezileştirmek  

---

## [ ] Kullanılan Kütüphaneler

- **MediatR** – CQRS ve application layer iletişimi  
- **TS.Result** – Result Pattern implementasyonu  
- **Mapster** – Nesne eşleme (mapping)  
- **FluentValidation** – Request doğrulama  
- **EntityFrameworkCore** – ORM altyapısı  
- **TS.EntityFrameworkCore.GenericRepository** – Generic repository yapısı  
- **OData** – Gelişmiş sorgulama yetenekleri  
- **Scrutor** – Assembly scanning & dependency injectionQ  

---

## Kurulum ve Kullanım

Bu yapıyı kullanmak için depoyu kopyalayın.

```bash
git clone https://github.com/ahmetgkdemr/2026-clean-architecture-setup.git
cd 2026-clean-architecture-setup

> Not: Yapı, değiştirelebilir veya genişletilebilir.
