﻿// <auto-generated />
using Crito.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Crito.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20230925131903_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Crito.Models.EmailListForm", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.HasKey("Email");

                    b.ToTable("EmailList", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
