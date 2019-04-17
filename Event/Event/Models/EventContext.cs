using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Event.Models
{
    public partial class EventContext : DbContext
    {
        public EventContext()
        {
        }

        public EventContext(DbContextOptions<EventContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chatt> Chatt { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventChatMessage> EventChatMessage { get; set; }
        public virtual DbSet<EventChatt> EventChatt { get; set; }
        public virtual DbSet<JoinEvent> JoinEvent { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-T01K2IFS\\SQLEXPRESS;Database=Event;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Chatt>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.RecieverId).HasColumnName("RecieverID");

                entity.Property(e => e.SenderId).HasColumnName("SenderID");

                entity.HasOne(d => d.Reciever)
                    .WithMany(p => p.ChattReciever)
                    .HasForeignKey(d => d.RecieverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Chatt__RecieverI__3D5E1FD2");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.ChattSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Chatt__SenderID__3E52440B");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name).HasMaxLength(25);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__LocationI__412EB0B6");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__TypeID__4222D4EF");
            });

            modelBuilder.Entity<EventChatMessage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.EventChattId).HasColumnName("EventChattID");

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.EventChatt)
                    .WithMany(p => p.EventChatMessage)
                    .HasForeignKey(d => d.EventChattId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventChat__Event__4BAC3F29");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventChatMessage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventChat__UserI__4CA06362");
            });

            modelBuilder.Entity<EventChatt>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventChatt)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventChat__Event__48CFD27E");
            });

            modelBuilder.Entity<JoinEvent>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.JoinEvent)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__JoinEvent__Event__45F365D3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.JoinEvent)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__JoinEvent__UserI__44FF419A");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Passwd).IsRequired();

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
