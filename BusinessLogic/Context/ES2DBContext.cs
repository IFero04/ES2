using System;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Context;

public partial class ES2DBContext : DbContext
{
    public ES2DBContext() { }

    public ES2DBContext(DbContextOptions<ES2DBContext> options) : base(options) { }

    public virtual DbSet<Atividade> Atividades { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Ingresso> Ingressos { get; set; }

    public virtual DbSet<InscricaoAtividade> InscricaoAtividades { get; set; }

    public virtual DbSet<InscricaoEvento> InscricaoEventos { get; set; }

    public virtual DbSet<Mensagem> Mensagems { get; set; }

    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=es2;Username=es2;Password=es2;SearchPath=public;Port=6060;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("topology", "postgis_topology");

        modelBuilder.Entity<Atividade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("atividade_pkey");

            entity.ToTable("atividade");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Descricao)
                .HasMaxLength(500)
                .HasColumnName("descricao");
            entity.Property(e => e.Hora)
                .HasMaxLength(100)
                .HasColumnName("hora");
            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Atividades)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("atividade_id_evento_fkey");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("evento_pkey");

            entity.ToTable("evento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Capacidade).HasColumnName("capacidade");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .HasColumnName("categoria");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Descricao)
                .HasMaxLength(500)
                .HasColumnName("descricao");
            entity.Property(e => e.Hora)
                .HasMaxLength(100)
                .HasColumnName("hora");
            entity.Property(e => e.IdOrganizador).HasColumnName("id_organizador");
            entity.Property(e => e.Local)
                .HasMaxLength(100)
                .HasColumnName("local");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdOrganizadorNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdOrganizador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_id_organizador_fkey");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feedback_pkey");

            entity.ToTable("feedback");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .HasColumnName("comentario");
            entity.Property(e => e.IdInscricao).HasColumnName("id_inscricao");

            entity.HasOne(d => d.IdInscricaoNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdInscricao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feedback_id_inscricao_fkey");
        });

        modelBuilder.Entity<Ingresso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingresso_pkey");

            entity.ToTable("ingresso");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Preco).HasColumnName("preco");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Ingressos)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ingresso_id_evento_fkey");
        });

        modelBuilder.Entity<InscricaoAtividade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inscricao_atividade_pkey");

            entity.ToTable("inscricao_atividade");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.IdAtividade).HasColumnName("id_atividade");
            entity.Property(e => e.IdParticipante).HasColumnName("id_participante");

            entity.HasOne(d => d.IdAtividadeNavigation).WithMany(p => p.InscricaoAtividades)
                .HasForeignKey(d => d.IdAtividade)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inscricao_atividade_id_atividade_fkey");

            entity.HasOne(d => d.IdParticipanteNavigation).WithMany(p => p.InscricaoAtividades)
                .HasForeignKey(d => d.IdParticipante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inscricao_atividade_id_participante_fkey");
        });

        modelBuilder.Entity<InscricaoEvento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inscricao_evento_pkey");

            entity.ToTable("inscricao_evento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.IdParticipante).HasColumnName("id_participante");
            entity.Property(e => e.TipoIngresso).HasColumnName("tipo_ingresso");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.InscricaoEventos)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inscricao_evento_id_evento_fkey");

            entity.HasOne(d => d.IdParticipanteNavigation).WithMany(p => p.InscricaoEventos)
                .HasForeignKey(d => d.IdParticipante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inscricao_evento_id_participante_fkey");

            entity.HasOne(d => d.TipoIngressoNavigation).WithMany(p => p.InscricaoEventos)
                .HasForeignKey(d => d.TipoIngresso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inscricao_evento_tipo_ingresso_fkey");
        });

        modelBuilder.Entity<Mensagem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mensagem_pkey");

            entity.ToTable("mensagem");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.Mensagem1)
                .HasMaxLength(500)
                .HasColumnName("mensagem");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Mensagems)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mensagem_id_evento_fkey");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("utilizador_pkey");

            entity.ToTable("utilizador");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(50)
                .HasColumnName("senha");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("tipo");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
