﻿// <auto-generated />
using System;
using GreetingService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GreetingService.Infrastructure.Migrations
{
    [DbContext(typeof(GreetingDbContext))]
    [Migration("20220314101009_added-default-values")]
    partial class addeddefaultvalues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GreetingService.Core.Entities.Greeting", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("id");

                    b.HasIndex("From");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("To");

                    b.ToTable("Greetings");
                });

            modelBuilder.Entity("GreetingService.Core.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserEmail");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("GreetingService.Core.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApprovalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ApprovalExpiry")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<string>("ApprovalStatusNote")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GreetingService.Core.Entities.Greeting", b =>
                {
                    b.HasOne("GreetingService.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("From")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("GreetingService.Core.Entities.Invoice", null)
                        .WithMany("Greetings")
                        .HasForeignKey("InvoiceId");

                    b.HasOne("GreetingService.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("To")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GreetingService.Core.Entities.Invoice", b =>
                {
                    b.HasOne("GreetingService.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserEmail");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GreetingService.Core.Entities.Invoice", b =>
                {
                    b.Navigation("Greetings");
                });
#pragma warning restore 612, 618
        }
    }
}
