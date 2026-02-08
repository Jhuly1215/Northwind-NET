using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Data.Entities;

namespace OrdersService.Api.Data;

public partial class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<customer> customers { get; set; }

    public virtual DbSet<employee> employees { get; set; }

    public virtual DbSet<invoice> invoices { get; set; }

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<order_detail> order_details { get; set; }

    public virtual DbSet<order_details_status> order_details_statuses { get; set; }

    public virtual DbSet<orders_status> orders_statuses { get; set; }

    public virtual DbSet<orders_tax_status> orders_tax_statuses { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<shipper> shippers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<customer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.city, "city");

            entity.HasIndex(e => e.company, "company");

            entity.HasIndex(e => e.first_name, "first_name");

            entity.HasIndex(e => e.last_name, "last_name");

            entity.HasIndex(e => e.state_province, "state_province");

            entity.HasIndex(e => e.zip_postal_code, "zip_postal_code");

            entity.Property(e => e.business_phone).HasMaxLength(25);
            entity.Property(e => e.city).HasMaxLength(50);
            entity.Property(e => e.company).HasMaxLength(50);
            entity.Property(e => e.country_region).HasMaxLength(50);
            entity.Property(e => e.email_address).HasMaxLength(50);
            entity.Property(e => e.fax_number).HasMaxLength(25);
            entity.Property(e => e.first_name).HasMaxLength(50);
            entity.Property(e => e.home_phone).HasMaxLength(25);
            entity.Property(e => e.job_title).HasMaxLength(50);
            entity.Property(e => e.last_name).HasMaxLength(50);
            entity.Property(e => e.mobile_phone).HasMaxLength(25);
            entity.Property(e => e.state_province).HasMaxLength(50);
            entity.Property(e => e.zip_postal_code).HasMaxLength(15);
        });

        modelBuilder.Entity<employee>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.city, "city");

            entity.HasIndex(e => e.company, "company");

            entity.HasIndex(e => e.first_name, "first_name");

            entity.HasIndex(e => e.last_name, "last_name");

            entity.HasIndex(e => e.state_province, "state_province");

            entity.HasIndex(e => e.zip_postal_code, "zip_postal_code");

            entity.Property(e => e.business_phone).HasMaxLength(25);
            entity.Property(e => e.city).HasMaxLength(50);
            entity.Property(e => e.company).HasMaxLength(50);
            entity.Property(e => e.country_region).HasMaxLength(50);
            entity.Property(e => e.email_address).HasMaxLength(50);
            entity.Property(e => e.fax_number).HasMaxLength(25);
            entity.Property(e => e.first_name).HasMaxLength(50);
            entity.Property(e => e.home_phone).HasMaxLength(25);
            entity.Property(e => e.job_title).HasMaxLength(50);
            entity.Property(e => e.last_name).HasMaxLength(50);
            entity.Property(e => e.mobile_phone).HasMaxLength(25);
            entity.Property(e => e.state_province).HasMaxLength(50);
            entity.Property(e => e.zip_postal_code).HasMaxLength(15);
        });

        modelBuilder.Entity<invoice>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.order_id, "fk_invoices_orders1_idx");

            entity.HasIndex(e => e.id, "id");

            entity.Property(e => e.amount_due)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");
            entity.Property(e => e.due_date).HasColumnType("datetime");
            entity.Property(e => e.invoice_date).HasColumnType("datetime");
            entity.Property(e => e.shipping)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");
            entity.Property(e => e.tax)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");

            entity.HasOne(d => d.order).WithMany(p => p.invoices)
                .HasForeignKey(d => d.order_id)
                .HasConstraintName("fk_invoices_orders");
        });

        modelBuilder.Entity<order>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.customer_id, "customer_id");

            entity.HasIndex(e => e.employee_id, "employee_id");

            entity.HasIndex(e => e.status_id, "fk_orders_orders_status");

            entity.HasIndex(e => e.id, "id");

            entity.HasIndex(e => e.ship_zip_postal_code, "ship_zip_postal_code");

            entity.HasIndex(e => e.shipper_id, "shipper_id");

            entity.HasIndex(e => e.tax_status_id, "tax_status");

            entity.Property(e => e.order_date).HasColumnType("datetime");
            entity.Property(e => e.paid_date).HasColumnType("datetime");
            entity.Property(e => e.payment_type).HasMaxLength(50);
            entity.Property(e => e.ship_city).HasMaxLength(50);
            entity.Property(e => e.ship_country_region).HasMaxLength(50);
            entity.Property(e => e.ship_name).HasMaxLength(50);
            entity.Property(e => e.ship_state_province).HasMaxLength(50);
            entity.Property(e => e.ship_zip_postal_code).HasMaxLength(50);
            entity.Property(e => e.shipped_date).HasColumnType("datetime");
            entity.Property(e => e.shipping_fee)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");
            entity.Property(e => e.status_id).HasDefaultValueSql("'0'");
            entity.Property(e => e.tax_rate).HasDefaultValueSql("'0'");
            entity.Property(e => e.taxes)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");

            entity.HasOne(d => d.customer).WithMany(p => p.orders)
                .HasForeignKey(d => d.customer_id)
                .HasConstraintName("fk_orders_customers");

            entity.HasOne(d => d.employee).WithMany(p => p.orders)
                .HasForeignKey(d => d.employee_id)
                .HasConstraintName("fk_orders_employees");

            entity.HasOne(d => d.shipper).WithMany(p => p.orders)
                .HasForeignKey(d => d.shipper_id)
                .HasConstraintName("fk_orders_shippers");

            entity.HasOne(d => d.status).WithMany(p => p.orders)
                .HasForeignKey(d => d.status_id)
                .HasConstraintName("fk_orders_orders_status");

            entity.HasOne(d => d.tax_status).WithMany(p => p.orders)
                .HasForeignKey(d => d.tax_status_id)
                .HasConstraintName("fk_orders_orders_tax_status");
        });

        modelBuilder.Entity<order_detail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.status_id, "fk_order_details_order_details_status_idx");

            entity.HasIndex(e => e.order_id, "fk_order_details_orders_idx");

            entity.HasIndex(e => e.id, "id");

            entity.HasIndex(e => e.inventory_id, "inventory_id");

            entity.HasIndex(e => e.product_id, "product_id");

            entity.HasIndex(e => e.purchase_order_id, "purchase_order_id");

            entity.Property(e => e.date_allocated).HasColumnType("datetime");
            entity.Property(e => e.quantity).HasPrecision(18, 4);
            entity.Property(e => e.unit_price)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");

            entity.HasOne(d => d.order).WithMany(p => p.order_details)
                .HasForeignKey(d => d.order_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_orders");

            entity.HasOne(d => d.product).WithMany(p => p.order_details)
                .HasForeignKey(d => d.product_id)
                .HasConstraintName("fk_order_details_products");

            entity.HasOne(d => d.status).WithMany(p => p.order_details)
                .HasForeignKey(d => d.status_id)
                .HasConstraintName("fk_order_details_order_details_status");
        });

        modelBuilder.Entity<order_details_status>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("order_details_status")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.status_name).HasMaxLength(50);
        });

        modelBuilder.Entity<orders_status>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("orders_status")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.status_name).HasMaxLength(50);
        });

        modelBuilder.Entity<orders_tax_status>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("orders_tax_status")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.tax_status_name).HasMaxLength(50);
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.product_code, "product_code");

            entity.Property(e => e.category).HasMaxLength(50);
            entity.Property(e => e.list_price).HasPrecision(19, 4);
            entity.Property(e => e.product_code).HasMaxLength(25);
            entity.Property(e => e.product_name).HasMaxLength(50);
            entity.Property(e => e.quantity_per_unit).HasMaxLength(50);
            entity.Property(e => e.standard_cost)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'");
        });

        modelBuilder.Entity<shipper>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.city, "city");

            entity.HasIndex(e => e.company, "company");

            entity.HasIndex(e => e.first_name, "first_name");

            entity.HasIndex(e => e.last_name, "last_name");

            entity.HasIndex(e => e.state_province, "state_province");

            entity.HasIndex(e => e.zip_postal_code, "zip_postal_code");

            entity.Property(e => e.business_phone).HasMaxLength(25);
            entity.Property(e => e.city).HasMaxLength(50);
            entity.Property(e => e.company).HasMaxLength(50);
            entity.Property(e => e.country_region).HasMaxLength(50);
            entity.Property(e => e.email_address).HasMaxLength(50);
            entity.Property(e => e.fax_number).HasMaxLength(25);
            entity.Property(e => e.first_name).HasMaxLength(50);
            entity.Property(e => e.home_phone).HasMaxLength(25);
            entity.Property(e => e.job_title).HasMaxLength(50);
            entity.Property(e => e.last_name).HasMaxLength(50);
            entity.Property(e => e.mobile_phone).HasMaxLength(25);
            entity.Property(e => e.state_province).HasMaxLength(50);
            entity.Property(e => e.zip_postal_code).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
