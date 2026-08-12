using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zakerly.Domain.Entities;

namespace Zakerly.Infrastructure.Persistence.Configurations;

public class LessonProgressConfiguration
    : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(
        EntityTypeBuilder<LessonProgress> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.LessonId)
            .IsRequired();

        builder.Property(x => x.IsCompleted)
            .IsRequired();

        builder.Property(x => x.CompletedAt)
            .IsRequired(false);

        builder.HasIndex(x => new
            {
                x.StudentId,
                x.LessonId
            })
            .IsUnique();

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lesson)
            .WithMany(x => x.Progresses)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}