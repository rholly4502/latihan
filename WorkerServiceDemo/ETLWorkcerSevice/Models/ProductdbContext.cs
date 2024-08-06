﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ETLWorkcerSevice.Models;

public partial class ProductdbContext : DbContext
{
    public ProductdbContext()
    {
    }

    public ProductdbContext(DbContextOptions<ProductdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product2> Product2s { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=127.0.0.1,1455;database=Productdb;uid=student;pwd=123;TrustServerCertificate=Yes;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product2>(entity =>
        {
            entity.ToTable("Product2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nama)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
