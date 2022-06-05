﻿// <auto-generated />
using System;
using Chat.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chat.Dal.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20220605130622_Add_To_User_SmsChecker_Field")]
    partial class Add_To_User_SmsChecker_Field
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Chat.Dal.Entities.ChatDal", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("Chat.Dal.Entities.MessageDal", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ChatDalId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("Edited")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatDalId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Chat.Dal.Entities.UserDal", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("HashPassword")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("SmsChecker")
                        .HasColumnType("text");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserDalId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserDalId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatDalUserDal", b =>
                {
                    b.Property<Guid>("ChatsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("ChatsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ChatDalUserDal");
                });

            modelBuilder.Entity("Chat.Dal.Entities.MessageDal", b =>
                {
                    b.HasOne("Chat.Dal.Entities.ChatDal", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatDalId");
                });

            modelBuilder.Entity("Chat.Dal.Entities.UserDal", b =>
                {
                    b.HasOne("Chat.Dal.Entities.UserDal", null)
                        .WithMany("Users")
                        .HasForeignKey("UserDalId");
                });

            modelBuilder.Entity("ChatDalUserDal", b =>
                {
                    b.HasOne("Chat.Dal.Entities.ChatDal", null)
                        .WithMany()
                        .HasForeignKey("ChatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Dal.Entities.UserDal", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Chat.Dal.Entities.ChatDal", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Chat.Dal.Entities.UserDal", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
