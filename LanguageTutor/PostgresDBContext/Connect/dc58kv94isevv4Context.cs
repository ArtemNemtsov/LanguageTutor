using Microsoft.EntityFrameworkCore;
using DBContext.Models;

namespace DBContext.Connect
{
    public partial class dc58kv94isevv4Context : DbContext
    {
        public dc58kv94isevv4Context()
        {
        }

        public dc58kv94isevv4Context(DbContextOptions<dc58kv94isevv4Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Topic> Topic { get; set; }
        public virtual DbSet<TtutorAudit> TtutorAudit { get; set; }
        public virtual DbSet<Word> Word { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=ec2-54-246-84-100.eu-west-1.compute.amazonaws.com; Port=5432;  sslmode=Require; Trust Server Certificate=true; Database=dc58kv94isevv4; User Id=xjltzheuxubsyt; Password=9994c10e4bf72451e50fd131eab3d2a9c2a2551d417a0fe3c2c67bac98aa79f1;CommandTimeout=300;Pooling=false;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.IdAccount)
                    .HasName("account_pkey");

                entity.ToTable("account", "system");

                entity.Property(e => e.IdAccount).HasColumnName("id_account");

                entity.Property(e => e.DateRegistration)
                    .HasColumnName("date_ registration")
                    .HasColumnType("timestamp(1) with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.HasKey(e => e.IdTopic)
                    .HasName("word_group_pkey");

                entity.ToTable("topic", "tutor");

                entity.HasIndex(e => e.NameTopic)
                    .HasName("word_group_name_group_key")
                    .IsUnique();

                entity.Property(e => e.IdTopic)
                    .HasColumnName("id_topic")
                    .HasDefaultValueSql("nextval('tutor.word_group_id_word_group_seq'::regclass)");

                entity.Property(e => e.NameTopic)
                    .HasColumnName("name_topic")
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<TtutorAudit>(entity =>
            {
                entity.HasKey(e => e.IdTutorAudit)
                    .HasName("ttutor_audit_pkey");

                entity.ToTable("ttutor_audit", "tutor");

                entity.Property(e => e.IdTutorAudit).HasColumnName("id_tutor_audit");

                entity.Property(e => e.CorrectTranslation)
                    .IsRequired()
                    .HasColumnName("correct_translation")
                    .HasMaxLength(70);

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.LanguageFrom)
                    .IsRequired()
                    .HasColumnName("language_from")
                    .HasMaxLength(40);

                entity.Property(e => e.LanguageTo)
                    .IsRequired()
                    .HasColumnName("language_to")
                    .HasMaxLength(40);

                entity.Property(e => e.NameLogin)
                    .IsRequired()
                    .HasColumnName("name_login")
                    .HasMaxLength(70);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("timestamp(6) with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.UserTranslation)
                    .HasColumnName("user_translation")
                    .HasMaxLength(70);

                entity.Property(e => e.Word)
                    .IsRequired()
                    .HasColumnName("word")
                    .HasMaxLength(70);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(e => e.IdWord)
                    .HasName("words_pkey");

                entity.ToTable("word", "tutor");

                entity.HasIndex(e => e.NameWord)
                    .HasName("words_word_key")
                    .IsUnique();

                entity.Property(e => e.IdWord).HasColumnName("id_word");

                entity.Property(e => e.IdTopic)
                    .HasColumnName("id_topic")
                    .HasDefaultValueSql("10");

                entity.Property(e => e.NameWord)
                    .IsRequired()
                    .HasColumnName("name_word")
                    .HasMaxLength(60);

                entity.HasOne(d => d.IdTopicNavigation)
                    .WithMany(p => p.Word)
                    .HasForeignKey(d => d.IdTopic)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("word_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
