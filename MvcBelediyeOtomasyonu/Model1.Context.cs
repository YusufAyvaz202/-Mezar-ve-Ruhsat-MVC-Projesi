﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcBelediyeOtomasyonu
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Adalar> Adalar { get; set; }
        public virtual DbSet<KirsalYapiRuhsatlari> KirsalYapiRuhsatlari { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilar { get; set; }
        public virtual DbSet<KullaniciTipleri> KullaniciTipleri { get; set; }
        public virtual DbSet<Mezarliklar> Mezarliklar { get; set; }
        public virtual DbSet<MezarYerleri> MezarYerleri { get; set; }
        public virtual DbSet<Ruhsatlar> Ruhsatlar { get; set; }
        public virtual DbSet<RuhsatTipleri> RuhsatTipleri { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
