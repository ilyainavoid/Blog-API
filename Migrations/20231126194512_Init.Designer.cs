﻿// <auto-generated />
using System;
using BlogApi.Services;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlogApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231126194512_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BlogApi.Models.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DeleteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ParentCommentId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PostId")
                        .HasColumnType("uuid");

                    b.Property<int>("SubComments")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Community", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SubscribersCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Community");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.CommunityAdministrator", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommunityId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "CommunityId");

                    b.HasIndex("CommunityId");

                    b.ToTable("CommunityAdministrator");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.CommunitySubscriber", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommunityId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "CommunityId");

                    b.HasIndex("CommunityId");

                    b.ToTable("CommunitySubscriber");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("Author")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PostId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<int>("CommentsCount")
                        .HasColumnType("integer");

                    b.Property<Guid?>("CommunityId")
                        .HasColumnType("uuid");

                    b.Property<string>("CommunityName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("HasLike")
                        .HasColumnType("boolean");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<int>("LikesAmount")
                        .HasColumnType("integer");

                    b.Property<int>("ReadingTime")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CommunityId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.Property<Guid>("PostsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.HasKey("PostsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Comment", b =>
                {
                    b.HasOne("BlogApi.Models.Entities.User", null)
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.Comment", "ParentComment")
                        .WithMany("ChildComments")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId");

                    b.Navigation("ParentComment");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.CommunityAdministrator", b =>
                {
                    b.HasOne("BlogApi.Models.Entities.Community", "Community")
                        .WithMany("Administrators")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.User", "User")
                        .WithMany("ManagedCommunities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.CommunitySubscriber", b =>
                {
                    b.HasOne("BlogApi.Models.Entities.Community", "Community")
                        .WithMany("Subscribers")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Like", b =>
                {
                    b.HasOne("BlogApi.Models.Entities.User", null)
                        .WithMany("Likes")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.Post", null)
                        .WithMany("Likes")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Post", b =>
                {
                    b.HasOne("BlogApi.Models.Entities.User", null)
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.Community", null)
                        .WithMany("Posts")
                        .HasForeignKey("CommunityId");
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.HasOne("BlogApi.Models.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApi.Models.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Comment", b =>
                {
                    b.Navigation("ChildComments");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Community", b =>
                {
                    b.Navigation("Administrators");

                    b.Navigation("Posts");

                    b.Navigation("Subscribers");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("BlogApi.Models.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("ManagedCommunities");

                    b.Navigation("Posts");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
