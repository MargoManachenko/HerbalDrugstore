using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HerbalDrugstore.Data;

namespace HerbalDrugstore.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161130180702_Eighth")]
    partial class Eighth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HerbalDrugstore.Models.Compound", b =>
                {
                    b.Property<int>("DrugId");

                    b.Property<int>("HerbId");

                    b.Property<int?>("DrugId1")
                        .IsRequired();

                    b.Property<int?>("HerbId1")
                        .IsRequired();

                    b.Property<int>("Quantity");

                    b.HasKey("DrugId", "HerbId");

                    b.HasIndex("DrugId1");

                    b.HasIndex("HerbId1");

                    b.ToTable("Compound");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Drug", b =>
                {
                    b.Property<int>("DrugId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Indications")
                        .IsRequired();

                    b.Property<string>("Instruction")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<int>("Quantity");

                    b.HasKey("DrugId");

                    b.ToTable("Drug");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.DrugChanges", b =>
                {
                    b.Property<int>("ChangeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("DrugId");

                    b.Property<bool>("Increasing");

                    b.HasKey("ChangeId");

                    b.HasIndex("DrugId");

                    b.ToTable("DrugChanges");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Herb", b =>
                {
                    b.Property<int>("HerbId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("HerbId");

                    b.ToTable("Herb");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Lot", b =>
                {
                    b.Property<int>("LotId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DrugId");

                    b.Property<float>("Price");

                    b.Property<int>("Quantity");

                    b.Property<int>("SupplyId");

                    b.HasKey("LotId");

                    b.HasIndex("DrugId");

                    b.HasIndex("SupplyId");

                    b.ToTable("Lot");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 25);

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<int?>("DrugChangesChangeId");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.HasKey("SupplierId");

                    b.HasIndex("DrugChangesChangeId");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Supply", b =>
                {
                    b.Property<int>("SupplyId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfSupply");

                    b.Property<float>("Price");

                    b.Property<int>("SupplierId");

                    b.HasKey("SupplyId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Supply");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Compound", b =>
                {
                    b.HasOne("HerbalDrugstore.Models.Drug", "Drug")
                        .WithMany()
                        .HasForeignKey("DrugId1")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HerbalDrugstore.Models.Herb", "Herb")
                        .WithMany()
                        .HasForeignKey("HerbId1")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HerbalDrugstore.Models.DrugChanges", b =>
                {
                    b.HasOne("HerbalDrugstore.Models.Drug", "Drug")
                        .WithMany()
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Lot", b =>
                {
                    b.HasOne("HerbalDrugstore.Models.Drug", "Grug")
                        .WithMany()
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HerbalDrugstore.Models.Supply", "Supply")
                        .WithMany()
                        .HasForeignKey("SupplyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Supplier", b =>
                {
                    b.HasOne("HerbalDrugstore.Models.DrugChanges")
                        .WithMany("Supplier")
                        .HasForeignKey("DrugChangesChangeId");
                });

            modelBuilder.Entity("HerbalDrugstore.Models.Supply", b =>
                {
                    b.HasOne("HerbalDrugstore.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
