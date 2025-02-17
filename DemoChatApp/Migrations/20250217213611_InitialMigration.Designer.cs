﻿// <auto-generated />
using System;
using DemoChatApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoChatApp.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20250217213611_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DemoChatApp.Models.Chat", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("DemoChatApp.Models.ChatMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ChatID")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sender")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ChatID");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("DemoChatApp.Models.ChatModelSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ChatID")
                        .HasColumnType("int");

                    b.Property<int>("SelectedModel")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ChatID")
                        .IsUnique();

                    b.ToTable("ChatModelSettings");
                });

            modelBuilder.Entity("DemoChatApp.Models.ModelParameters", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ChatModelSettingsID")
                        .HasColumnType("int");

                    b.Property<int>("FrequencyPenalty")
                        .HasColumnType("int");

                    b.Property<int>("MaxTokens")
                        .HasColumnType("int");

                    b.Property<int>("PresencePenalty")
                        .HasColumnType("int");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<float>("TopP")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.HasIndex("ChatModelSettingsID")
                        .IsUnique();

                    b.ToTable("ModelParameters");
                });

            modelBuilder.Entity("DemoChatApp.Models.ChatMessage", b =>
                {
                    b.HasOne("DemoChatApp.Models.Chat", "Chat")
                        .WithMany("ChatHistory")
                        .HasForeignKey("ChatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("DemoChatApp.Models.ChatModelSettings", b =>
                {
                    b.HasOne("DemoChatApp.Models.Chat", "Chat")
                        .WithOne("ModelSettings")
                        .HasForeignKey("DemoChatApp.Models.ChatModelSettings", "ChatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("DemoChatApp.Models.ModelParameters", b =>
                {
                    b.HasOne("DemoChatApp.Models.ChatModelSettings", "ChatModelSettings")
                        .WithOne("Parameters")
                        .HasForeignKey("DemoChatApp.Models.ModelParameters", "ChatModelSettingsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatModelSettings");
                });

            modelBuilder.Entity("DemoChatApp.Models.Chat", b =>
                {
                    b.Navigation("ChatHistory");

                    b.Navigation("ModelSettings")
                        .IsRequired();
                });

            modelBuilder.Entity("DemoChatApp.Models.ChatModelSettings", b =>
                {
                    b.Navigation("Parameters")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
